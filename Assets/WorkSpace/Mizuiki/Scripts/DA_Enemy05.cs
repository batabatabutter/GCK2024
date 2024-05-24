using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy05 : DA_Enemy
{
	private void Start()
	{
		// タイプ指定アリ
		Type = Enemy.Type.Bomurun;
		IsType = true;

		// 範囲指定アリ
		IsRadius = true;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		for (int i = 0; i < (int)range; i++)
		{
			AttackOne(target.position);
		}
	}
}
