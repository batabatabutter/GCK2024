using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy02 : DA_Enemy
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		base.Attack(target, direction, range, rankValue, attackRank);
	}
}
