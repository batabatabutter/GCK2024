using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAffectLight : MonoBehaviour
{
	[Header("�������g�̔�����������x��")]
	[SerializeField] private int m_lightLevel = 0;
	[Header("�󂯂Ă���������x��")]
	[SerializeField] private int m_receiveLightLevel = 0;
	private float m_receiveLightValue = 1.0f;    // ���x
	[Header("�X�v���C�g�����_�[")]
	[SerializeField] private SpriteRenderer m_spriteRenderer;

	[Header("�q�̃}�b�v�I�u�W�F�N�g")]
	[SerializeField] private MapObject m_mapObject = null;


	private void Awake()
	{
	}

	// Start is called before the first frame update
	void Start()
    {
		// �X�v���C�g�����_�[���Ȃ���Ύ擾
		if (!m_spriteRenderer)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}
		// ����������
		//ReceiveLightLevel = 0;
	}

	// ���g�̎��������x��
	public int LightLevel
	{
		get { return m_lightLevel; }
		set { m_lightLevel = value; }
	}

	// �󂯂Ă��閾�邳
	public int ReceiveLightLevel
	{
		get { return m_receiveLightLevel; }
		set
		{
			// ���邳���x���̐ݒ�(�󂯂Ă���������x���Ǝ��g�̌������x�����r���đ傫���ق��̖��邳�ɂȂ�)
			m_receiveLightLevel = Mathf.Max(value, m_lightLevel);
			// ���x���v�Z
			m_receiveLightValue = m_receiveLightLevel / 7.0f;
			// ���x�̒l�� 0 ~ 1 �ɃN�����v
			m_receiveLightValue = Mathf.Clamp(m_receiveLightValue, 0.0f, 1.0f);
			// �F��ݒ�
			m_spriteRenderer.color = new(m_receiveLightValue, m_receiveLightValue, m_receiveLightValue, 1.0f);
			// �}�b�v�I�u�W�F�N�g�̖��x��ݒ�
			if (m_mapObject)
			{
				m_mapObject.SetValue(m_receiveLightValue);
			}
		}
	}

	// �}�b�v�I�u�W�F�N�g
	public MapObject MapObject
	{
		set { m_mapObject = value; }
	}

}
