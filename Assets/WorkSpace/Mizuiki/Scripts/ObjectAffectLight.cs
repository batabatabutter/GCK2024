using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAffectLight : MonoBehaviour
{
	[Header("自分自身の発する光源レベル")]
	[SerializeField] private int m_lightLevel = 0;

	[Header("受けている光源レベル")]
	[SerializeField] private int m_receiveLightLevel = 0;
	// 明度
	private float m_receiveLightValue = 1.0f;

	[Header("スプライトレンダー")]
	[SerializeField] private SpriteRenderer m_spriteRenderer;



	// Start is called before the first frame update
	void Start()
    {
		// スプライトレンダーがなければ取得
		if (!m_spriteRenderer)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}
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
			// 明るさレベルの設定(受けている光源レベルと自身の光源レベルを比較して大きいほうの明るさになる)
			m_receiveLightLevel = Mathf.Max(value, m_lightLevel);
			// 明度を計算
			m_receiveLightValue = m_receiveLightLevel / 7.0f;
			// 明度の値を 0 ~ 1 にクランプ
			m_receiveLightValue = Mathf.Clamp(m_receiveLightValue, 0.0f, 1.0f);
			// 透明度を設定
			if (m_spriteRenderer)
			{
				m_spriteRenderer.color = new (1.0f, 1.0f, 1.0f, 1.0f - m_receiveLightValue);
			}
		}
	}

}
