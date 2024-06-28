using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GrowUpRock01 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// �U���̒��S
		Vector2Int center = new ((int)range / 2, (int)range / 2);

		// �^�[�Q�b�g�̈ʒu
		Vector3 targetPos = target.position;
		targetPos.x -= center.x;
		targetPos.y += center.y;

		// �^�[�Q�b�g�̎���ɍU�����o��
		for (int y = 0; y < (int)range; y++)
		{
			for (int x = 0; x < (int)range; x++)
			{
				// �^�[�Q�b�g�̈ʒu�ɂ͔������Ȃ�
				if (y == center.y && x == center.x)
					continue;

				// �U�������ʒu
				Vector3 attackPos = new(targetPos.x + x, targetPos.y - y, 0);

				// �U������
				AttackOne(attackPos, attackRank);
			}
		}
	}
}
