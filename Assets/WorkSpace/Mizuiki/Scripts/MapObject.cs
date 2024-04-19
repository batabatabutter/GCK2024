using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("ブロックの色")]
    [SerializeField] private Color m_blockColor = Color.white;

    // 自身のスプライト
    private SpriteRenderer m_ownSprite;


    // Start is called before the first frame update
    void Start()
    {
        // スプライトの取得
        m_ownSprite = GetComponent<SpriteRenderer>();
        m_ownSprite.color = m_blockColor;
    }

	// 明度の設定
	public void SetValue(float value)
	{
		Color color = m_blockColor * Color.HSVToRGB(0.0f, 0.0f, value);
		m_ownSprite.color = color;
	}

	// ブロックの色
	public Color BlockColor
    {
        get { return m_blockColor; }
        set { m_blockColor = value; }
    }

}
