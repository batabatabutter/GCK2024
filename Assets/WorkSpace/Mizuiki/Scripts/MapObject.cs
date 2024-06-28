using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("�u���b�N�̐F")]
    [SerializeField] private Color m_blockColor = Color.white;

	// ���x
	public float m_value = 0.0f;
    // ���g�̃X�v���C�g
    private SpriteRenderer m_ownSprite;

	private void Awake()
	{
        // �X�v���C�g�̎擾
        m_ownSprite = GetComponent<SpriteRenderer>();
        m_ownSprite.color = m_blockColor;
	}

	// ���x�̐ݒ�
	public void SetValue(float value)
	{
		Color color = m_blockColor * new Color(value, value, value, 1.0f);
		m_ownSprite.color = color;
		m_value = value;
	}

	// �u���b�N�̐F
	public Color BlockColor
    {
        get { return m_blockColor; }
        set { m_blockColor = value; }
    }

}
