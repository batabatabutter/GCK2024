using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularSawChangeButton : MonoBehaviour
{
	[Header("�ύX������")]
	[SerializeField] private MiningData.MiningType m_miningType;

	[Header("�ۂ̂�")]
	[SerializeField] private CircularSaw m_circularSaw = null;

	[Header("�؂肩���N���X")]
	[SerializeField] CircularSawChange m_circularChange = null;

	[Header("�{�^��")]
	[SerializeField] private Button m_button = null;



	private void Awake()
	{
		// �{�^���擾
		if (m_button == null)
		{
			m_button = GetComponent<Button>();
		}
		// �Ăяo���֐��ݒ�
		m_button.onClick.AddListener(OnClick);
	}

	// �Ăяo��
	public void OnClick()
	{
		// �ۂ̂����Ȃ�
		if (m_circularSaw == null)
			return;

		// ��ޕύX
		m_circularSaw.SetType(m_miningType);

		// �Z���N�^�[�ύX
		m_circularChange.ChangeSelector();

	}

	// UI���W�擾
	public Vector2 GetRectPosition()
	{
		return m_button.image.rectTransform.anchoredPosition;
	}


	// ���
	public MiningData.MiningType MiningType
	{
		get { return m_miningType; }
	}

	// �ۂ̂�
	public CircularSaw CircularSaw
	{
		set { m_circularSaw = value; }
	}

}
