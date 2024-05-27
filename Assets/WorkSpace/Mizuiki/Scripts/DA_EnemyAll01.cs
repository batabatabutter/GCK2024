using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_EnemyAll01 : DA_Enemy
{
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// ƒ‰ƒ“ƒ_ƒ€‚È“G‚Ìí—Ş‚ğæ“¾
		Enemy.Type type = (Enemy.Type)Random.Range(0, (int)Enemy.Type.OverID);
		// ¶¬‚·‚é“G‚Ìí—Ş‚ğİ’è
		Type = type;

		// Œˆ‚ß‚ç‚ê‚½”“G‚ğ¶¬
		for (int i = 0; i < (int)range; i++)
		{
			AttackOne(target.position, attackRank);
		}
	}
}
