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

	[Header("�c�[���X�N���v�g")]
	[SerializeField] private PlayerTool m_playerTool = null;
	[Header("�A�b�v�O���[�h�X�N���v�g")]
	[SerializeField] private PlayerUpgrade m_playerUpgrade = null;

	// �c�[���ݒu�\
	private bool m_canPut = true;


	// Start is called before the first frame update
	void Start()
    {
		// �c�[�����Ȃ���Ύ擾
		if (m_playerTool == null)
		{
			m_playerTool = GetComponent<PlayerTool>();
		}

		// �A�b�v�O���[�h���Ȃ���Ύ擾
		if (m_playerUpgrade == null)
		{
			m_playerUpgrade = GetComponent<PlayerUpgrade>();
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
			// �u���b�N�^�O���t���Ă���
			if (cast.transform.CompareTag("Block"))
			{
				// ���܂�h�~�œ��������ʂ̖@�������� 0.1 ���Z����
				mousePos = cast.point + (cast.normal * new Vector2(0.1f, 0.1f));

				break;
			}
		}
		// �c�[���̏d�˒u�����
		foreach (RaycastHit2D cast in rayCast)
		{
			// Tool�^�O���t���Ă��āA�����O���b�h
			if (cast.transform.CompareTag("Tool") &&
				MyFunction.CheckSameGrid(mousePos, cast.transform.position))
			{
				// �ݒu�ł��Ȃ�����
				m_canPut = false;
			}
		}

		// �l�̌ܓ�����
		mousePos = MyFunction.RoundHalfUp(mousePos);

		// �A�C�e���̐ݒu�ʒu
		m_cursorImage.transform.position = mousePos;

	}

	// ����
	public void Upgrade()
	{
		m_playerUpgrade.Upgrade();
	}

	// �c�[���̎g�p
	public void UseTool()
	{
		// �A�C�e�����ݒu�ł��Ȃ�
		if (!m_canPut)
		{
            Debug.Log("���łɃc�[��������");
			return;
		}

		// �c�[���̎g�p
		m_playerTool.UseTool(m_cursorImage.transform.position);

	}

	// �c�[���ύX
	public void ChangeTool(int val)
	{
        m_playerTool.ChangeTool(val);
		AudioManager.Instance.PlaySE(AudioDataID.Select);
    }

    // �c�[���̐؂�ւ�
    public void SwitchTool()
	{
        m_playerTool.SwitchTool();
        AudioManager.Instance.PlaySE(AudioDataID.Change);
    }



    // �I���c�[���̎擾
    public ToolData.ToolType ToolType
	{
		get { return m_playerTool.ToolType; }
	}
    public float GetToolRecast(ToolData.ToolType type)
    {
        return m_playerTool.RecastTime(type);
    }
}
