using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAffectLight : MonoBehaviour
{
	[Header("自分自身の発する光源レベル")]
	[SerializeField] private int m_lightLevel = 0;
	[Header("受けている光源レベル")]
	[SerializeField] private int m_receiveLightLevel = 0;
	private float m_receiveLightValue = 1.0f;    // 明度
	[Header("スプライトレンダー")]
	[SerializeField] protected SpriteRenderer m_spriteRenderer;

	//[Header("オブジェクトの色")]
	//[SerializeField] private Color m_color = Color.white;

	//[Header("子のマップオブジェクト")]
	//[SerializeField] private MapObject m_mapObject = null;

	////	光処理
	//private bool m_brightness = true;



	// Start is called before the first frame update
	void Start()
    {
		// スプライトレンダーがなければ取得
		if (!m_spriteRenderer)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}
		// 光源初期化
		//ReceiveLightLevel = 0;
	}

	// 自身の持つ光源レベル
	public int LightLevel
	{
		get { return m_lightLevel; }
		set { m_lightLevel = value; }
	}

	// 受けている明るさ
	public int ReceiveLightLevel
	{
		get { return m_receiveLightLevel; }
		set
		{
			//// 光源レベルが変わっていない
			//if (m_receiveLightLevel == value)
			//	return;
			// 明るさレベルの設定(受けている光源レベルと自身の光源レベルを比較して大きいほうの明るさになる)
			m_receiveLightLevel = Mathf.Max(value, m_lightLevel);
			// 明度を計算
			m_receiveLightValue = m_receiveLightLevel / 7.0f;
			// 明度の値を 0 ~ 1 にクランプ
			m_receiveLightValue = Mathf.Clamp(m_receiveLightValue, 0.0f, 1.0f);
			// 透明度を設定
			if (m_spriteRenderer)
				m_spriteRenderer.color = new (1.0f, 1.0f, 1.0f, 1.0f - m_receiveLightValue);
			//// マップオブジェクトの明度を設定
			//if (m_mapObject)
			//{
			//	m_mapObject.SetValue(m_receiveLightValue);
			//}
		}
	}

	//// マップオブジェクト
	//public MapObject MapObject
	//{
	//	set { m_mapObject = value; }
	//}

	//// 色
	//public Color Color
	//{
	//	set { m_color = value; }
	//}

	////	光処理フラグ
	//public bool BrightnessFlag
	//{
	//	get { return m_brightness; }
	//	set { m_brightness = value; }
	//}
}
