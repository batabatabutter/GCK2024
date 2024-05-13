using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_FallRock01 : DungeonAttackFallRock
{
	[Header("��̗���")]

	[Header("��̃T�C�Y")]
	[SerializeField] private int m_massSize = 3;

	[Header("�����N�̑�����")]
	[SerializeField] private float m_rankValue = 1.0f;


	public override void Attack(Transform target, int attackRank = 1)
	{
		// ��̃T�C�Y
		int massSize = m_massSize + (int)(attackRank * m_rankValue);
		int massRange = massSize / 2;
		// �^�[�Q�b�g�̃O���b�h�擾
		Vector2Int targetGrid = MyFunction.RoundHalfUpInt(target.position);
		// �p�^�[������2
		for (int y = targetGrid.y - massRange; y <= targetGrid.y + massRange; y++)
		{
			for (int x = targetGrid.x - massRange; x <= targetGrid.x + massRange; x++)
			{
				// �U�������ʒu
				Vector3 attackPos = new(x, y, 0);
				// �U������
				AttackOne(attackPos);
			}
		}
	}

	// �U���͈͂̐ݒ�
	public override void SetAttackRange(float range)
	{
		// ��T�C�Y�̐ݒ�
		m_massSize = (int)range;
	}

	// �����N�����ʂ̐ݒ�
	public override void SetRankValue(float value)
	{
		m_rankValue = value;
	}

}
