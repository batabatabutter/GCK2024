using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackTurn
{
	// �U������
	Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = null;

	// �����ŏ�������U���p�^�[��
	DungeonAttackPattern m_attackPattern;

	// ����������U���̃C���f�b�N�X
	int m_attackIndex = 0;

	// �^�C�}�[
	float m_timer = 0.0f;


	public void Attack(Transform target, int attackRank, float attackGrade)
	{
		// �C���f�b�N�X�͈̔͊m�F
		if (m_attackIndex >= m_attackPattern.AttackList.Count)
		{
			m_attackIndex = 0;
		}
		// ���Ԍo��
		m_timer -= Time.deltaTime;
		// ���̍U������
		if (m_timer < 0.0f)
		{
			// �U���p�^�[���擾
			DungeonAttackPattern.AttackPattern attack = m_attackPattern.AttackList[m_attackIndex];
			// �U������
			m_attacker[attack.type].Attack(target, attack.direction, attack.range, attack.rankValue, attackRank);
			// �N�[���^�C���ݒ�
			m_timer = attack.time * attackGrade;
			// �C���f�b�N�X�̃C���N�������g
			m_attackIndex++;
		}
	}


	// �A�^�b�J�[
	public Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> Attacker
	{
		set { m_attacker = value; }
	}
	// �U���p�^�[��
	public DungeonAttackPattern AttackPattern
	{
		set { m_attackPattern = value; }
	}


}
