using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
	[Header("�c�[���̐ݒu�͈�(���a)")]
	[SerializeField] private float m_toolSettingRange = 2.0f;

    [Header("�J�[�\���摜")]
    [SerializeField] private GameObject m_cursorImage = null;

	[Header("���C���[�}�X�N")]
	[SerializeField] private LayerMask m_layerMask;

	[Header("�c�[��")]
	[SerializeField] private PlayerTool m_playerTool;

	// �c�[���ݒu�\
	private bool m_canPut = true;

	// �ݒu�c�[��
	private ToolData.ToolType m_toolType = ToolData.ToolType.TOACH;


	[Header("�f�o�b�O---------------------------")]
	[SerializeField] private bool m_debug = false;
	[SerializeField] private Text m_text = null;


	// Start is called before the first frame update
	void Start()
    {
		// �c�[�����Ȃ���Ύ擾
		if (m_playerTool == null)
		{
			if (TryGetComponent(out PlayerTool tool))
			{
				m_playerTool = tool;
			}
		}

    }

    // Update is called once per frame
    void Update()
    {
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

		// �A�C�e���̐ݒu�ʒu
		m_cursorImage.transform.position = mousePos;

		// �f�o�b�O
		if (m_debug)
		{
			if (m_text != null)
			{
				m_text.text = m_toolType.ToString();
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
		if (!m_playerTool.CheckCreate(m_toolType))
		{
			Debug.Log("�f�ޕs��");
			return;
		}

		// �N�[���^�C�����Ȃ�ݒu�ł��Ȃ�
		if (!m_playerTool.Available(m_toolType))
		{
			Debug.Log("�N�[���^�C����");
			return;
		}

		// �c�[�����g�p����
		m_playerTool.UseTool(m_toolType, m_cursorImage.transform.position);

    }

	// �c�[���ύX
	public void ChangeTool(int val)
	{
		// �ύX��̒l
		ToolData.ToolType change = m_toolType - val;

		// �ύX�オ 0 ����
		if (change < 0)
		{
			// ��Ԍ��̃c�[���ɂ���
			change = ToolData.ToolType.OVER - 1;
		}
		// �ύX�オ�͈͊O
		else if (change >= ToolData.ToolType.OVER)
		{
			change = 0;
		}

		// �c�[����ύX����
		m_toolType = change;
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


	// �I���c�[���̎擾
	public ToolData.ToolType ToolType
	{
		get { return m_toolType; }
	}
    public float GetToolRecast(ToolData.ToolType type)
    {
        return m_playerTool.RecastTime(type);
    }
}
