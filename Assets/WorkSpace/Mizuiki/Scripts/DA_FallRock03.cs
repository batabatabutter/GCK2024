using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_FallRock03 : DA_FallRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// ��̃T�C�Y
		int massSize = (int)range + (int)(attackRank * rankValue);
		int massRange = massSize / 2;
		// �^�[�Q�b�g�̃O���b�h�擾
		Vector2 targetGrid = MyFunction.RoundHalfUp(target.position);
		Vector2 startPos = new(targetGrid.x - massRange, targetGrid.y - massRange);
		// �p�^�[������2
		for (int y = 0; y < massSize; y++)
		{
			for (int x = 0; x < massSize; x++)
			{
				if (x != 0 && y != 0 &&
					x != massSize - 1 && y != massSize - 1)
					continue;

				// �U�������ʒu
				Vector3 attackPos = startPos + new Vector2(x, y);
				// �U������
				AttackOne(attackPos);
			}
		}
	}
}
