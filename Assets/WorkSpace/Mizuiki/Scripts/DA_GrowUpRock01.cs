using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GrowUpRock01 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// 攻撃の中心
		Vector2Int center = new ((int)range / 2, (int)range / 2);

		// ターゲットの位置
		Vector3 targetPos = target.position;
		targetPos.x -= center.x;
		targetPos.y += center.y;

		// ターゲットの周りに攻撃を出す
		for (int y = 0; y < (int)range; y++)
		{
			for (int x = 0; x < (int)range; x++)
			{
				// ターゲットの位置には発生しない
				if (y == center.y && x == center.x)
					continue;

				// 攻撃発生位置
				Vector3 attackPos = new(targetPos.x + x, targetPos.y - y, 0);

				// 攻撃生成
				AttackOne(attackPos, attackRank);
			}
		}
	}
}
