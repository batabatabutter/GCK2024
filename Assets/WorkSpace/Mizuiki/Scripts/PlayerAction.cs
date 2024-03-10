using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
	[Header("アイテムの設置範囲(半径)")]
	[SerializeField] private float m_itemSettingRange = 2.0f;

    [Header("カーソル画像")]
    [SerializeField] private GameObject m_cursorImage = null;

	[Header("設置アイテム")]
	[SerializeField] private GameObject[] m_putItems;

	[Header("レイヤーマスク")]
	[SerializeField] private LayerMask m_layerMask;


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

		Debug.Log(rayCast.point);
		// ブロックに当たった
		if (rayCast)
		{
			mousePos = rayCast.point + (rayCast.normal * new Vector2(0.1f, 0.1f));
		}

		//mousePos = rayCast.point;
		//Debug.Log(rayCast.point);

		// プレイヤーとマウスカーソルの位置が設置範囲内
		if (Vector2.Distance(playerPos, mousePos) < m_itemSettingRange)
		{
			// 四捨五入する
			mousePos = RoundHalfUp(mousePos);

			// ツールの設置位置をマウスカーソルの位置にする
			m_cursorImage.transform.position = mousePos;
		}
		else
		{
			// 届く最大範囲に設定
			mousePos = playerPos + (playerToMouse * m_itemSettingRange);

			// 四捨五入する
			mousePos = RoundHalfUp(mousePos);

			// アイテムの設置位置
			m_cursorImage.transform.position = mousePos;
		}

	}

	// ツール設置
	public void Put()
    {
		// 選択されているアイテムが作成できるか
		//if (!CheckCreate(作りたいアイテムの種類))

		// アイテムを置く
		GameObject tool = Instantiate(m_putItems[0]);
		// 座標設定
		tool.transform.position = m_cursorImage.transform.position;
    }


	Vector2 RoundHalfUp(Vector2 value)
	{
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);

		return value;
	}

	// 四捨五入
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
