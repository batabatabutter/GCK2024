using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
	[Header("�����i")]
	[SerializeField] private Dictionary<ItemData.Type, int> m_items = new();
	[Header("�ő吔")]
	[SerializeField] private int m_maxCount = 99;

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
			m_items[itemData.type] = 0;

			// �f�o�b�O���I���ɂȂ��Ă����珊�������J���X�g������
			if (m_debug)
			{
				m_items[itemData.type] = m_maxCount;
			}

		}
	}

	// Update is called once per frame
	void Update()
    {
        if (m_debug)
		{
			// �e�L�X�g������
			if (m_text != null)
			{
				m_text.text = "";

				foreach(KeyValuePair<ItemData.Type, int> item in m_items)
				{
					m_text.text += item.Key.ToString() + " : " + item.Value.ToString() + "\n";
				}

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
		ItemData.Type itemType = item.ItemType;

		// �E���Ȃ�
		if (!CheckAcquirable(itemType))
			return;

		// �v���C���[�̃g�����X�t�H�[����ݒ肷��
		item.SetPlayerTransform(transform);

	}


	// �E���邩�m�F
	public bool CheckAcquirable(ItemData.Type itemType)
	{
		// ���������ő吔�ɒB���Ă��Ȃ�
		if (m_items[itemType] < m_maxCount)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// �A�C�e�����E��
	/// </summary>
	/// <param name="type">�A�C�e���̎��</param>
	/// <param name="count">�A�C�e���̐�</param>
	/// <returns>�E������</returns>
	public int PicUp(ItemData.Type type, int count)
	{
		// ���������ő�
		if (m_items[type] >= m_maxCount)
			return 0;

		// �E����
		int picCount = m_maxCount - m_items[type];

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
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			// �A�C�e���̎�ގ擾
			ItemData.Type type = data.itemMaterials[i].type;

			// �A�C�e�������݂��Ȃ�
			if (!m_items.ContainsKey(type))
			{
				Debug.Log(type + "�����݂��Ȃ�");
				continue;
			}

			// [type] �� [count] �����
			m_items[type] -= data.itemMaterials[i].count * value;
		}
	}

	// �A�C�e���̏������擾
	public int GetItemCount(ItemData.Type type)
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




	public Dictionary<ItemData.Type, int> Items
	{
		get { return m_items; }
	}


}
