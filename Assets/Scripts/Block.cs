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

    // ブロックが破壊されている
    private bool m_isBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        
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

    // 採掘ダメージ
    public virtual void AddMiningDamage(float power)
    {
        // 破壊不可能ブロックの場合は処理しない
        if (m_dontBroken)
            return;

        // 採掘ダメージ加算
        m_blockEndurance -= power;

        // 耐久が0になった
        if (m_blockEndurance <= 0.0f)
        {
            BrokenBlock();
        }
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



	// ブロックが壊れた
	public void BrokenBlock()
	{
		// 破壊不可能ブロックの場合は処理しない
		if (m_dontBroken)
			return;

        // すでに破壊されている
        if (m_isBroken)
            return;

		// アイテムドロップ
		DropItem();

		// 自身を削除
		Destroy(gameObject);
        m_isBroken = true;

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
