using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTotem : Block
{
	[Header("受けるダメージ")]
	[SerializeField] private float m_ownDamage = 100;

	[Header("ダメージ間隔")]
	[SerializeField] private float m_damageInterval = 1.0f;
	// ダメージ用のタイマー
	private float m_damageTimer = 0.0f;

	private void Update()
	{
		// 自身にダメージ(Intervalごと)
		if (m_damageTimer <= 0.0f)
		{
			// タイマーにインターバルを加算
			m_damageTimer += m_damageInterval;
			// ダメージ
			AddMiningDamage(m_ownDamage);
		}

	}
}
