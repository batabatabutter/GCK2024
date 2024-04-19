using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("�u���b�N�̐F")]
    [SerializeField] private Color m_blockColor = Color.white;

    [Header("�e�̃u���b�N�X�N���v�g")]
    [SerializeField] private Block m_parent = null;
    [Header("�e�̃X�v���C�g")]
    [SerializeField] private SpriteRenderer m_parentSprite;
    // ���g�̃X�v���C�g
    private SpriteRenderer m_ownSprite;


    // Start is called before the first frame update
    void Start()
    {
        // �X�v���C�g�̎擾
        m_ownSprite = GetComponent<SpriteRenderer>();
        m_ownSprite.color = m_blockColor;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = m_blockColor * Color.HSVToRGB(0.0f, 0.0f, m_parent.ReceiveLightValue);
        m_ownSprite.color = color;
    }


    // �u���b�N�̐F
    public Color BlockColor
    {
        get { return m_blockColor; }
        set { m_blockColor = value; }
    }
    // �e
    public Block Parent
    {
        set { m_parent = value; }
    }
    // �e�̃X�v���C�g
    public SpriteRenderer ParentSprite
    {
        get { return m_parentSprite; }
        set { m_parentSprite = value; }
    }

}
