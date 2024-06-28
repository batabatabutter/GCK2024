using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Item : MonoBehaviour
{
    [Header("�A�C�e���̎��")]
    [SerializeField] private ItemData.ItemType m_itemType;

    [Header("�X�^�b�N��")]
    [SerializeField] private int m_count;

    [Header("�v���C���[���E��������܂ł̎���")]
    [SerializeField] private float m_picupTime = 1.0f;
    [Header("�A�C�e���h���b�v�̓��������鎞��")]
    [SerializeField] private float m_dropTime = 0.5f;

    // �A�C�e���̌��̈ʒu
    private Vector3 m_originalPos = Vector3.zero;
    // �A�C�e���̃h���b�v����
    private Vector3 m_dropDirection = Vector3.up;
    // �v���C���[�̃g�����X�t�H�[��
    private Transform m_player = null;
    // �A�C�e���̈ړ�����
    private float m_moveTime = 0.0f;



    // Start is called before the first frame update
    void Awake()
    {
        //// ���W�b�h�{�f�B���Ȃ���΂���
        //if (GetComponent<Rigidbody2D>() == null)
        //{
        //    Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
        //    rigidbody.isKinematic = true;
        //}

        // ���̈ʒu��ݒ肷��
        m_originalPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // �X�^�b�N����0�����������
        if (m_count <= 0)
        {
            Destroy(gameObject);
        }
        
        // �A�C�e���̃h���b�v����
        if (m_moveTime > m_picupTime)
        {
            m_moveTime -= Time.deltaTime;

            transform.position += m_dropDirection * Time.deltaTime * 0.5f;
        }
        // �A�C�e���̈ړ�
        else if (m_moveTime > 0.0f)
        {
            m_moveTime -= Time.deltaTime;

            // �v���C���[�̃g�����X�t�H�[��������
            if (m_player != null)
            {
                // ����
                float t = m_moveTime / m_picupTime;
                // ���W�̐ݒ�
                transform.position = Vector3.Lerp(m_player.position, m_originalPos, t);
            }
        }

    }

    /// <summary>
    /// �h���b�v
    /// </summary>
    /// <param name="count">�h���b�v��</param>
    public void Drop(int count)
    {
        // �h���b�v��
        m_count = count;
        // �h���b�v����
        m_dropDirection= new (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        m_dropDirection.Normalize();
        // �h���b�v�̎���
        m_moveTime = m_dropTime + m_picupTime;
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

	// �g�����X�t�H�[���̐ݒ�
	public void SetPlayerTransform(Transform player, bool overwrite = false)
	{
		// �g�����X�t�H�[���㏑��
		if (overwrite)
		{
			m_player = player;

			return;
		}

		// �g�����X�t�H�[�����ݒ肳��Ă��Ȃ�
		if (m_player == null)
		{
			m_player = player;
            m_moveTime = m_picupTime;
		}

        // ���̈ʒu��ݒ�
        m_originalPos = transform.position;

	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �X�^�b�N���� 0 �ȉ�
		if (m_count <= 0)
			return;

		// �A�C�e���ɓ�������
		if (collision.CompareTag("Item"))
        {
            // �A�C�e������������
            MargeItem(collision.GetComponent<Item>());

            return;
        }

        // �v���C���[�ɓ�������
        if (collision.CompareTag("Player"))
        {
            // �E����
            PickedUp(collision.GetComponent<Player>());

            return;
        }
	}


    // �A�C�e������������
    private void MargeItem(Item item)
    {
        // �A�C�e���X�N���v�g�����݂��Ȃ�
        if (item == null)
            return;

        // �A�C�e���̎�ނ��Ⴄ
        if (item.ItemType != m_itemType)
            return;

        // ��������
        m_count += item.Count;

        // ���������A�C�e�����폜
        item.Count = 0;

    }

    // �E����
    private void PickedUp(Player player)
    {
        // �v���C���[�X�N���v�g�����݂��Ȃ�
        if (player == null)
            return;

        // �����A�C�e���X�N���v�g�̎擾
        if (player.transform.Find("Item").TryGetComponent(out PlayerItem playerItem))
        {
            // �E���������Ԃ��Ă���
            m_count -= playerItem.PicUp(m_itemType, m_count);
        }

        m_player = null;
    }




	public ItemData.ItemType ItemType
    {
        get { return m_itemType; }
        set { m_itemType = value; }
    }

    public int Count
    {
        get { return m_count; }
        set { m_count = value; }
    }
}
