using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class DA_FallRock02 : DungeonAttackFallRock
{
	[Header("ランダム落石")]

	[Header("落石の範囲")]
	[SerializeField, Min(0.0f)] private float m_fallRockRange = 5.0f;


	public override void Attack(Transform target, int attackRank = 1)
	{
		// 落石の発生位置をランダムに決める
		Vector3 random = new(Random.Range(-m_fallRockRange, m_fallRockRange), Random.Range(-m_fallRockRange, m_fallRockRange), 0.0f);
		// 四捨五入してブロックと会う位置に調整
		random = MyFunction.RoundHalfUp(random);
		// 落石発生
		AttackOne(target.position + random, attackRank);
	}

	// 攻撃範囲の設定
	public override void SetAttackRange(float range)
	{
		// 落石範囲の設定
		m_fallRockRange = range;
	}

	// ランク増加量の設定
	public override void SetRankValue(float value)
	{
		
	}
}
