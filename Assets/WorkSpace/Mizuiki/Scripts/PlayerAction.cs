using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
	[Header("ツールの設置範囲(半径)")]
	[SerializeField] private float m_toolSettingRange = 2.0f;

    [Header("カーソル画像")]
    [SerializeField] private GameObject m_cursorImage = null;

	[Header("レイヤーマスク")]
	[SerializeField] private LayerMask m_layerMask;

	[Header("ツールスクリプト")]
	[SerializeField] private PlayerTool m_playerTool = null;
	[Header("アップグレードスクリプト")]
	[SerializeField] private PlayerUpgrade m_playerUpgrade = null;

	// ツール設置可能
	private bool m_canPut = true;


	// Start is called before the first frame update
	void Start()
    {
		// ツールがなければ取得
		if (m_playerTool == null)
		{
			m_playerTool = GetComponent<PlayerTool>();
		}

		// アップグレードがなければ取得
		if (m_playerUpgrade == null)
		{
			m_playerUpgrade = GetComponent<PlayerUpgrade>();
		}

    }

    // Update is called once per frame
    void Update()
    {
		// 設置可能な状態にしておく
		m_canPut = true;

		// プレイヤーの位置
		Vector2 playerPos = transform.position;

		// マウスの位置を取得
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// プレイヤーの位置からマウスの位置へのベクトル
		Vector2 playerToMouse = mousePos - playerPos;
		// 距離を取得しておく
		float length = playerToMouse.magnitude;
		// ベクトル正規化
		playerToMouse.Normalize();

		// プレイヤーとマウスカーソルの位置が設置範囲内
		if (Vector2.Distance(playerPos, mousePos) < m_toolSettingRange)
		{
			//// 四捨五入する
			//mousePos = RoundHalfUp(mousePos);

			//// ツールの設置位置をマウスカーソルの位置にする
			//m_cursorImage.transform.position = mousePos;
		}
		else
		{
			// 届く最大範囲に設定
			mousePos = playerPos + (playerToMouse * m_toolSettingRange);

			length = m_toolSettingRange;

			//// 四捨五入する
			//mousePos = RoundHalfUp(mousePos);

			//// アイテムの設置位置
			//m_cursorImage.transform.position = mousePos;
		}

		// プレイヤーから採掘方向へのRayCast
		RaycastHit2D[] rayCast = Physics2D.RaycastAll(playerPos, playerToMouse, length, m_layerMask);

		// ブロックからの押し返し
		foreach (RaycastHit2D cast in rayCast)
		{
			// ブロックタグが付いている
			if (cast.transform.CompareTag("Block"))
			{
				// 埋まり防止で当たった面の法線方向に 0.1 加算する
				mousePos = cast.point + (cast.normal * new Vector2(0.1f, 0.1f));

				break;
			}
		}
		// ツールの重ね置き回避
		foreach (RaycastHit2D cast in rayCast)
		{
			// Toolタグが付いていて、同じグリッド
			if (cast.transform.CompareTag("Tool") &&
				MyFunction.CheckSameGrid(mousePos, cast.transform.position))
			{
				// 設置できなくする
				m_canPut = false;
			}
		}

		// 四捨五入する
		mousePos = MyFunction.RoundHalfUp(mousePos);

		// アイテムの設置位置
		m_cursorImage.transform.position = mousePos;

	}

	// 強化
	public void Upgrade()
	{
		m_playerUpgrade.Upgrade();
	}

	// ツールの使用
	public void UseTool()
	{
		// アイテムが設置できない
		if (!m_canPut)
		{
            Debug.Log("すでにツールがある");
			return;
		}

		// ツールの使用
		m_playerTool.UseTool(m_cursorImage.transform.position);

	}

	// ツール変更
	public void ChangeTool(int val)
	{
        m_playerTool.ChangeTool(val);
		AudioManager.Instance.PlaySE(AudioDataID.Select);
    }

    // ツールの切り替え
    public void SwitchTool()
	{
        m_playerTool.SwitchTool();
        AudioManager.Instance.PlaySE(AudioDataID.Change);
    }



    // 選択ツールの取得
    public ToolData.ToolType ToolType
	{
		get { return m_playerTool.ToolType; }
	}
    public float GetToolRecast(ToolData.ToolType type)
    {
        return m_playerTool.RecastTime(type);
    }
}
