using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
	[Header("�����i")]
	[SerializeField] private Dictionary<Item.Type, int> m_items = new();
	[Header("�ő吔")]
	[SerializeField] private int m_maxCount = 99;

	[Header("�A�C�e���̌��m�͈�(���a)")]
	[SerializeField] private float m_detectionRange = 3.0f;
	[Header("�A�C�e���̋z�����ݑ��x(/s)")]
	[SerializeField] private float m_suctionSpeed = 0.5f;
	[Header("�A�C�e�����E��������͈�(���a)")]
	[SerializeField] private float m_picupRange = 1.0f;


	// Start is called before the first frame update
	void Start()
    {
		// �R���C�_�[�̒ǉ�
		CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
		// ���a�̐ݒ�
		col.radius = m_detectionRange;
		// �g���K�[�ɐݒ�
		col.isTrigger = true;

		for (Item.Type type = Item.Type.STONE; type < Item.Type.OVER; type++)
		{
			m_items[type] = 0;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }


	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �^�O���A�C�e���ȊO
		if (!collision.CompareTag("Item"))
			return;

		// �A�C�e���X�N���v�g�̎擾
		if (!collision.TryGetComponent<Item>(out Item item))
			return;

		// �A�C�e���̎��
		Item.Type itemType = item.ItemType;

		// �E���Ȃ�
		if (!CheckAcquirable(itemType))
			return;

		// ����
		float distance = Vector3.Distance(transform.position, collision.transform.position);

		// �E��
		if (distance < m_picupRange)
		{
			// �E���邾���E��
			m_items[itemType] += item.Picup(m_maxCount - m_items[itemType]);
			Debug.Log("picup");
		}
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		// �^�O���A�C�e���ȊO
		if (!collision.CompareTag("Item"))
			return;

		// �A�C�e������v���C���[�ւ̃x�N�g��
		Vector3 itemToPlayer = transform.position - collision.transform.position;
		// ���K��
		itemToPlayer.Normalize();

		// �v���C���[�ɋ߂Â�����
		collision.transform.position = collision.transform.position + (itemToPlayer * m_suctionSpeed * Time.deltaTime);
	}


	// �E���邩�m�F
	public bool CheckAcquirable(Item.Type itemType)
	{
		// ���������ő吔�ɒB���Ă��Ȃ�
		if (m_items[itemType] < m_maxCount)
		{
			return true;
		}

		return false;
	}

	// �A�C�e���̏������擾
	public int GetItemCount(Item.Type type)
	{
		return m_items[type];
	}




	public Dictionary<Item.Type, int> Items
	{
		get { return m_items; }
	}


}
