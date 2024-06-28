using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̈ړ��X�N���v�g")]
    [SerializeField] private PlayerMove m_playerMove = null;

    [Header("�v���C���[�̍̌@�X�N���v�g")]
    [SerializeField] private PlayerMining m_playerMining = null;

	[Header("�v���C���[�̐ݒu�X�N���v�g")]
	[SerializeField] private PlayerAction m_playerAction = null;

	[Header("�̌@����X�N���v�g")]
	[SerializeField] private CircularSaw m_circularSaw = null;

	// ����
	private Controls m_controls = null;


	// Start is called before the first frame update
	void Start()
    {
		// �R���g���[���̐���
		m_controls = new Controls();
		// �R���g���[���̗L����
		m_controls.Enable();

	}

	// Update is called once per frame
	void Update()
    {
		// �ړ�
		// ���͕����̎擾
		Vector2 velocity = m_controls.Player.Move.ReadValue<Vector2>();
		// �ړ��̌Ăяo��
		m_playerMove.MovePlayer(velocity);

		// �̌@
		if (m_controls.Player.Attack.IsPressed())		// ������Ă��
		{
			m_playerMining.Mining();
		}

		// �c�[���g�p
		if (m_controls.Player.Tool.WasPressedThisFrame())	// �������u��
		{
			m_playerAction.UseTool();
		}

		// ����
		if (m_controls.Player.Upgrade.WasPerformedThisFrame())
		{
			//m_playerAction.Upgrade();
			m_circularSaw.Upgrade();
		}

		// �c�[���ύX
		int scroll = (int)m_controls.Player.ChangeTool.ReadValue<float>() / 120;
		if (scroll != 0)
		{
			m_playerAction.ChangeTool(scroll);
		}

		// �c�[���̃��A�A�m�[�}���؂�ւ�
		if (m_controls.Player.SwitchTool.WasPerformedThisFrame())
		{
			m_playerAction.SwitchTool();
		}

	}
}
