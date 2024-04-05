using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTool : MonoBehaviour
{
	public class ToolContainer
	{
		public ToolData data = null;		// �c�[���̏��
		public bool create		= true;		// �쐬�\
		public bool available	= true;		// �g�p�\
		public bool isRecast	= false;	// ���L���X�g��
		public float recastTime = 0.0f;     // ���L���X�g����
	}

	[Header("�c�[���̃f�[�^�x�[�X")]
	[SerializeField] private ToolDataBase m_dataBase = null;

	[Header("�A�C�e��")]
	[SerializeField] private PlayerItem m_playerItem;

	[Header("�c�[��")]
	[SerializeField] private Dictionary<ToolData.ToolType, ToolContainer> m_tools = new();

	// �c�[���X�V�p�̃I�u�W�F�N�g
	private Dictionary<ToolData.ToolType, Tool> m_toolScripts = new();

	// ���A�c�[����I��
	private bool m_rare = false;

	// �I���c�[��
	private ToolData.ToolType m_toolType = 0;
	// �I�����A�c�[��
	private ToolData.ToolType m_toolTypeRare = ToolData.ToolType.RARE + 1;

	[Header("�f�o�b�O---------------------------")]
	[SerializeField] private bool m_debug = false;
	[SerializeField] private Text m_text = null;



	// Start is called before the first frame update
	void Start()
    {
		// �A�C�e�����Ȃ���Ύ擾
		if (m_playerItem == null)
		{
			if (TryGetComponent(out PlayerItem item))
			{
				m_playerItem = item;
			}
		}

		// �c�[���̍쐬
		foreach (ToolData toolData in m_dataBase.tool)
		{
			// �c�[���̎��
			ToolData.ToolType type = toolData.type;

			// �㏑���h�~
			if (m_tools.ContainsKey(type))
				continue;

			// �V���ȃc�[���̍쐬
			m_tools[type] = new()
			{
				// �f�[�^�x�[�X�̏���ݒ�
				data = toolData
			};

			// �T�|�[�g�c�[���X�V�p
			if (toolData.category == ToolData.ToolCategory.SUPPORT)
			{
				if (toolData.prefab)
				{
					m_toolScripts[type] = Instantiate(toolData.prefab.GetComponent<Tool>(), transform);
				}
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
		// �c�[���̍X�V
		foreach(ToolContainer tool in m_tools.Values)
		{
			// �c�[���̎�ގ擾
			ToolData.ToolType type = tool.data.type;

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

		// �f�o�b�O
		if (m_debug)
		{
			if (m_text != null)
			{
				if (m_rare)
				{
					m_text.text = m_toolTypeRare.ToString();
				}
				else
				{
					m_text.text = m_toolType.ToString();
				}
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

	// �c�[���ύX
	public void ChangeTool(int val)
	{
		// RARE���擾
		ToolData.ToolType rare = ToolData.ToolType.RARE;

		// �c�[���̎�ނ̃��X�g�擾
		List<ToolData.ToolType> typeList = new(m_tools.Keys);

		// ���A�c�[��
		if (m_rare)
		{
			// RARE �ȉ��̒l���폜
			typeList.RemoveAll(type => type <= rare);

			// �v�f���� 0 �Ȃ珈�����Ȃ�
			if (typeList.Count == 0)
				return;

			// ���ݑI�𒆂̃c�[���̃C���f�b�N�X
			int index = typeList.IndexOf(m_toolTypeRare);

			// �ύX��̒l
			int change = index - val;

			// �ύX�オ 0 ����
			while (change < 0)
			{
				// �擪����͂ݏo������
				change += typeList.Count;
			}
			// �ύX�オ�͈͊O
			while (change >= typeList.Count)
			{
				change -= typeList.Count;
			}

			// �ύX��̒l��ݒ�
			m_toolTypeRare = typeList[change];

		}
		// �ʏ�c�[��
		else
		{
			// RARE �ȏ�̒l���폜
			typeList.RemoveAll(type => type >= rare);

			// �v�f���� 0 �Ȃ珈�����Ȃ�
			if (typeList.Count == 0)
				return;

			// ���ݑI�𒆂̃c�[���̃C���f�b�N�X
			int index = typeList.IndexOf(m_toolType);

			// �ύX��̒l
			int change = index + val;

			// �ύX�オ 0 ����
			while (change < 0)
			{
				// �擪����͂ݏo������
				change += typeList.Count;

			}
			// �ύX�オ�͈͊O
			while (change >= typeList.Count)
			{
				// ��������͂ݏo������
				change -= typeList.Count;
			}

			// �ύX��̒l��ݒ�
			m_toolType = typeList[change];
		}

	}

	// �c�[���؂�ւ�
	public void SwitchTool()
	{
		m_rare = !m_rare;

	}

	// �c�[�����g�p����
	public void UseTool(Vector3 position)
	{
		if (m_rare)
		{
			UseTool(m_toolTypeRare, position);
		}
		else
		{
			UseTool(m_toolType, position);
		}
	}
	public void UseTool(ToolData.ToolType type, Vector3 position)
	{
		// �c�[�������݂��Ȃ�
		if (!m_tools.ContainsKey(type))
		{
            Debug.Log("�c�[�������݂��Ȃ���");
			return;
		}

		// �I������Ă���A�C�e�����쐬�ł��Ȃ�
		if (!CheckCreate(type))
		{
			Debug.Log("�f�ޕs��");
			return;
		}

		// �N�[���^�C�����Ȃ�ݒu�ł��Ȃ�
		if (!Available(type))
		{
			Debug.Log("�N�[���^�C����");
			return;
		}

		// �c�[���̃f�[�^�擾
		ToolData data = m_tools[type].data;
		// �c�[���̕���
		switch (data.category)
		{
			case ToolData.ToolCategory.PUT:			// �ݒu�^
				// �c�[���̐ݒu
				Put(data, position);
				// ���L���X�g���Ԃ̐ݒ�
				m_tools[type].recastTime = data.recastTime;
				m_tools[type].isRecast = true;
				break;

			case ToolData.ToolCategory.SUPPORT:		// �K���^
				// �c�[���g�p�̏������Ăяo��
				m_toolScripts[type].UseTool(gameObject);
				// ���L���X�g���Ԃ̐ݒ�
				m_tools[type].recastTime = data.recastTime;
				// �g�p�s�\�ɂ���
				m_tools[type].available = false;
				break;
		}

		// �f�ނ������
		m_playerItem.ConsumeMaterials(data);

	}

	// �ݒu����
	public void Put(ToolData data, Vector3 position, bool con = false)
	{
		// �v���n�u���ݒ肳��Ă��Ȃ���ΕԂ�
		if (!data.prefab)
			return;

		// �A�C�e����u��
		GameObject tool = Instantiate(data.prefab, position, Quaternion.identity);
		// �A�N�e�B�u�ɂ���(�O�̂���)
		tool.SetActive(true);

		// �f�ނ������
		if (con)
		{
			m_playerItem.ConsumeMaterials(data);
		}

	}

	// �c�[�����쐬�ł��邩�`�F�b�N
	public bool CheckCreate(ToolData.ToolType type)
	{
		// �c�[���̃f�[�^�擾
		ToolData data = m_tools[type].data;

		// �f�[�^������΍쐬�\���`�F�b�N
		if (data)
		{
			return CheckCreate(data);
		}

		// �I���c�[�������݂��Ȃ�
		return false;
	}
	public bool CheckCreate(ToolData data, int value = 1)
	{
		// �f�ނ̎�ޕ����[�v
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			// �A�C�e���̎��
			ItemData.Type type = data.itemMaterials[i].type;

			// �A�C�e�������݂��Ȃ�
			if (!m_playerItem.Items.ContainsKey(type))
			{
				Debug.Log("�A�C�e�������݂��Ȃ�");
				return false;
			}

			// �K�v��
			int count = data.itemMaterials[i].count * value;

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

	// �f�ނ̏���
	public void ConsumeMaterials(ToolData data, int value = 1)
	{
		m_playerItem.ConsumeMaterials(data, value);
	}


	// �c�[���̕��ނ��擾
	public ToolData.ToolCategory GetCategory(ToolData.ToolType type)
	{
		return m_tools[type].data.category;
	}
	// �c�[���̍쐬�ۂ��擾
	public bool GetCreate(ToolData.ToolType type)
	{
		return m_tools[type].create;
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

	// �I���c�[��
	public ToolData.ToolType ToolType
	{
		get { return m_toolType; }
	}
	// �I�����A�c�[��
	public ToolData.ToolType ToolTypeRare
	{
		get { return m_toolTypeRare; }
	}

}
