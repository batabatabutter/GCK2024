using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : ObjectAffectLight
{
    [Header("ブロックの耐久")]
    [SerializeField] private float m_blockEndurance = 100;

    [Header("破壊不可")]
    [SerializeField] private bool m_dontBroken = false;

    [Header("ブロックの情報")]
    [SerializeField] private BlockData m_blockData = null;

    [Header("アイテムのデータベース")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;

    //  地面のライト情報
    private Ground m_ground = null;

    // ブロックが破壊されている
    private bool m_isBroken = false;


	private void Awake()
	{
		// データベースが設定されてない
		if (m_itemDataBase == null)
		{
			Debug.Log(gameObject.name + "のアイテムデータベースを設定してね");
		}
	}

    // Update is called once per frame
    void Update()
    {
		// 自分自身を破壊する
		if (m_isBroken)
        {
            Destroy(gameObject);
        }

        //  光源処理
        if (Ground)
            ReceiveLightLevel = m_ground.ReceiveLightLevel;
    }

	/// <summary>
	/// 採掘ダメージ
	/// </summary>
	/// <param name="power"></param>
	/// <returns>ダメージを与えたか</returns>
	public virtual bool AddMiningDamage(float power, int dropCount = 1)
    {
        // 破壊不可能ブロックの場合は処理しない
        if (m_dontBroken)
            return false;

        // 採掘ダメージ加算
        m_blockEndurance -= power;

        // 耐久が0になった
        if (m_blockEndurance <= 0.0f)
        {
            BrokenBlock(dropCount);
        }

        return true;
    }

	// アイテムドロップ
	public virtual void DropItem(int dropCount = 1)
	{
        foreach (BlockData.DropItems dropItem in m_blockData.DropItem)
        {
            // 0 ~ 1乱数取得
            float random = Random.value;

            // 乱数がドロップ確率より大きい
            if (dropItem.rate < random)
                continue;

            // アイテムのデータを取得
            ItemData data = MyFunction.GetItemData(m_itemDataBase, dropItem.type);

            // データがない場合はドロップしない
            if (data == null)
                continue;

			// アイテムのゲームオブジェクトを生成
			GameObject obj = Instantiate(data.Prefab, transform.position, Quaternion.identity);

            // 名前を変える
            obj.name = "Material_" + dropItem.type.ToString();

            // アイテムがドロップしたときの処理
            if (obj.TryGetComponent(out Item item))
            {
                // 種類の設定
                item.ItemType = dropItem.type;
                // ドロップ数の設定
                item.Drop(dropItem.count * dropCount);
            }

            //  明るさが次第
            obj.GetComponent<ObjectAffectLight>().BrightnessFlag = BrightnessFlag;

            // 画像を設定
            if (obj.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.sprite = data.Sprite;
            }
        }
	}



	/// <summary>
	/// ブロックを破壊
	/// </summary>
	/// <returns>ブロックが壊れた</returns>
	public bool BrokenBlock(int dropCount = 1)
	{
		// 破壊不可能ブロックの場合は処理しない
		if (m_dontBroken)
			return false;

        // すでに破壊されている
        if (m_isBroken)
            return false;

		// アイテムドロップ
		DropItem(dropCount);

		// 自身を削除
		Destroy(gameObject);
        m_isBroken = true;

        return true;
	}


    // 耐久力
    public float Endurance
    {
        set { m_blockEndurance = value; }
    }

    // 破壊不可能か
    public bool DontBroken
    {
        get { return m_dontBroken; }
        set { m_dontBroken = value; }
    }

    // ブロックデータ
    public BlockData BlockData
    {
        get { return m_blockData; }
        set { m_blockData = value; }
    }

    //  地面
    public Ground Ground 
    { 
        get { return m_ground; } 
        set { m_ground = value; }
    }

}
