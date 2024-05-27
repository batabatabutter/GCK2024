using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GorwUpRock02 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// 生える方向の決定
		int dir = (int)MyFunction.GetDirection(direction);

		// ターゲットからの距離
		Vector3 targetDistance = Vector3.zero;

		// 横
		bool horizon = dir % 2 == 1;

		// 岩の生成数
		int rockCount = (int)range + ((int)(attackRank * rankValue));

		// 攻撃位置
		Vector3 pos = target.position;
		// 加算量
		Vector3 addPos;

		// プレイヤーの横方向
        if (horizon)
        {
			// 岩の生え始める位置
			pos.y = target.position.y - (rockCount / 2);
			// 岩が並ぶ加算方向
			addPos = Vector3.up;
			// dir から左右を計算
			targetDistance.x += (dir - 2.0f) * distance;
        }
		// プレイヤーの縦方向
		else
		{
			// 岩の生え始める位置
			pos.x = target.position.x - (rockCount / 2);
			// 岩が並ぶ加算方向
			addPos = Vector3.right;
			// dir から上下を計算
			targetDistance.y += (dir - 1.0f) * distance;
		}

        // 並んだ岩を生成する
        for (int i = 0; i < rockCount; i++)
		{
			// 攻撃生成
			AttackOne(pos + targetDistance);
			// 岩の生える位置加算
			pos += addPos;
		}
	}
}
