using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BlockData;

public class ToolMining : Tool
{
	[Header("ツールの種類"), CustomEnum(typeof(ToolData.ToolType))]
	[SerializeField] private string toolTypeStr = "";
	private ToolData.ToolType m_toolType => SerializeUtil.Restore<ToolData.ToolType>(toolTypeStr);

	[Header("強化する値(n倍)")]
	[SerializeField] private MiningData.MiningValue m_boost = new();

	[Header("採掘力の n 倍で効果が切れる")]
	[SerializeField] private float m_boostAmountValue = 2.0f;
	// 効果が切れる値
	private float m_amountValue = 0.0f;
	//	効果がついた時の合計採掘ダメージ
	private float m_startTakenDmg = 0.0f;

	// プレイヤーのツールスクリプト
	private PlayerTool m_playerTool = null;

	// プレイヤーの採掘スクリプト
	private PlayerMining m_playerMining = null;

	//	ツール強化中
	private bool m_isEnhance = false;


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
			// 採掘を削除する
			m_playerMining = null;
			//	フラグ設定
			m_isEnhance = false;

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

			//	初期値
			m_startTakenDmg = mining.TakenDamage;

            //	フラグ設定
            m_isEnhance = true;
		}

		// ツールの取得
		m_playerTool = obj.GetComponent<PlayerTool>();
	}

	//	終了値取得
	public float AmountValue => m_amountValue;

	//	初期値取得
	public float StartTakenDMG => m_startTakenDmg;

	//	フラグ取得
	public bool IsEnhance => m_isEnhance;

	//	タイプ取得
	public ToolData.ToolType ToolType => m_toolType;

	//	プレイヤー情報取得
	public PlayerMining PlayerMining => m_playerMining;
}