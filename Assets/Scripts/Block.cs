using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("ブロックの耐久")]
    [SerializeField] private float m_blockEndurance = 100;

    [Header("破壊不可")]
    [SerializeField] private bool m_dontBroken = false;

    [Header("ドロップするアイテム")]
    [SerializeField] private List<GameObject> m_dropItems = new List<GameObject>();

    [Header("自分自身の光源レベル")]
    [SerializeField] private int m_lightLevel = 0;

    [Header("マップ表示用のオブジェクト")]
    [SerializeField] private GameObject m_mapObject = null;
    [SerializeField] private GameObject m_mapBlind = null;
    [SerializeField] private Color m_blockColor = Color.white;
    [SerializeField] private int m_order = 0;

    // ブロックが破壊されている
    private bool m_isBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        if (m_mapObject)
        {
            // マップオブジェクトの生成
            GameObject mapObj = Instantiate(m_mapObject, transform);
            // 色の設定
            mapObj.GetComponent<SpriteRenderer>().color = m_blockColor;
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = m_order;
            mapObj.GetComponent<MapObject>().BlockColor = m_blockColor;
            // スプライトの設定
            mapObj.GetComponent<MapObject>().ParentSprite = gameObject.GetComponent<SpriteRenderer>();
        }
        if (m_mapBlind)
        {
            // マップの目隠し生成
            Instantiate(m_mapBlind, transform);
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
        
    }

	/// <summary>
	/// 採掘ダメージ
	/// </summary>
	/// <param name="power"></param>
	/// <returns></returns>
	public virtual bool AddMiningDamage(float power)
    {
        // 破壊不可能ブロックの場合は処理しない
        if (m_dontBroken)
            return false;

        // 採掘ダメージ加算
        m_blockEndurance -= power;

        // 耐久が0になった
        if (m_blockEndurance <= 0.0f)
        {
            return BrokenBlock();
        }

        return false;
    }

	// アイテムドロップ
	public virtual void DropItem(int count = 1)
	{
        foreach (GameObject dropItem in m_dropItems)
        {
            // アイテムのゲームオブジェクトを生成
            GameObject obj = Instantiate(dropItem);
            obj.transform.position = transform.position;

            // 明るさの概念を追加
            obj.AddComponent<ChangeBrightness>();

            // アイテムがドロップしたときの処理
            if (obj.TryGetComponent(out Item item))
            {
                item.Drop(count);
            }

        }

	}



	/// <summary>
	/// ブロックを破壊
	/// </summary>
	/// <returns>ブロックが壊れた</returns>
	public bool BrokenBlock()
	{
		// 破壊不可能ブロックの場合は処理しない
		if (m_dontBroken)
			return false;

        // すでに破壊されている
        if (m_isBroken)
            return false;

		// アイテムドロップ
		DropItem();

		// 自身を削除
		Destroy(gameObject);
        m_isBroken = true;

        return true;
	}



	// 破壊不可能か
	public bool DontBroken
    {
        get { return m_dontBroken; }
        set { m_dontBroken = value; }
    }

    // 光源レベル
    public int LightLevel
    {
        get { return m_lightLevel; }
    }

}
