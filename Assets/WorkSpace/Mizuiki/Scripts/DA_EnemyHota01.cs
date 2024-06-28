using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_EnemyHota01 : DA_Enemy
{
	private void Start()
	{
		// タイプ指定アリ
		Type = Enemy.Type.Hotarun;
		IsType = true;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// とりあえず4方向に出す
		AttackOne(target.position + (Vector3.up * distance), attackRank);
		AttackOne(target.position + (Vector3.down * distance), attackRank);
		AttackOne(target.position + (Vector3.left * distance), attackRank);
		AttackOne(target.position + (Vector3.right * distance), attackRank);
	}
}
