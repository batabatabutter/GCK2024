using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("ブロックの耐久")]
    [SerializeField] private float m_blockEndurance = 100;

    [Header("破壊不可")]
    [SerializeField] private bool m_dontBroken = false;

    [Header("自分自身の発する光源レベル")]
    [SerializeField] private int m_lightLevel = 0;
    [Header("受けている光源レベル")]
    [SerializeField] private int m_receiveLightLevel = 0;
    private float m_receiveLightValue = 1.0f;    // 明度
	[Header("スプライトレンダー")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    [Header("ブロックの情報")]
    [SerializeField] private BlockData m_blockData = null;

    [Header("アイテムのデータベース")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;

    [Header("子のマップオブジェクト")]
    [SerializeField] private MapObject m_mapObject = null;

    // ブロックが破壊されている
    private bool m_isBroken = false;


	private void Awake()
	{
		// スプライトレンダーがなければ取得
		if (!m_spriteRenderer)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}
		// データベースが設定されてない
		if (m_itemDataBase == null)
		{
			Debug.Log(gameObject.name + "のアイテムデータベースを設定してね");
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        // 光源初期化
        ReceiveLightLevel = 0;

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

            //  明るさがついていたら
            if(GetComponent<ChangeBrightness>())
            {
                // 明るさの概念を追加
                var itemBr = obj.AddComponent<ChangeBrightness>();
                itemBr.SetPlayerTransform(GetComponent<ChangeBrightness>().GetPlayerTransform());
            }

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

    // 自身の持つ光源レベル
    public int LightLevel
    {
        get { return m_lightLevel; }
        set { m_lightLevel = value; }
    }

    // 受けている明るさ
    public int ReceiveLightLevel
    {
        get { return m_receiveLightLevel; }
        set
        {
			// 明るさレベルの設定(受けている光源レベルと自身の光源レベルを比較して大きいほうの明るさになる)
            m_receiveLightLevel = Mathf.Max(value, m_lightLevel);
			// 明度を計算
			m_receiveLightValue = m_receiveLightLevel / 7.0f;
            // 明度の値を 0 ~ 1 にクランプ
            m_receiveLightValue = Mathf.Clamp(m_receiveLightValue, 0.0f, 1.0f);
			// 色を設定
			m_spriteRenderer.color = new (m_receiveLightValue, m_receiveLightValue, m_receiveLightValue, 1.0f);
            // マップオブジェクトの明度を設定
            if (m_mapObject)
            {
				m_mapObject.SetValue(m_receiveLightValue);
			}
		}
	}

    // ブロックデータ
    public BlockData BlockData
    {
        get { return m_blockData; }
        set { m_blockData = value; }
    }

    // マップオブジェクト
    public MapObject MapObject
    {
        set { m_mapObject = value; }
    }

}
