using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
	[System.Serializable]
	public struct ToolContainer
	{
		public ToolData.ToolType type;		// �c�[���̏��
		public GameObject tool;				// �C���X�^���X����c�[���̎���
		public float recastTime;			// ���L���X�g����
	}


	[Header("�c�[���̐ݒu�͈�(���a)")]
	[SerializeField] private float m_toolSettingRange = 2.0f;

    [Header("�J�[�\���摜")]
    [SerializeField] private GameObject m_cursorImage = null;

	[Header("���C���[�}�X�N")]
	[SerializeField] private LayerMask m_layerMask;

	[Header("�ݒu�c�[��")]
	[SerializeField] private ToolContainer[] m_putTools;

	[Header("�c�[���̃f�[�^�x�[�X")]
	[SerializeField] private ToolDataBase m_data;

	[Header("�A�C�e��")]
	[SerializeField] private PlayerItem m_playerItem;

	// �c�[���ݒu�\
	private bool m_canPut = true;

	// �ݒu�c�[��
	private int m_toolType = 0;


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
    }

    // Update is called once per frame
    void Update()
    {
		// �c�[���̍X�V
		for (int i = 0; i < m_putTools.Length; i++)
		{
			if (m_putTools[i].recastTime > 0.0f)
			{
				m_putTools[i].recastTime -= Time.deltaTime;
			}
		}

		// �ݒu�\�ȏ�Ԃɂ��Ă���
		m_canPut = true;

		// �v���C���[�̈ʒu
		Vector2 playerPos = transform.position;

		// �}�E�X�̈ʒu���擾
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// �v���C���[�̈ʒu����}�E�X�̈ʒu�ւ̃x�N�g��
		Vector2 playerToMouse = mousePos - playerPos;
		// �������擾���Ă���
		float length = playerToMouse.magnitude;
		// �x�N�g�����K��
		playerToMouse.Normalize();

		// �v���C���[�ƃ}�E�X�J�[�\���̈ʒu���ݒu�͈͓�
		if (Vector2.Distance(playerPos, mousePos) < m_toolSettingRange)
		{
			//// �l�̌ܓ�����
			//mousePos = RoundHalfUp(mousePos);

			//// �c�[���̐ݒu�ʒu���}�E�X�J�[�\���̈ʒu�ɂ���
			//m_cursorImage.transform.position = mousePos;
		}
		else
		{
			// �͂��ő�͈͂ɐݒ�
			mousePos = playerPos + (playerToMouse * m_toolSettingRange);

			length = m_toolSettingRange;

			//// �l�̌ܓ�����
			//mousePos = RoundHalfUp(mousePos);

			//// �A�C�e���̐ݒu�ʒu
			//m_cursorImage.transform.position = mousePos;
		}

		// �v���C���[����̌@�����ւ�RayCast
		RaycastHit2D[] rayCast = Physics2D.RaycastAll(playerPos, playerToMouse, length, m_layerMask);

		// �u���b�N����̉����Ԃ�
		foreach (RaycastHit2D cast in rayCast)
		{
			// �u���b�N�ɓ�������
			if (cast.transform)
			{
				// �u���b�N�^�O���t���Ă���
				if (cast.transform.CompareTag("Block"))
				{
					// ���܂�h�~�œ��������ʂ̖@�������� 0.1 ���Z����
					mousePos = cast.point + (cast.normal * new Vector2(0.1f, 0.1f));

					break;
				}
			}
		}
		// �c�[���̏d�˒u�����
		foreach (RaycastHit2D cast in rayCast)
		{
			if (cast.transform)
			{
				// Tool�^�O���t���Ă���
				if (cast.transform.CompareTag("Tool"))
				{
					// �����O���b�h
					if (CheckSameGrid(mousePos, cast.transform.position))
					{
						// �ݒu�ł��Ȃ�����
						m_canPut = false;
					}
				}
			}
		}

		// �l�̌ܓ�����
		mousePos = RoundHalfUp(mousePos);

		if (m_text)
		{
			m_text.text += mousePos.ToString();
		}

		// �A�C�e���̐ݒu�ʒu
		m_cursorImage.transform.position = mousePos;

		// �f�o�b�O
		if (m_debug)
		{
			if (m_text != null)
			{
				m_text.text = m_putTools[m_toolType].type.ToString();
			}
		}

	}

	// �c�[���ݒu
	public void Put()
    {
		// �A�C�e�����ݒu�ł��Ȃ�
		if (!m_canPut)
		{
			Debug.Log("���łɃc�[��������");
			return;
		}

		// �I������Ă���A�C�e�����쐬�ł��Ȃ�
		if (!CheckCreate(m_toolType))
		{
			Debug.Log("�f�ޕs��");
			return;
		}

		// �N�[���^�C�����Ȃ�ݒu�ł��Ȃ�
		if (m_putTools[m_toolType].recastTime > 0.0f)
		{
			Debug.Log("�N�[���^�C����");
			return;
		}

		// �N�[���^�C���̐ݒ�
		m_putTools[m_toolType].recastTime = GetToolData(m_toolType).recastTime;

		// �A�C�e����u��
		GameObject tool = Instantiate(m_putTools[m_toolType].tool);
		// ���W�ݒ�
		tool.transform.position = m_cursorImage.transform.position;
		// �A�N�e�B�u�ɂ���
		tool.SetActive(true);

		// �f�ނ������
		m_playerItem.ConsumeMaterials(GetToolData(m_toolType));

    }

	// �c�[���ύX
	public void ChangeTool(int val)
	{
		// �ύX��̒l
		int change = m_toolType + val;

		// �ύX�オ 0 ����
		if (change < 0)
		{
			// ��Ԍ��̃c�[���ɂ���
			change = (int)ToolData.ToolType.OVER - 1;
		}
		// �ύX�オ�͈͊O
		else if (change >= (int)ToolData.ToolType.OVER)
		{
			change = 0;
		}

		// �c�[����ύX����
		m_toolType = change;
	}


	// �c�[���̃f�[�^���擾
	private ToolData GetToolData(int toolType)
	{
		// �c�[���̎�ޕ����[�v
		for (int i = 0; i < m_data.tool.Count; i++)
		{
			// �f�[�^�x�[�X�̃c�[���Ɛݒ肳��Ă���c�[��������
			if (m_data.tool[i].toolType == m_putTools[toolType].type)
			{
				return m_data.tool[i];
			}
		}

		return null;
	}

	// �c�[�����쐬�ł��邩�`�F�b�N
	private bool CheckCreate(int type)
	{
		ToolData data = GetToolData(type);

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

	// �l�̌ܓ�
	private Vector2 RoundHalfUp(Vector2 value)
	{
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);

		return value;
	}
	private Vector2Int RoundHalfUpInt(Vector2 value)
	{
		Vector2Int val = new()
		{
			x = (int)RoundHalfUp(value.x),
			y = (int)RoundHalfUp(value.y)
		};

		return val;
	}
	static float RoundHalfUp(float value)
	{
		// �����_�ȉ��̎擾
		float fraction = value - MathF.Floor(value);

		// �����_�ȉ���0.5����
		if (fraction < 0.5f)
		{
			// �؂�̂Ă�
			return MathF.Floor(value);
		}
		// �؂�グ��
		return MathF.Floor(value) + 1.0f;

	}

	// �����O���b�h�ɂ���
	private bool CheckSameGrid(Vector2 pos1, Vector2 pos2)
	{
		// �l�̌ܓ������l���擾(int)
		Vector2Int p1 = RoundHalfUpInt(pos1);
		Vector2Int p2 = RoundHalfUpInt(pos2);

		// �����O���b�h
		if (p1 == p2)
		{
			return true;
		}

		// �Ⴄ
		return false;
	}



	public ToolData.ToolType ToolType
	{
		get { return m_putTools[m_toolType].type; }
	}
    public float GetToolRecast(ToolData.ToolType type)
    {
        return m_putTools[(int)type].recastTime;
    }
}
