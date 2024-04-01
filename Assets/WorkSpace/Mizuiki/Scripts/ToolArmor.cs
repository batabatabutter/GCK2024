using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolArmor : Tool
{
    [Header("アーマーの数")]
    [SerializeField] private int m_armorCount = 1;

    // プレイヤー
    private Player m_player = null;
    // プレイヤーのツール
    private PlayerTool m_playerTool = null;


	private void Update()
	{
        // プレイヤーがなければ処理しない
        if (!m_player)
            return;
		
        // アーマーがなくなればリキャスト開始
        if (m_player.Armor <= 0)
        {
            // アーマーのリキャスト開始
            m_playerTool.SetRecast(true, ToolData.ToolType.ARMOR);

            m_player = null;
        }

	}

	public override void UseTool(GameObject obj)
	{
        Debug.Log("アーマー");

        // プレイヤーの取得
        if (obj.TryGetComponent(out Player player))
        {
            // プレイヤーの取得
            m_player = player;

            // アーマーの設定
            player.Armor = m_armorCount;
        }

        m_playerTool = obj.GetComponent<PlayerTool>();

	}

}
