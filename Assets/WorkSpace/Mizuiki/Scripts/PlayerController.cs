using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの移動スクリプト")]
    [SerializeField] private PlayerMove m_playerMove = null;

    [Header("プレイヤーの採掘スクリプト")]
    [SerializeField] private PlayerMining m_playerMining = null;

	[Header("プレイヤーの設置スクリプト")]
	[SerializeField] private PlayerAction m_playerAction = null;

	[Header("プレイヤーの強化スクリプト")]
	[SerializeField] private PlayerUpgrade m_playerUpgrade = null;

	// 入力
	private Controls m_controls = null;


	// Start is called before the first frame update
	void Start()
    {
		// コントローラの生成
		m_controls = new Controls();
		// コントローラの有効化
		m_controls.Enable();

	}

	// Update is called once per frame
	void Update()
    {
		// 移動
		// 入力方向の取得
		Vector2 velocity = m_controls.Player.Move.ReadValue<Vector2>();
		// 移動の呼び出し
		m_playerMove.MovePlayer(velocity);

		// 採掘
		if (m_controls.Player.Attack.IsPressed())		// 押されてる間
		{
			m_playerMining.Mining();
		}

		// 設置
		if (m_controls.Player.Toach.WasPressedThisFrame())	// 押した瞬間
		{
			m_playerAction.PutToach();
		}

		// 強化
		if (m_controls.Player.Upgrade.WasPerformedThisFrame())
		{
			m_playerUpgrade.Upgrade();
		}

		// ツール使用
		if (m_controls.Player.Tool.WasPressedThisFrame())
		{
			m_playerAction.UseTool();
		}

		// ツール変更
		int scroll = (int)m_controls.Player.ChangeTool.ReadValue<float>() / 120;
		if (scroll != 0)
		{
			m_playerAction.ChangeTool(scroll);
		}

	}
}
