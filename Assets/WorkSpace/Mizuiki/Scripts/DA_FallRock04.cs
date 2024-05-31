using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DA_FallRock04 : DA_FallRock
{
	[Header("---------- 中心からの落石 ----------")]
	[Header("落石クラス")]
	[SerializeField] private DA_FallRock03 m_fallRock03;

	[Header("攻撃フラグ")]
	[SerializeField] private bool m_attack = false;

	[Header("落石の間隔(秒)")]
	[SerializeField] private float m_fallTime = 0.5f;
	private float m_fallTimer = 0.0f;

	[Header("並ぶ数")]
	[SerializeField] private int m_range = 5;
	[Header("次発生する攻撃のサイズ")]
	[SerializeField] private int m_attackSize = 1;

	[Header("ランク")]
	[SerializeField] private int m_rank = 0;
	[Header("ランクに応じた増加量")]
	[SerializeField] private float m_rankValue = 1.0f;

	[Header("ターゲットの位置")]
	[SerializeField] private Vector3 m_targetPosition = Vector3.zero;


	private void Update()
	{
		// 攻撃しない
		if (m_attack == false)
		{
			return;
		}

		// 攻撃範囲を超えている
		if (m_attackSize > m_range)
		{
			m_attack = false;
			return;
		}

		// 時間経過
		m_fallTimer -= Time.deltaTime;

		// 攻撃発生
		if (m_fallTimer <= 0.0f)
		{
			// 攻撃
			m_fallRock03.AttackLump(m_targetPosition, m_attackSize, m_rankValue);
			// 攻撃サイズ加算
			m_attackSize += 2;
			// 攻撃時間
			m_fallTimer = m_fallTime;
		}

	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// ターゲットの位置取得
		m_targetPosition = target.position;
		// 並ぶ数取得
		m_range = (int)range;
		// 攻撃サイズ
		m_attackSize = 1;
		// ランク補正値
		m_rankValue = rankValue;
		// ランク
		m_rank = attackRank;

		// 一発目の攻撃
		m_fallRock03.Attack(target, direction, m_attackSize, distance, rankValue, attackRank);

		// 攻撃フラグオン
		m_attack = true;
		// タイマー設定
		m_fallTimer = m_fallTime;
		// 攻撃サイズ加算
		m_attackSize += 2;
	}


}
