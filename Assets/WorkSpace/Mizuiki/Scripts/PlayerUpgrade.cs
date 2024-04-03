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

    [Header("採掘速度")]
    [SerializeField] private float m_upgradeSpeed = 0.1f;
    [Header("採掘力")]
    [SerializeField] private float m_upgradePower = 10.0f;
    [Header("クリティカル率")]
    [SerializeField] private float m_upgradeCritical = 1.0f;

    [Header("強化ランク")]
    [SerializeField] private int m_upgradeRank = 1;



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

        // 採掘速度強化
        m_playerMining.MiningSpeed += m_upgradeSpeed * value;
        // 採掘力強化
        m_playerMining.MiningPower += m_upgradePower * value;
        // クリティカル率教科
        m_playerMining.CriticalRate += m_upgradeCritical * value;

        // 素材の消費
        m_playerTool.ConsumeMaterials(m_upgradeData, value);

	}

}
