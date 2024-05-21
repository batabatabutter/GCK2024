using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_FallRock03 : DA_FallRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// 塊のサイズ
		int massSize = (int)range + (int)(attackRank * rankValue);
		int massRange = massSize / 2;
		// ターゲットのグリッド取得
		Vector2 targetGrid = MyFunction.RoundHalfUp(target.position);
		Vector2 startPos = new(targetGrid.x - massRange, targetGrid.y - massRange);
		// パターンその2
		for (int y = 0; y < massSize; y++)
		{
			for (int x = 0; x < massSize; x++)
			{
				if (x != 0 && y != 0 &&
					x != massSize - 1 && y != massSize - 1)
					continue;

				// 攻撃発生位置
				Vector3 attackPos = startPos + new Vector2(x, y);
				// 攻撃発生
				AttackOne(attackPos);
			}
		}
	}
}
