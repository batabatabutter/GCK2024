using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public enum Type
    {
        STONE,  // ��
        COAL,   // �ΒY

        OVER
    }

    [Header("�A�C�e���̎��")]
    [SerializeField] private Type m_itemType;

    [Header("�X�^�b�N��")]
    [SerializeField] private int m_count;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �X�^�b�N����0�����������
        if (m_count <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    /// <summary>
    /// �h���b�v
    /// </summary>
    /// <param name="count">�h���b�v��</param>
    public void Drop(int count)
    {
        m_count = count;
    }

    /// <summary>
    /// �E��
    /// </summary>
    /// <param name="count">�E���鐔</param>
    /// <returns>�E����</returns>
    public int Picup(int count)
    {
        // �S�ďE����
        if (m_count <= count)
        {
            m_count = 0;
            return m_count;
        }
        else
        {
            // �E���邾���E��
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
