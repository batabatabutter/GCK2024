using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // ブロックの色
    private Color m_blockColor = Color.white;

    // 親のスプライト
    private SpriteRenderer m_parentSprite;
    // 自身のスプライト
    private SpriteRenderer m_ownSprite;


    // Start is called before the first frame update
    void Start()
    {
        // スプライトの取得
        m_ownSprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float h, s, v;
        Color.RGBToHSV(m_parentSprite.color, out h, out s, out v);

        Color color = m_blockColor * Color.HSVToRGB(0.0f, 0.0f, v);

        m_ownSprite.color = color;

    }


    public Color BlockColor
    {
        get { return m_blockColor; }
        set { m_blockColor = value; }
    }
    public SpriteRenderer ParentSprite
    {
        get { return m_parentSprite; }
        set { m_parentSprite = value; }
    }

}
