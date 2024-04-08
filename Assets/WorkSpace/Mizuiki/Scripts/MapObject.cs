using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("�u���b�N�̐F")]
    [SerializeField] private Color m_blockColor = Color.white;

    [Header("�e�̃X�v���C�g")]
    [SerializeField] private SpriteRenderer m_parentSprite;
    // ���g�̃X�v���C�g
    private SpriteRenderer m_ownSprite;


    // Start is called before the first frame update
    void Start()
    {
        // �X�v���C�g�̎擾
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
