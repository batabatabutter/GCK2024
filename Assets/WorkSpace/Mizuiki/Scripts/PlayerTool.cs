using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
	public class ToolContainer
	{
		public ToolData.ToolType type;      // �c�[���̏��
		public bool available	= true;		// �g�p�\
		public bool isRecast	= false;	// ���L���X�g��
		public float recastTime = 0.0f;     // ���L���X�g����
	}

	[Header("�c�[���̃f�[�^�x�[�X")]
	[SerializeField] private ToolDataBase m_dataBase = null;

	[Header("�A�C�e��")]
	[SerializeField] private PlayerItem m_playerItem;

	[Header("�ݒu�c�[��")]
	[SerializeField] private Dictionary<ToolData.ToolType, ToolContainer> m_tools = new();

	// �c�[���X�V�p�̋�̃I�u�W�F�N�g
	//private GameObject m_toolObject = null;
	private Dictionary<ToolData.ToolType, Tool> m_toolScripts = new();


	// Start is called before the first frame update
	void Start()
    {
		// �c�[���X�V�p�̃I�u�W�F�N�g�쐬
		//m_toolObject = new GameObject("Tools");

		// �A�C�e�����Ȃ���Ύ擾
		if (m_playerItem == null)
		{
			if (TryGetComponent(out PlayerItem item))
			{
				m_playerItem = item;
			}
		}

		// �c�[���̍쐬
		for (ToolData.ToolType type = ToolData.ToolType.TOACH; type < ToolData.ToolType.OVER; type++)
		{
			m_tools[type] = new ToolContainer();

			// �c�[���X�V�p
			if (m_dataBase.tool[(int)type].tool)
			{
				m_toolScripts[type] = Instantiate(m_dataBase.tool[(int)type].tool, transform);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
		// �c�[���̍X�V
		for (ToolData.ToolType type = ToolData.ToolType.TOACH; type < ToolData.ToolType.OVER; type++)
		{
			// ���L���X�g��
			if (m_tools[type].isRecast)
			{
				// ���Ԍo��
				m_tools[type].recastTime -= Time.deltaTime;
			}
			// ���L���X�g���Ԃ������Ă���
			if (m_tools[type].recastTime <= 0.0f)
			{
				// ���L���X�g�̃t���O���I�t�ɂ���
				m_tools[type].isRecast = false;
				// �g�p�\�ɂ���
				m_tools[type].available = true;
			}
		}


	}


	// �g�p�\
	public bool Available(ToolData.ToolType type)
	{
		// ���L���X�g��
		if(m_tools[type].available)
		{
			// �g�p�\
			return true;
		}
		// �g�p�s�\
		return false;
	}

	// �c�[�����g�p����
	public void UseTool(ToolData.ToolType type, Vector3 position)
	{
		// �Ăяo���֐����o�^����Ă���
		if (m_dataBase.tool[(int)type].tool)
		{
			// �c�[���g�p�̏������Ăяo��
			m_toolScripts[type].UseTool(gameObject);

			// ���L���X�g���Ԃ̐ݒ�
			m_tools[type].recastTime = m_dataBase.tool[(int)type].recastTime;

			// �g�p�s�\�ɂ���
			m_tools[type].available = false;

		}
		// �ݒu�c�[��
		else if (m_dataBase.tool[(int)type].objectPrefab)
		{
			// �A�C�e����u��
			GameObject tool = Instantiate(m_dataBase.tool[(int)type].objectPrefab, position, Quaternion.identity);
			// �A�N�e�B�u�ɂ���(�O�̂���)
			tool.SetActive(true);
			// ���L���X�g���Ԃ̐ݒ�
			m_tools[type].recastTime = m_dataBase.tool[(int)type].recastTime;
			m_tools[type].isRecast = true;
		}

		// �f�ނ������
		m_playerItem.ConsumeMaterials(m_dataBase.tool[(int)type]);

	}

	// �c�[�����쐬�ł��邩�`�F�b�N
	public bool CheckCreate(ToolData.ToolType type)
	{
		ToolData data = m_dataBase.tool[(int)type];

		if (data != null)
		{
			return CheckCreate(data);
		}

		// �I���c�[�������݂��Ȃ�
		return false;
	}
	private bool CheckCreate(ToolData data)
	{
		// �f�ނ̎�ޕ����[�v
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			ItemData.Type type = data.itemMaterials[i].type;
			int count = data.itemMaterials[i].count;

			// �����A�C�e�������K�v�f�ސ�����
			if (m_playerItem.Items[type] < count)
			{
				// �쐬�ł��Ȃ�
				return false;
			}

		}
		// �K�v�f�ސ��������Ă���
		return true;
	}



	// �c�[���̃��L���X�g���Ԃ̎擾
	public float RecastTime(ToolData.ToolType type)
	{
		return m_tools[type].recastTime;
	}

	// �c�[���̃��L���X�g��Ԃ̐ݒ�
	public void SetRecast(bool recast, ToolData.ToolType type)
	{
		m_tools[type].isRecast = recast;
	}

}
