using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public enum Type
    {
        STONE,  // 岩
        COAL,   // 石炭

        OVER
    }

    [Header("アイテムの種類")]
    [SerializeField] private Type m_itemType;

    [Header("スタック数")]
    [SerializeField] private int m_count;



    // Start is called before the first frame update
    void Start()
    {
        // リジッドボディがなければつける
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // スタック数が0だったら消す
        if (m_count <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    /// <summary>
    /// ドロップ
    /// </summary>
    /// <param name="count">ドロップ数</param>
    public void Drop(int count)
    {
        m_count = count;
    }

    /// <summary>
    /// 拾う
    /// </summary>
    /// <param name="count">拾える数</param>
    /// <returns>拾う数</returns>
    public int Picup(int count)
    {
        // 拾う数
		int picUpCount = m_count;

		// 全て拾える
		if (m_count <= count)
        {
            m_count = 0;
        }
        else
        {
            // 拾えるだけ拾う
            m_count -= count;
            picUpCount = count;
        }

        // 拾う数を返す
        return picUpCount;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        // アイテムに当たった
		if (collision.CompareTag("Item"))
        {
            MargeItem(collision.GetComponent<Item>());
            return;
        }

        // プレイヤーに当たった
        if (collision.CompareTag("Player"))
        {

            return;
        }
	}


    // アイテムをくっつける
    void MargeItem(Item item)
    {
        // アイテムスクリプトが存在しない
        if (item == null)
            return;

        // アイテムの種類が違う
        if (item.ItemType != m_itemType)
            return;

        // スタック数が 0 以下
        if (m_count <= 0)
            return;

        // くっつける
        m_count += item.Count;

        // くっつけたアイテムを削除
        item.Count = 0;

    }


	public Type ItemType
    {
        get { return m_itemType; }
    }

    public int Count
    {
        get { return m_count; }
        set { m_count = value; }
    }
}
