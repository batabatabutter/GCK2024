using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_RollRock01 : DungeonAttackRollRock
{
	[Header("���Ԋ�")]

	[Header("���ɕ��Ԑ�")]
	[SerializeField] private int m_lineUpCount = 5;

	[Header("�����N������")]
	[SerializeField] private float m_rankValue = 0.5f;


	// �U���J�n
	public override void Attack(Transform target, int attackRank = 1)
	{
		// �]��������̌���
		int rand = Random.Range(0, (int)MyFunction.Direction.RANDOM);

		// ��
		bool horizon = rand % 2 == 1;

		// ��̐�����
		int rockCount = m_lineUpCount + ((int)(attackRank * m_rankValue));

		// �U���ʒu
		Vector3 pos = target.position;
		// ���Z��
		Vector3 addPos;

		// ��
        if (horizon)
        {
			pos.y = target.position.y - rockCount / 2;
			addPos = Vector3.up;
        }
		// �c
		else
		{
			pos.x = target.position.x - rockCount / 2;
			addPos = Vector3.right;
		}

        // ���񂾊�𐶐�����
        for (int i = 0; i < rockCount; i++)
		{
			// �U������
			AttackOne(pos, rand);

			pos += addPos;
		}
	}
}
