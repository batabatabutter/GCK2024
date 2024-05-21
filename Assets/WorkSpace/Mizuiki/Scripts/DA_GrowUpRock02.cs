using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GorwUpRock02 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// 生える方向の決定
		int dir = (int)MyFunction.GetDirection(direction);

		// ターゲットからの距離
		Vector3 distance = Vector3.zero;

		// 横
		bool horizon = dir % 2 == 1;

		// 岩の生成数
		int rockCount = 5 + ((int)(attackRank * rankValue));

		// 攻撃位置
		Vector3 pos = target.position;
		// 加算量
		Vector3 addPos;

		// 横
        if (horizon)
        {
			pos.y = target.position.y - (rockCount / 2);
			addPos = Vector3.up;
			distance.x += (dir - 2.0f) * range;
        }
		// 縦
		else
		{
			pos.x = target.position.x - (rockCount / 2);
			addPos = Vector3.right;
			distance.y += (dir - 1.0f) * range;
		}

        // 並んだ岩を生成する
        for (int i = 0; i < rockCount; i++)
		{
			// 攻撃生成
			AttackOne(pos + distance);

			pos += addPos;
		}
	}
}
