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
        // 全て拾える
        if (m_count <= count)
        {
            m_count = 0;
            return m_count;
        }
        else
        {
            // 拾えるだけ拾う
            m_count -= count;
            return count;
        }

    }


    public Type ItemType
    {
        get { return m_itemType; }
    }

    public int Count
    {
        get { return m_count; }
    }
}
