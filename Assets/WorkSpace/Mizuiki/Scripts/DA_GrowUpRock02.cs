using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GorwUpRock02 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// ����������̌���
		int dir = (int)MyFunction.GetDirection(direction);

		// �^�[�Q�b�g����̋���
		Vector3 targetDistance = Vector3.zero;

		// ��
		bool horizon = dir % 2 == 1;

		// ��̐�����
		int rockCount = (int)range + ((int)(attackRank * rankValue));

		// �U���ʒu
		Vector3 pos = target.position;
		// ���Z��
		Vector3 addPos;

		// �v���C���[�̉�����
        if (horizon)
        {
			// ��̐����n�߂�ʒu
			pos.y = target.position.y - (rockCount / 2);
			// �₪���ԉ��Z����
			addPos = Vector3.up;
			// dir ���獶�E���v�Z
			targetDistance.x += (dir - 2.0f) * distance;
        }
		// �v���C���[�̏c����
		else
		{
			// ��̐����n�߂�ʒu
			pos.x = target.position.x - (rockCount / 2);
			// �₪���ԉ��Z����
			addPos = Vector3.right;
			// dir ����㉺���v�Z
			targetDistance.y += (dir - 1.0f) * distance;
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
