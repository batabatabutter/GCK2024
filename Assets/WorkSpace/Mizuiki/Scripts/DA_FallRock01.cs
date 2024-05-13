using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_FallRock01 : DungeonAttackFallRock
{
	[Header("塊の落石")]

	[Header("塊のサイズ")]
	[SerializeField] private int m_massSize = 3;

	[Header("ランクの増加量")]
	[SerializeField] private float m_rankValue = 1.0f;


	public override void Attack(Transform target, int attackRank = 1)
	{
		// 塊のサイズ
		int massSize = m_massSize + (int)(attackRank * m_rankValue);
		int massRange = massSize / 2;
		// ターゲットのグリッド取得
		Vector2Int targetGrid = MyFunction.RoundHalfUpInt(target.position);
		// パターンその2
		for (int y = targetGrid.y - massRange; y <= targetGrid.y + massRange; y++)
		{
			for (int x = targetGrid.x - massRange; x <= targetGrid.x + massRange; x++)
			{
				// 攻撃発生位置
				Vector3 attackPos = new(x, y, 0);
				// 攻撃発生
				AttackOne(attackPos);
			}
		}
	}

	// 攻撃範囲の設定
	public override void SetAttackRange(float range)
	{
		// 塊サイズの設定
		m_massSize = (int)range;
	}

	// ランク増加量の設定
	public override void SetRankValue(float value)
	{
		m_rankValue = value;
	}

}
