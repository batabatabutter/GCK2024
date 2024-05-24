using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy04 : DA_Enemy
{
	private void Start()
	{
		// タイプ指定アリ
		Type = Enemy.Type.Hotarun;
		IsType = true;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// とりあえず4方向に出す
		AttackOne(target.position + (Vector3.up * range), attackRank);
		AttackOne(target.position + (Vector3.down * range), attackRank);
		AttackOne(target.position + (Vector3.left * range), attackRank);
		AttackOne(target.position + (Vector3.right * range), attackRank);
	}
}
