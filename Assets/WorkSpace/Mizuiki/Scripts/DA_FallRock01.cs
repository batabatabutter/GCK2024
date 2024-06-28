using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_FallRock01 : DA_FallRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// 塊のサイズ
		int massSize = (int)range + (int)(attackRank * rankValue);
		int massRange = massSize / 2;
		// ターゲットのグリッド取得
		Vector2Int targetGrid = MyFunction.RoundHalfUpInt(target.position);
		// パターンその2
		for (int y = targetGrid.y - massRange; y <= targetGrid.y + massRange; y++)
		{
			for (int x = targetGrid.x - massRange; x <= targetGrid.x + massRange; x++)
			{
				// 攻撃発生位置
				Vector3 attackPos = new(x, y, 0);
				// 攻撃発生
				AttackOne(attackPos);
			}
		}
	}


}
