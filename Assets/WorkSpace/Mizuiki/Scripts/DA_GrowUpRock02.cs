using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DA_GrowUpRock02 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// ����������̌���
		MyFunction.Direction dir = MyFunction.GetDirection(direction);

		AttackLump(target.position, dir, range, distance, rankValue, attackRank);
	}

	public void AttackLump(Vector3 pos, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// �^�[�Q�b�g����̋���
		Vector3 targetDistance = Vector3.zero;

		// ��
		bool horizon = (int)direction % 2 == 1;

		// ��̐�����
		int rockCount = (int)range + ((int)(attackRank * rankValue));

		// ���Z��
		Vector3 addPos;

		// �v���C���[�̉�����
		if (horizon)
		{
			// ��̐����n�߂�ʒu
			pos.y -= (rockCount / 2);
			// �₪���ԉ��Z����
			addPos = Vector3.up;
			// dir ���獶�E���v�Z
			targetDistance.x += ((int)direction - 2.0f) * distance;
		}
		// �v���C���[�̏c����
		else
		{
			// ��̐����n�߂�ʒu
			pos.x -= (rockCount / 2);
			// �₪���ԉ��Z����
			addPos = Vector3.right;
			// dir ����㉺���v�Z
			targetDistance.y += ((int)direction - 1.0f) * distance;
		}

		// ���񂾊�𐶐�����
		for (int i = 0; i < rockCount; i++)
		{
			// �U������
			AttackOne(pos + targetDistance);
			// ��̐�����ʒu���Z
			pos += addPos;
		}

	}
}
