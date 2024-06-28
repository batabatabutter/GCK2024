using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DA_GrowUpRock02 : DA_GrowUpRock
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// 生える方向の決定
		MyFunction.Direction dir = MyFunction.GetDirection(direction);

		AttackLump(target.position, dir, range, distance, rankValue, attackRank);
	}

	public void AttackLump(Vector3 pos, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// ターゲットからの距離
		Vector3 targetDistance = Vector3.zero;

		// 横
		bool horizon = (int)direction % 2 == 1;

		// 岩の生成数
		int rockCount = (int)range + ((int)(attackRank * rankValue));

		// 加算量
		Vector3 addPos;

		// プレイヤーの横方向
		if (horizon)
		{
			// 岩の生え始める位置
			pos.y -= (rockCount / 2);
			// 岩が並ぶ加算方向
			addPos = Vector3.up;
			// dir から左右を計算
			targetDistance.x += ((int)direction - 2.0f) * distance;
		}
		// プレイヤーの縦方向
		else
		{
			// 岩の生え始める位置
			pos.x -= (rockCount / 2);
			// 岩が並ぶ加算方向
			addPos = Vector3.right;
			// dir から上下を計算
			targetDistance.y += ((int)direction - 1.0f) * distance;
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
