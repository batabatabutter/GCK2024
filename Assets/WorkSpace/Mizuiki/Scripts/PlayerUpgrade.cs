using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
	[Header("プレイヤーの採掘スクリプト")]
	[SerializeField] private PlayerMining m_playerMining = null;

    [Header("プレイヤーのツールスクリプト")]
    [SerializeField] private PlayerTool m_playerTool = null;

    [Header("強化段階の区切り")]
    [SerializeField] private int m_stageDelimiter = 10;

    [Header("強化段階")]
    [SerializeField] private UpgradeData[] m_upgradeStage;

    [System.Serializable]
    public struct UpgradeStageValue
    {
        public int stage;                       // 増加し始める強化段階
        public MiningData.MiningValue value;  // 強化量
    }
    [Header("強化段階ごとの増加量"), Tooltip("[stage] の段階から1段階上がるごとに[value]の数値が加算される")]
    [SerializeField] private UpgradeStageValue[] m_upgradeStageValue;

    [Header("強化ランク")]
    [SerializeField] private int m_upgradeRank = 0;



	private void Start()
	{
        // 採掘スクリプトがない
		if (!m_playerMining)
        {
            // 採掘スクリプトを取得
            m_playerMining = GetComponent<PlayerMining>();
        }

        // ツールスクリプトがない
        if (!m_playerTool)
        {
            // プレイヤースクリプトの取得
            m_playerTool = GetComponent<PlayerTool>();
        }
	}

    // 採掘アップグレードを使用
	public void Upgrade(int value = 1)
	{
        // 必要素材の取得
        Items[] items = GetNeedMaterials(value);

        // 素材が足りない
        if (!m_playerTool.CheckCreate(items))
        {
            Debug.Log("素材不足");
            return;
        }

        // 素材の消費
        m_playerTool.ConsumeMaterials(items, value);

        Debug.Log("アップグレード : " + (m_upgradeRank + value).ToString());

        // アップグレード
        m_upgradeRank += value;

        // 強化値
        UpgradeData upgradeValue = GetValue();
        MiningData.MiningValue upgradeStageValue = GetStageValue(m_upgradeRank - value, m_upgradeRank);

	}

    // 必要素材の取得
    private Items[] GetNeedMaterials(int rank)
    {
        // 必要素材
        Items[] items = new Items[0];

        for (int i = 0; i < rank; i++)
        {
            // 必要素材取得
            Items[] cost = GetValue(m_upgradeRank + i).Cost;

			Items[] dst = new Items[items.Length + cost.Length];

			// 必要素材の追加
			Array.Copy(items, dst, items.Length);
			Array.Copy(cost, 0, dst, items.Length, cost.Length);
            items = dst;
        }

        return items;
    }

    // 強化値の取得(ランクごと)
    private UpgradeData GetValue()
    {
        // 強化値を返す
        return GetValue(m_upgradeRank);
    }
    private UpgradeData GetValue(int rank)
    {
		// 強化段階の取得
		int stage = GetStage(rank);
		// 強化段階の範囲にクランプ
		stage = Mathf.Clamp(stage, 0, m_upgradeStage.Length);
		// 強化値を返す
		return m_upgradeStage[stage];
	}

	// 強化値の取得(段階ごと)
	private MiningData.MiningValue GetStageValue(int beforeRank, int afterRank)
    {
        // 強化前の段階
        int beforeStage = GetStage(beforeRank);
        // 強化後の段階
        int afterStage = GetStage(afterRank);

        // 強化段階が上がっていない
        if (beforeStage >= afterStage)
            return MiningData.MiningValue.Zero();

        // 強化量
        MiningData.MiningValue val = MiningData.MiningValue.Zero();
        foreach (UpgradeStageValue stageValue in m_upgradeStageValue)
        {
            // 増加開始段階
            int stage = stageValue.stage;

            // 増加し始める段階ではない
            if (stage < afterStage)
                continue;

            // 強化値加算
            val += stageValue.value;
        }
        // 増加段階分掛ける
        return val * (afterStage - beforeRank);
    }

    // 強化段階取得
    private int GetStage(int rank)
    {
        return rank / m_stageDelimiter;
    }

}
