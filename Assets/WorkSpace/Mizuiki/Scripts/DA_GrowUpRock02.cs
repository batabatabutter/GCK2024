using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GorwUpRock02 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// ����������̌���
		int dir = (int)MyFunction.GetDirection(direction);

		// �^�[�Q�b�g����̋���
		Vector3 distance = Vector3.zero;

		// ��
		bool horizon = dir % 2 == 1;

		// ��̐�����
		int rockCount = 5 + ((int)(attackRank * rankValue));

		// �U���ʒu
		Vector3 pos = target.position;
		// ���Z��
		Vector3 addPos;

		// ��
        if (horizon)
        {
			pos.y = target.position.y - (rockCount / 2);
			addPos = Vector3.up;
			distance.x += (dir - 2.0f) * range;
        }
		// �c
		else
		{
			pos.x = target.position.x - (rockCount / 2);
			addPos = Vector3.right;
			distance.y += (dir - 1.0f) * range;
		}

        // ���񂾊�𐶐�����
        for (int i = 0; i < rockCount; i++)
		{
			// �U������
			AttackOne(pos + distance);

			pos += addPos;
		}
	}
}
