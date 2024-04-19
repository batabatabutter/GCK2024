using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("�u���b�N�̐F")]
    [SerializeField] private Color m_blockColor = Color.white;

    // ���g�̃X�v���C�g
    private SpriteRenderer m_ownSprite;


    // Start is called before the first frame update
    void Start()
    {
        // �X�v���C�g�̎擾
        m_ownSprite = GetComponent<SpriteRenderer>();
        m_ownSprite.color = m_blockColor;
    }

	// ���x�̐ݒ�
	public void SetValue(float value)
	{
		Color color = m_blockColor * Color.HSVToRGB(0.0f, 0.0f, value);
		m_ownSprite.color = color;
	}

	// �u���b�N�̐F
	public Color BlockColor
    {
        get { return m_blockColor; }
        set { m_blockColor = value; }
    }

}
