using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
	[System.Serializable]
	public struct ToolContainer
	{
		public ToolData.ToolType type;		// ツールの情報
		public GameObject tool;				// インスタンスするツールの実体
		public float recastTime;			// リキャスト時間
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


	[Header("デバッグ---------------------------")]
	[SerializeField] private bool m_debug = false;
	[SerializeField] private Text m_text = null;


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
		// ツールの更新
		for (int i = 0; i < m_putTools.Length; i++)
		{
			if (m_putTools[i].recastTime > 0.0f)
			{
				m_putTools[i].recastTime -= Time.deltaTime;
			}
		}

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
			// ブロックに当たった
			if (cast.transform)
			{
				// ブロックタグが付いている
				if (cast.transform.CompareTag("Block"))
				{
					// 埋まり防止で当たった面の法線方向に 0.1 加算する
					mousePos = cast.point + (cast.normal * new Vector2(0.1f, 0.1f));

					break;
				}
			}
		}
		// ツールの重ね置き回避
		foreach (RaycastHit2D cast in rayCast)
		{
			if (cast.transform)
			{
				// Toolタグが付いている
				if (cast.transform.CompareTag("Tool"))
				{
					// 同じグリッド
					if (CheckSameGrid(mousePos, cast.transform.position))
					{
						// 設置できなくする
						m_canPut = false;
					}
				}
			}
		}

		// 四捨五入する
		mousePos = RoundHalfUp(mousePos);

		if (m_text)
		{
			m_text.text += mousePos.ToString();
		}

		// アイテムの設置位置
		m_cursorImage.transform.position = mousePos;

		// デバッグ
		if (m_debug)
		{
			if (m_text != null)
			{
				m_text.text = m_putTools[m_toolType].type.ToString();
			}
		}

	}

	// ツール設置
	public void Put()
    {
		// アイテムが設置できない
		if (!m_canPut)
		{
			Debug.Log("すでにツールがある");
			return;
		}

		// 選択されているアイテムが作成できない
		if (!CheckCreate(m_toolType))
		{
			Debug.Log("素材不足");
			return;
		}

		// クールタイム中なら設置できない
		if (m_putTools[m_toolType].recastTime > 0.0f)
		{
			Debug.Log("クールタイム中");
			return;
		}

		// クールタイムの設定
		m_putTools[m_toolType].recastTime = GetToolData(m_toolType).recastTime;

		// アイテムを置く
		GameObject tool = Instantiate(m_putTools[m_toolType].tool);
		// 座標設定
		tool.transform.position = m_cursorImage.transform.position;
		// アクティブにする
		tool.SetActive(true);

		// 素材を消費する
		m_playerItem.ConsumeMaterials(GetToolData(m_toolType));

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
	}


	// ツールのデータを取得
	private ToolData GetToolData(int toolType)
	{
		// ツールの種類分ループ
		for (int i = 0; i < m_data.tool.Count; i++)
		{
			// データベースのツールと設定されているツールが同じ
			if (m_data.tool[i].toolType == m_putTools[toolType].type)
			{
				return m_data.tool[i];
			}
		}

		return null;
	}

	// ツールを作成できるかチェック
	private bool CheckCreate(int type)
	{
		ToolData data = GetToolData(type);

		if (data != null)
		{
			return CheckCreate(data);
		}

		// 選択ツールが存在しない
		return false;
	}
	private bool CheckCreate(ToolData data)
	{
		// 素材の種類分ループ
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			ItemData.Type type = data.itemMaterials[i].type;
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
	private Vector2Int RoundHalfUpInt(Vector2 value)
	{
		Vector2Int val = new()
		{
			x = (int)RoundHalfUp(value.x),
			y = (int)RoundHalfUp(value.y)
		};

		return val;
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

	// 同じグリッドにある
	private bool CheckSameGrid(Vector2 pos1, Vector2 pos2)
	{
		// 四捨五入した値を取得(int)
		Vector2Int p1 = RoundHalfUpInt(pos1);
		Vector2Int p2 = RoundHalfUpInt(pos2);

		// 同じグリッド
		if (p1 == p2)
		{
			return true;
		}

		// 違う
		return false;
	}



	public ToolData.ToolType ToolType
	{
		get { return m_putTools[m_toolType].type; }
	}
    public float GetToolRecast(ToolData.ToolType type)
    {
        return m_putTools[(int)type].recastTime;
    }
}
