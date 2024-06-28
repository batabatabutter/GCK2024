using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_EnemyBom01 : DA_Enemy
{
	private void Start()
	{
		// �^�C�v�w��A��
		Type = Enemy.Type.Bomurun;
		IsType = true;

		// �͈͎w��A��
		IsRadius = true;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		for (int i = 0; i < (int)range; i++)
		{
			AttackOne(target.position);
		}
	}
}
