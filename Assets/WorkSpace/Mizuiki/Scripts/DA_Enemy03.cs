using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy03 : DA_Enemy
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// çUåÇà íu
		Vector3 attackPos = target.position;

		for (int i = 0; i < (int)range; i++)
		{

		}

	}
}
