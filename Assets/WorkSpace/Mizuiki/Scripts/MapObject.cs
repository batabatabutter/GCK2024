using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("ブロックの色")]
    [SerializeField] private Color m_blockColor = Color.white;

	// 明度
	public float m_value = 0.0f;
    // 自身のスプライト
    private SpriteRenderer m_ownSprite;

	private void Awake()
	{
        // スプライトの取得
        m_ownSprite = GetComponent<SpriteRenderer>();
        m_ownSprite.color = m_blockColor;
	}

	// 明度の設定
	public void SetValue(float value)
	{
		Color color = m_blockColor * new Color(value, value, value, 1.0f);
		m_ownSprite.color = color;
		m_value = value;
	}

	// ブロックの色
	public Color BlockColor
    {
        get { return m_blockColor; }
        set { m_blockColor = value; }
    }

}
