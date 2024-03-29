using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
	[System.Serializable]
	public struct ToolContainer
	{
		public ToolData.ToolType type;
		public GameObject tool;
	}


	[Header("ツールの設置範囲(半径)")]
	[SerializeField] private float m_toolSettingRange = 2.0f;

    [Header("カーソル画像")]
    [SerializeField] private GameObject m_cursorImage = null;

	[Header("レイヤーマスク")]
	[SerializeField] private LayerMask m_layerMask;

	[Header("設置ツール")]
	[SerializeField] private ToolContainer[] m_putTools;

	[Header("ツールのデータベース")]
	[SerializeField] private ToolDataBase m_data;

	[Header("アイテム")]
	[SerializeField] private PlayerItem m_playerItem;

	// ツール設置可能
	private bool m_canPut = true;

	// 設置ツール
	private int m_toolType = 0;


	// Start is called before the first frame update
	void Start()
    {
		// アイテムがなければ取得
        if (m_playerItem == null)
		{
			if (TryGetComponent(out PlayerItem item))
			{
				m_playerItem = item;
			}
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
		// プレイヤーから採掘方向へのRayCast
		RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, length, m_layerMask);

		// ブロックに当たった
		if (rayCast)
		{
			// Toolタグが付いている
			if (rayCast.transform.CompareTag("Tool"))
			{
				// 設置できなくする
				m_canPut = false;
			}
			else
			{
				// 埋まり防止で当たった面の法線方向に 0.1 加算する
				mousePos = rayCast.point + (rayCast.normal * new Vector2(0.1f, 0.1f));
			}
		}

		// プレイヤーとマウスカーソルの位置が設置範囲内
		if (Vector2.Distance(playerPos, mousePos) < m_toolSettingRange)
		{
			// 四捨五入する
			mousePos = RoundHalfUp(mousePos);

			// ツールの設置位置をマウスカーソルの位置にする
			m_cursorImage.transform.position = mousePos;
		}
		else
		{
			// 届く最大範囲に設定
			mousePos = playerPos + (playerToMouse * m_toolSettingRange);

			// 四捨五入する
			mousePos = RoundHalfUp(mousePos);

			// アイテムの設置位置
			m_cursorImage.transform.position = mousePos;
		}

	}

	// ツール設置
	public void Put()
    {
		// アイテムが設置できない
		if (!m_canPut)
		{
			return;
		}

		// 選択されているアイテムが作成できない
		if (!CheckCreate(m_toolType))
		{
			Debug.Log("素材不足");
			return;
		}

		// アイテムを置く
		GameObject tool = Instantiate(m_putTools[m_toolType].tool);
		// 座標設定
		tool.transform.position = m_cursorImage.transform.position;
    }

	// ツール変更
	public void ChangeTool(int val)
	{
		// 変更後の値
		int change = m_toolType + val;

		// 変更後が 0 未満
		if (change < 0)
		{
			// 一番後ろのツールにする
			change = (int)ToolData.ToolType.OVER - 1;
		}
		// 変更後が範囲外
		else if (change >= (int)ToolData.ToolType.OVER)
		{
			change = 0;
		}

		// ツールを変更する
		m_toolType = change;
		Debug.Log(m_putTools[m_toolType].type);
	}



	// ツールを作成できるかチェック
	private bool CheckCreate(int type)
	{
		// ツールの種類分ループ
		for (int i = 0; i < m_data.tool.Count; i++)
		{
			// 作成ツールのコスト
			if (m_data.tool[i].toolType == m_putTools[type].type)
			{
				return CheckCreate(m_data.tool[i]);
			}
		}
		// 選択ツールが存在しない
		return false;
	}
	private bool CheckCreate(ToolData data)
	{
		// 素材の種類分ループ
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			Item.Type type = data.itemMaterials[i].type;
			int count = data.itemMaterials[i].count;

			// 所持アイテム数が必要素材数未満
			if (m_playerItem.Items[type] < count)
			{
				// 作成できない
				return false;
			}

		}
		// 必要素材数所持している
		return true;
	}

	// 四捨五入
	private Vector2 RoundHalfUp(Vector2 value)
	{
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);

		return value;
	}
	static float RoundHalfUp(float value)
	{
		// 小数点以下の取得
		float fraction = value - MathF.Floor(value);

		// 小数点以下が0.5未満
		if (fraction < 0.5f)
		{
			// 切り捨てる
			return MathF.Floor(value);
		}
		// 切り上げる
		return MathF.Floor(value) + 1.0f;

	}

}
