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

    [Header("自分自身の発する光源レベル")]
    [SerializeField] private int m_lightLevel = 0;
    [Header("受けている光源レベル")]
    [SerializeField] private int m_receiveLightLevel = 0;

    [Header("スプライトレンダー")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    // ブロックが破壊されている
    private bool m_isBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        // スプライトレンダーがなければ取得
        if (!m_spriteRenderer)
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
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
        
        // 受けている明るさレベルに応じて色を設定
        if (m_receiveLightLevel > 0)
        {
            // 透明度
            float alpha = m_receiveLightLevel / 7.0f * 100.0f;
            // 色を設定
            m_spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
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

    // 自身の持つ光源レベル
    public int LightLevel
    {
        get { return m_lightLevel; }
    }

    // 受けている明るさ
    public int ReceiveLightLevel
    {
        get { return m_receiveLightLevel; }
        set { m_receiveLightLevel = value; }
    }

}
