using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_RollRock02 : DA_RollRock
{


	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		base.Attack(target, direction, range, distance, rankValue, attackRank);
	}

}
