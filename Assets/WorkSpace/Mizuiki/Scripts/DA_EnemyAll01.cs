using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_EnemyAll01 : DA_Enemy
{
	private void Start()
	{
		// 半径を指定
		IsRadius = true;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// 生成範囲の設定
		Radius = distance;

		// ランダムな敵の種類を取得
		Enemy.Type type = (Enemy.Type)Random.Range(0, (int)Enemy.Type.OverID);
		// 生成する敵の種類を設定
		Type = type;

		// 決められた数敵を生成
		for (int i = 0; i < (int)range; i++)
		{
			AttackOne(target.position, attackRank);
		}
	}
}
