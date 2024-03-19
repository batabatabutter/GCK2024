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
        // ���W�b�h�{�f�B���Ȃ���΂���
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
        }

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
        // �E����
		int picUpCount = m_count;

		// �S�ďE����
		if (m_count <= count)
        {
            m_count = 0;
        }
        else
        {
            // �E���邾���E��
            m_count -= count;
            picUpCount = count;
        }

        // �E������Ԃ�
        return picUpCount;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        // �A�C�e���ɓ�������
		if (collision.CompareTag("Item"))
        {
            MargeItem(collision.GetComponent<Item>());
            return;
        }

        // �v���C���[�ɓ�������
        if (collision.CompareTag("Player"))
        {

            return;
        }
	}


    // �A�C�e������������
    void MargeItem(Item item)
    {
        // �A�C�e���X�N���v�g�����݂��Ȃ�
        if (item == null)
            return;

        // �A�C�e���̎�ނ��Ⴄ
        if (item.ItemType != m_itemType)
            return;

        // �X�^�b�N���� 0 �ȉ�
        if (m_count <= 0)
            return;

        // ��������
        m_count += item.Count;

        // ���������A�C�e�����폜
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
