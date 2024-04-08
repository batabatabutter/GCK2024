using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMining : Tool
{
	[Header("ツールの種類")]
	[SerializeField] private ToolData.ToolType m_toolType = ToolData.ToolType.DRILL;

	[Header("強化する値(n倍)")]
	[SerializeField] private PlayerMining.MiningValue m_boost = new();

	[Header("採掘力の n 倍で効果が切れる")]
	[SerializeField] private float m_boostAmountValue = 2.0f;
	// 効果が切れる値
	private float m_amountValue = 0.0f;

	// プレイヤーのツールスクリプト
	private PlayerTool m_playerTool = null;

	// プレイヤーの採掘スクリプト
	private PlayerMining m_playerMining = null;


	// Update is called once per frame
	void Update()
	{
		// 採掘が存在しない
		if (m_playerMining == null)
			return;

		// 強化終了
		if (m_playerMining.TakenDamage > m_amountValue)
		{
			// リキャスト開始
			m_playerTool.SetRecast(true, m_toolType);
			// 強化剥奪
			m_playerMining.MiningValueBoost -= m_boost;
		}

	}

	// ツールを使用する
	public override void UseTool(GameObject obj)
	{
		// プレイヤーの取得
		if (obj.TryGetComponent(out PlayerMining mining))
		{
			// 採掘スクリプトの設定
			m_playerMining = mining;

			// 強化
			mining.MiningValueBoost += m_boost;

			// 強化終了値
			m_amountValue = mining.TakenDamage + (mining.MiningPower * m_boostAmountValue);

		}

		// ツールの取得
		m_playerTool = obj.GetComponent<PlayerTool>();
	}

}