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
		// ベクトル正規化
		playerToMouse.Normalize();
		// プレイヤーから採掘方向へのRayCast
		RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_itemSettingRange);

		//mousePos.x = RoundHalfUp(rayCast.point.x);
		//mousePos.y = RoundHalfUp(rayCast.point.y);

		// 四捨五入する
		mousePos.x = RoundHalfUp(mousePos.x);
		mousePos.y = RoundHalfUp(mousePos.y);

		// プレイヤーとマウスカーソルの位置が設置範囲内
		if (Vector2.Distance(playerPos, mousePos) < m_itemSettingRange)
		{
			// 四捨五入する
			mousePos.x = RoundHalfUp(mousePos.x);
			mousePos.y = RoundHalfUp(mousePos.y);

			m_cursorImage.transform.position = mousePos;
		}
		else
		{
			//// プレイヤーの位置からマウスの位置へのベクトル
			//Vector2 playerToMouse = mousePos - playerPos;
			//// ベクトル正規化
			//playerToMouse.Normalize();

			mousePos = playerPos + (playerToMouse * m_itemSettingRange);

			// 四捨五入する
			mousePos.x = RoundHalfUp(mousePos.x);
			mousePos.y = RoundHalfUp(mousePos.y);

			// アイテムの設置位置
			m_cursorImage.transform.position = mousePos;
		}

	}


	public void Put()
    {
		// アイテムを置く
		GameObject tool = Instantiate(m_putItems[0]);
		// 座標設定
		tool.transform.position = m_cursorImage.transform.position;
    }


	// 四捨五入
	static float RoundHalfUp(float num)
	{
		// 小数点以下の取得
		float fraction = num - MathF.Floor(num);

		// 小数点以下が0.5未満
		if (fraction < 0.5f)
		{
			// 切り捨てる
			return MathF.Floor(num);
		}
		// 切り上げる
		return MathF.Floor(num) + 1.0f;

	}

}
