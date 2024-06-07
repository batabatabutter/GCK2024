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
	[SerializeField] protected SpriteRenderer m_spriteRenderer;

	//[Header("�I�u�W�F�N�g�̐F")]
	//[SerializeField] private Color m_color = Color.white;

	//[Header("�q�̃}�b�v�I�u�W�F�N�g")]
	//[SerializeField] private MapObject m_mapObject = null;

	////	������
	//private bool m_brightness = true;



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
			//// �������x�����ς���Ă��Ȃ�
			//if (m_receiveLightLevel == value)
			//	return;
			// ���邳���x���̐ݒ�(�󂯂Ă���������x���Ǝ��g�̌������x�����r���đ傫���ق��̖��邳�ɂȂ�)
			m_receiveLightLevel = Mathf.Max(value, m_lightLevel);
			// ���x���v�Z
			m_receiveLightValue = m_receiveLightLevel / 7.0f;
			// ���x�̒l�� 0 ~ 1 �ɃN�����v
			m_receiveLightValue = Mathf.Clamp(m_receiveLightValue, 0.0f, 1.0f);
			// �����x��ݒ�
			if (m_spriteRenderer)
				m_spriteRenderer.color = new (1.0f, 1.0f, 1.0f, 1.0f - m_receiveLightValue);
			//// �}�b�v�I�u�W�F�N�g�̖��x��ݒ�
			//if (m_mapObject)
			//{
			//	m_mapObject.SetValue(m_receiveLightValue);
			//}
		}
	}

	//// �}�b�v�I�u�W�F�N�g
	//public MapObject MapObject
	//{
	//	set { m_mapObject = value; }
	//}

	//// �F
	//public Color Color
	//{
	//	set { m_color = value; }
	//}

	////	�������t���O
	//public bool BrightnessFlag
	//{
	//	get { return m_brightness; }
	//	set { m_brightness = value; }
	//}
}
