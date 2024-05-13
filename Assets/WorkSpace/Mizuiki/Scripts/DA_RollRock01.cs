using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_RollRock01 : DungeonAttackRollRock
{
	[Header("•À‚ÔŠâ")]

	[Header("‰¡‚É•À‚Ô”")]
	[SerializeField] private int m_lineUpCount = 5;

	[Header("ƒ‰ƒ“ƒN‘‰Á—Ê")]
	[SerializeField] private float m_rankValue = 0.5f;


	// UŒ‚ŠJn
	public override void Attack(Transform target, int attackRank = 1)
	{
		// “]‚ª‚é•ûŒü‚ÌŒˆ’è
		int rand = Random.Range(0, (int)MyFunction.Direction.RANDOM);

		// ‰¡
		bool horizon = rand % 2 == 1;

		// Šâ‚Ì¶¬”
		int rockCount = m_lineUpCount + ((int)(attackRank * m_rankValue));

		// UŒ‚ˆÊ’u
		Vector3 pos = target.position;
		// ‰ÁZ—Ê
		Vector3 addPos;

		// ‰¡
        if (horizon)
        {
			pos.y = target.position.y - rockCount / 2;
			addPos = Vector3.up;
        }
		// c
		else
		{
			pos.x = target.position.x - rockCount / 2;
			addPos = Vector3.right;
		}

        // •À‚ñ‚¾Šâ‚ğ¶¬‚·‚é
        for (int i = 0; i < rockCount; i++)
		{
			// UŒ‚¶¬
			AttackOne(pos, rand);

			pos += addPos;
		}
	}
}
