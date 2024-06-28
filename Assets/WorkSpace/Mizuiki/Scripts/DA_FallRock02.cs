using UnityEngine;

public class DA_FallRock02 : DA_FallRock
{
	//[Header("ランダム落石")]

	//[Header("落石の範囲")]
	//[SerializeField, Min(0.0f)] private float m_fallRockRange = 5.0f;


	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// 落石の発生位置をランダムに決める
		Vector3 random = new(Random.Range(-/*m_fallRockRange*/range, /*m_fallRockRange*/range), Random.Range(-/*m_fallRockRange*/range, /*m_fallRockRange*/range), 0.0f);
		// 四捨五入してブロックと会う位置に調整
		random = MyFunction.RoundHalfUp(random);
		// 落石発生
		AttackOne(target.position + random, attackRank);
	}

	//// 攻撃範囲の設定
	//public override void SetAttackRange(float range)
	//{
	//	// 落石範囲の設定
	//	m_fallRockRange = range;
	//}

	//// ランク増加量の設定
	//public override void SetRankValue(float value)
	//{
		
	//}
}
