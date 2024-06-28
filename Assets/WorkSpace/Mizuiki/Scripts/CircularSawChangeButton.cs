using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularSawChangeButton : MonoBehaviour
{
	[Header("変更する種類")]
	[SerializeField] private MiningData.MiningType m_miningType;

	[Header("丸のこ")]
	[SerializeField] private CircularSaw m_circularSaw = null;

	[Header("切りかえクラス")]
	[SerializeField] CircularSawChange m_circularChange = null;

	[Header("ボタン")]
	[SerializeField] private Button m_button = null;



	private void Awake()
	{
		// ボタン取得
		if (m_button == null)
		{
			m_button = GetComponent<Button>();
		}
		// 呼び出し関数設定
		m_button.onClick.AddListener(OnClick);
	}

	// 呼び出し
	public void OnClick()
	{
		// 丸のこがない
		if (m_circularSaw == null)
			return;

		// 種類変更
		m_circularSaw.SetType(m_miningType);

		// セレクター変更
		m_circularChange.ChangeSelector();

	}

	// UI座標取得
	public Vector2 GetRectPosition()
	{
		return m_button.image.rectTransform.anchoredPosition;
	}


	// 種類
	public MiningData.MiningType MiningType
	{
		get { return m_miningType; }
	}

	// 丸のこ
	public CircularSaw CircularSaw
	{
		set { m_circularSaw = value; }
	}

}
