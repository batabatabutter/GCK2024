using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	[Header("プレイヤー")]
	[SerializeField] private PlayerController m_player = null;

	[Header("アタッカー")]
	[SerializeField] private DungeonAttacker m_attacker = null;

	[Header("敵")]
	[SerializeField] private EnemyManager m_enemy = null;

	[Header("ツール")]
	[SerializeField] private ToolManager m_tool = null;


	public void SetPause(bool pause)
	{
		// プレイヤーの更新を止める
		if (m_player)
		{
			m_player.enabled = pause;
		}

		// アタッカーの更新を止める
		if (m_attacker)
		{
			m_attacker.enabled = pause;
		}

		// 敵の更新を止める
		if (m_enemy)
		{
			m_enemy.SetEnabled(pause);
		}

		// ツールの更新を止める
		if (m_tool)
		{
			m_tool.SetEnabled(pause);
		}

	}


}
