using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackTurn
{
	// 攻撃自体
	Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = null;

	// ここで処理する攻撃パターン
	DungeonAttackPattern m_attackPattern;

	// 発生させる攻撃のインデックス
	int m_attackIndex = 0;

	// タイマー
	float m_timer = 0.0f;


	public void Attack(Transform target, int attackRank, float attackGrade)
	{
		// インデックスの範囲確認
		if (m_attackIndex >= m_attackPattern.AttackList.Count)
		{
			m_attackIndex = 0;
		}
		// 時間経過
		m_timer -= Time.deltaTime;
		// 次の攻撃発生
		if (m_timer < 0.0f)
		{
			// 攻撃パターン取得
			DungeonAttackPattern.AttackPattern attack = m_attackPattern.AttackList[m_attackIndex];
			// 攻撃発生
			m_attacker[attack.type].Attack(target, attack.direction, attack.range, attack.rankValue, attackRank);
			// クールタイム設定
			m_timer = attack.time * attackGrade;
			// インデックスのインクリメント
			m_attackIndex++;
		}
	}


	// アタッカー
	public Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> Attacker
	{
		set { m_attacker = value; }
	}
	// 攻撃パターン
	public DungeonAttackPattern AttackPattern
	{
		set { m_attackPattern = value; }
	}


}
