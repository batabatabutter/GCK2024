using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
	[Header("�����i")]
	[SerializeField] private Dictionary<ItemData.ItemType, int> m_items = new();
	[Header("�ő吔")]
	[SerializeField] private int m_maxNormalCount = 99;
	[SerializeField] private int m_maxRareCount = 1;

	[Header("�A�C�e���̌��m�͈�(���a)")]
	[SerializeField] private float m_detectionRange = 3.0f;

	[Header("�A�C�e���̃f�[�^�x�[�X")]
	[SerializeField] private ItemDataBase m_itemDataBase = null;

	[Header("�f�o�b�O----------")]
	[SerializeField] private bool m_debug = false;
	[SerializeField] private Text m_text = null;

	// Start is called before the first frame update
	void Start()
    {
		// �R���C�_�[�̒ǉ�
		CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
		// ���a�̐ݒ�
		col.radius = m_detectionRange;
		// �g���K�[�ɐݒ�
		col.isTrigger = true;

		// �����A�C�e�����̏�����
		foreach(ItemData itemData in m_itemDataBase.item)
		{
			m_items[itemData.Type] = 0;

			// �f�o�b�O���I���ɂȂ��Ă����珊�������J���X�g������
			if (m_debug)
			{
				m_items[itemData.Type] = GetMaxCount(itemData.Type);
			}

		}
	}

	// Update is called once per frame
	void Update()
    {
		// �e�L�X�g������
		if (m_text != null)
		{
			m_text.text = "";

			foreach(KeyValuePair<ItemData.ItemType, int> item in m_items)
			{
				m_text.text += item.Key.ToString() + " : " + item.Value.ToString() + "\n";
			}

		}
    }


	// �A�C�e����������
	private void OnTriggerStay2D(Collider2D collision)
	{
		// �^�O���A�C�e���ȊO
		if (!collision.CompareTag("Item"))
			return;

		// �A�C�e���X�N���v�g�̎擾
		if (!collision.TryGetComponent(out Item item))
			return;

		// �A�C�e���̎��
		ItemData.ItemType itemType = item.ItemType;

		// �E���Ȃ�
		if (!CheckAcquirable(itemType))
			return;

		// �v���C���[�̃g�����X�t�H�[����ݒ肷��
		item.SetPlayerTransform(transform);

	}


	// �E���邩�m�F
	public bool CheckAcquirable(ItemData.ItemType itemType)
	{
		// �ő及����
		int maxCount = GetMaxCount(itemType);

		// ���������ő吔�ɒB���Ă��Ȃ�
		if (m_items[itemType] < maxCount)
		{
			return true;
		}

		return false;
	}
	// �A�C�e���̍ő吔���擾
	public int GetMaxCount(ItemData.ItemType itemType)
	{
		// �ʏ�A�C�e��
		if (itemType < ItemData.ItemType.BIRTHDAY_STONE)
		{
			return m_maxNormalCount;
		}
		// ���A�A�C�e��
		else
		{
			return m_maxRareCount;
		}
	}

	/// <summary>
	/// �A�C�e�����E��
	/// </summary>
	/// <param name="type">�A�C�e���̎��</param>
	/// <param name="count">�A�C�e���̐�</param>
	/// <returns>�E������</returns>
	public int PicUp(ItemData.ItemType type, int count)
	{
		// ���������ő�
		if (m_items[type] >= m_maxNormalCount)
			return 0;

		// �E����
		int picCount = m_maxNormalCount - m_items[type];

		// �E���鐔���A�C�e���̃X�^�b�N�����傫��
		if (picCount > count)
		{
			// �A�C�e���̃X�^�b�N���E��
			picCount = count;
		}

		// �A�C�e�����E��
		m_items[type] += picCount;

		// �E��������Ԃ�
		return picCount;
	}

	// �A�C�e���������
	public void ConsumeMaterials(ToolData data, int value = 1)
	{
		ConsumeMaterials(data.ItemMaterials, value);
	}
	public void ConsumeMaterials(Items[] items, int value = 1)
	{
		for (int i = 0; i < items.Length; i++)
		{
			// �A�C�e���̎�ގ擾
			ItemData.ItemType type = items[i].Type;

			// �A�C�e�������݂��Ȃ�
			if (!m_items.ContainsKey(type))
			{
				Debug.Log(type + "�����݂��Ȃ�");
				continue;
			}

			// [type] �� [count] �����
			m_items[type] -= items[i].count * value;
		}
	}

	// �A�C�e���̏������擾
	public int GetItemCount(ItemData.ItemType type)
	{
		// �A�C�e�������݂��Ȃ�
		if (!m_items.ContainsKey(type))
		{
			Debug.Log(type + "�����݂��Ȃ�");
			return�@0;
		}
		// ��������Ԃ�
		return m_items[type];
	}




	public Dictionary<ItemData.ItemType, int> Items
	{
		get { return m_items; }
	}


}
