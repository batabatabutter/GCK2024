using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	[Header("�v���C���[")]
	[SerializeField] private PlayerController m_player = null;

	[Header("�A�^�b�J�[")]
	[SerializeField] private DungeonAttacker m_attacker = null;

	[Header("�G")]
	[SerializeField] private EnemyManager m_enemy = null;

	[Header("�c�[��")]
	[SerializeField] private ToolManager m_tool = null;


	public void SetPause(bool pause)
	{
		// �v���C���[�̍X�V���~�߂�
		if (m_player)
		{
			m_player.enabled = pause;
		}

		// �A�^�b�J�[�̍X�V���~�߂�
		if (m_attacker)
		{
			m_attacker.enabled = pause;
		}

		// �G�̍X�V���~�߂�
		if (m_enemy)
		{
			m_enemy.SetEnabled(pause);
		}

		// �c�[���̍X�V���~�߂�
		if (m_tool)
		{
			m_tool.SetEnabled(pause);
		}

	}


}
