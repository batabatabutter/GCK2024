using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
	[Header("プレイヤーの採掘スクリプト")]
	[SerializeField] private PlayerMining m_playerMining = null;

    [Header("プレイヤーのツールスクリプト")]
    [SerializeField] private PlayerTool m_playerTool = null;

    [Header("採掘強化")]
    [SerializeField] private ToolData m_upgradeData = null;

    [Header("強化値")]
    [SerializeField] private PlayerMining.MiningValue m_upgradeValue;

    [Header("強化段階")]
    [SerializeField] private PlayerMining.MiningValue[] m_upgradeStage;

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

	// Update is called once per frame
	void Update()
    {
    }

    // 採掘アップグレードを使用
	public void Upgrade(int value = 1)
	{
        // 作成できない
        if (!m_playerTool.CheckCreate(m_upgradeData))
        {
            Debug.Log("素材不足");
            return;
        }

        Debug.Log("アップグレード : " + (m_upgradeRank + value).ToString());

        // アップグレード
        m_upgradeRank += value;

        // 強化値
        PlayerMining.MiningValue upgradeValue = GetValue();

        // 強化
        m_playerMining.MiningValueBase += upgradeValue * value;

        // 素材の消費
        m_playerTool.ConsumeMaterials(m_upgradeData, value);

	}

    // 強化値の取得
    private PlayerMining.MiningValue GetValue()
    {
        // 強化段階の取得
        int rank = m_upgradeRank / 100;
        // 強化段階の範囲にクランプ
        rank = Mathf.Clamp(rank, 0, m_upgradeStage.Length);
        // 強化値を返す
        return m_upgradeStage[rank];
    }

}
