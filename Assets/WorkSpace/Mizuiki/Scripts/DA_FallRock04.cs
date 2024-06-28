using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DA_FallRock04 : DA_FallRock
{
	[Header("---------- ���S����̗��� ----------")]
	[Header("���΃N���X")]
	[SerializeField] private DA_FallRock03 m_fallRock03;

	[Header("�U���t���O")]
	[SerializeField] private bool m_attack = false;

	[Header("���΂̊Ԋu(�b)")]
	[SerializeField] private float m_fallTime = 0.5f;
	private float m_fallTimer = 0.0f;

	[Header("���Ԑ�")]
	[SerializeField] private int m_range = 5;
	[Header("����������U���̃T�C�Y")]
	[SerializeField] private int m_attackSize = 1;

	[Header("�����N")]
	[SerializeField] private int m_rank = 0;
	[Header("�����N�ɉ�����������")]
	[SerializeField] private float m_rankValue = 1.0f;

	[Header("�^�[�Q�b�g�̈ʒu")]
	[SerializeField] private Vector3 m_targetPosition = Vector3.zero;


	private void Update()
	{
		// �U�����Ȃ�
		if (m_attack == false)
		{
			return;
		}

		// �U���͈͂𒴂��Ă���
		if (m_attackSize > m_range)
		{
			m_attack = false;
			return;
		}

		// ���Ԍo��
		m_fallTimer -= Time.deltaTime;

		// �U������
		if (m_fallTimer <= 0.0f)
		{
			// �U��
			m_fallRock03.AttackLump(m_targetPosition, m_attackSize, m_rankValue);
			// �U���T�C�Y���Z
			m_attackSize += 2;
			// �U������
			m_fallTimer = m_fallTime;
		}

	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// �^�[�Q�b�g�̈ʒu�擾
		m_targetPosition = target.position;
		// ���Ԑ��擾
		m_range = (int)range;
		// �U���T�C�Y
		m_attackSize = 1;
		// �����N�␳�l
		m_rankValue = rankValue;
		// �����N
		m_rank = attackRank;

		// �ꔭ�ڂ̍U��
		m_fallRock03.Attack(target, direction, m_attackSize, distance, rankValue, attackRank);

		// �U���t���O�I��
		m_attack = true;
		// �^�C�}�[�ݒ�
		m_fallTimer = m_fallTime;
		// �U���T�C�Y���Z
		m_attackSize += 2;
	}


}
