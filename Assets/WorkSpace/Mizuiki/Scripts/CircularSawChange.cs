using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularSawChange : MonoBehaviour
{
	[Header("丸のこ")]
	[SerializeField] private CircularSaw m_circularSaw = null;

	[Header("丸のこ変更キャンバス")]
	[SerializeField] private Canvas m_canvas = null;

	[Header("ボタン")]
	[SerializeField] private CircularSawChangeButton[] m_buttons = null;

	[Header("選択項目用画像")]
	[SerializeField] private Image m_selectImage = null;
	// 選択肢とその位置
	private Dictionary<MiningData.MiningType, Vector2> m_iconPosition = new();

	[Header("半径")]
	[SerializeField] private float m_radius = 1.0f;

	[Header("コライダー")]
	[SerializeField] private CircleCollider2D m_circleCollider = null;


	private void Awake()
	{
		// 半径設定
		SetRadius();
		// ボタン設定
		SetButtons();
	}

	private void Update()
	{
		m_selectImage.rectTransform.anchoredPosition = m_iconPosition[m_circularSaw.MiningType];
	}

	// 半径の反映
	[ContextMenu("SetRadius")]
	public void SetRadius()
	{
		// コライダーが設定されていない
		if (TryGetComponent(out CircleCollider2D col))
		{
			m_circleCollider = col;
		}
		else if (m_circleCollider == null)
		{
			m_circleCollider = gameObject.AddComponent<CircleCollider2D>();
		}

		// トリガーにする
		m_circleCollider.isTrigger = true;
		// コライダーの半径を設定する
		m_circleCollider.radius = m_radius;
	}

	// ボタンの設定
	public void SetButtons()
	{
		// 丸のこがなければ設定しない
		if (m_circularSaw == null)
			return;

		foreach(CircularSawChangeButton button in m_buttons)
		{
			// 丸のこを設定
			button.CircularSaw = m_circularSaw;
			// 位置を取得
			m_iconPosition[button.MiningType] = button.GetRectPosition();
		}
	}

	// 近づいたらキャンバスを表示する
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 当たったものがプレイヤーではない
		if (!collision.gameObject.CompareTag("Player"))
			return;

		// キャンバスを表示する
		m_canvas.gameObject.SetActive(true);
	}

	// 離れたらキャンバスを非表示にする
	private void OnTriggerExit2D(Collider2D collision)
	{
		// 離れたものがプレイヤーではない
		if (!collision.gameObject.CompareTag("Player"))
			return;

		// キャンバスを非表示にする
		m_canvas.gameObject.SetActive(false);
	}
}
