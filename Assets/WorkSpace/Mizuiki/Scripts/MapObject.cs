using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("ブロックの色")]
    [SerializeField] private Color m_blockColor = Color.white;

    [Header("親のブロックスクリプト")]
    [SerializeField] private Block m_parent = null;
    [Header("親のスプライト")]
    [SerializeField] private SpriteRenderer m_parentSprite;
    // 自身のスプライト
    private SpriteRenderer m_ownSprite;


    // Start is called before the first frame update
    void Start()
    {
        // スプライトの取得
        m_ownSprite = GetComponent<SpriteRenderer>();
        m_ownSprite.color = m_blockColor;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = m_blockColor * Color.HSVToRGB(0.0f, 0.0f, m_parent.ReceiveLightValue);
        m_ownSprite.color = color;
    }


    // ブロックの色
    public Color BlockColor
    {
        get { return m_blockColor; }
        set { m_blockColor = value; }
    }
    // 親
    public Block Parent
    {
        set { m_parent = value; }
    }
    // 親のスプライト
    public SpriteRenderer ParentSprite
    {
        get { return m_parentSprite; }
        set { m_parentSprite = value; }
    }

}
