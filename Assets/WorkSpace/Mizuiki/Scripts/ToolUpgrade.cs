using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUpgrade : Tool
{
    [Header("強化値")]
    [SerializeField] private int m_upgradeValue = 20;
    [Header("強化量")]
    [SerializeField] private float m_upgradeAmount = 2.0f;

    // プレイヤーのツール
	private PlayerTool m_playerTool = null;

	// 採掘
	private PlayerMining m_mining = null;
    // ツール使用時の採掘回数
    private int m_useMiningCount = 0;


    // Update is called once per frame
    void Update()
    {
        // 採掘スクリプトがなければ処理をしない
        if (!m_mining)
            return;

        // ツール使用後からの採掘回数
        int count = m_mining.BrokenCount - m_useMiningCount;

        // 強化値分採掘した
        if (count >= m_upgradeValue)
        {
            // 強化のリセット
            m_mining.MiningSpeedRate = 1.0f;

            // リキャスト開始
            m_playerTool.SetRecast(true, ToolData.ToolType.UPGRADE);

            m_mining = null;

        }
        
    }

    // 採掘アップグレードを使用
	public override void UseTool(GameObject obj)
	{
        Debug.Log("アップグレード");

        // プレイヤーの取得
        if (obj.TryGetComponent(out PlayerMining mining))
        {
            // 採掘スクリプトを取得する
            m_mining = mining;

            // 現在の採掘回数を取得
            m_useMiningCount = mining.BrokenCount;

            // 採掘速度の倍率を設定
            m_mining.MiningSpeedRate = m_upgradeAmount;

        }

        // ツールの取得
        m_playerTool = obj.GetComponent<PlayerTool>();
	}

}
