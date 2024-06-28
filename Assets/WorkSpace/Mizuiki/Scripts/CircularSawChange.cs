using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularSawChange : MonoBehaviour
{
	[Header("�ۂ̂�")]
	[SerializeField] private CircularSaw m_circularSaw = null;

	[Header("�ۂ̂��ύX�L�����o�X")]
	[SerializeField] private Canvas m_canvas = null;

	[Header("�ۂ̂��I����ʗp�o�[�`�����J����")]
	[SerializeField] private CinemachineVirtualCamera m_virtualCamera = null;

	[Header("�{�^��")]
	[SerializeField] private CircularSawChangeButton[] m_buttons = null;

	[Header("�I�����ڗp�摜")]
	[SerializeField] private Image m_selectImage = null;
	// �I�����Ƃ��̈ʒu
	private Dictionary<MiningData.MiningType, Vector2> m_iconPosition = new();

	[Header("���a")]
	[SerializeField] private float m_radius = 1.0f;

	[Header("�R���C�_�[")]
	[SerializeField] private CircleCollider2D m_circleCollider = null;


	private void Awake()
	{
		// ���a�ݒ�
		SetRadius();
		// �{�^���ݒ�
		SetButtons();

		// �L�����o�X���\���ɂ��Ă���
		m_canvas.gameObject.SetActive(false);

		// �o�[�`�����J�����𖳌��ɂ��Ă���
		m_virtualCamera.enabled = false;

	}

	// ���a�̔��f
	[ContextMenu("SetRadius")]
	public void SetRadius()
	{
		// �R���C�_�[���ݒ肳��Ă��Ȃ�
		if (TryGetComponent(out CircleCollider2D col))
		{
			m_circleCollider = col;
		}
		else if (m_circleCollider == null)
		{
			m_circleCollider = gameObject.AddComponent<CircleCollider2D>();
		}

		// �g���K�[�ɂ���
		m_circleCollider.isTrigger = true;
		// �R���C�_�[�̔��a��ݒ肷��
		m_circleCollider.radius = m_radius;
	}

	// �{�^���̐ݒ�
	public void SetButtons()
	{
		// �ۂ̂����Ȃ���ΐݒ肵�Ȃ�
		if (m_circularSaw == null)
			return;

		foreach(CircularSawChangeButton button in m_buttons)
		{
			// �ۂ̂���ݒ�
			button.CircularSaw = m_circularSaw;
			// �ʒu���擾
			m_iconPosition[button.MiningType] = button.GetRectPosition();
		}
	}

	// �Z���N�^�[�̈ʒu��ύX����
	public void ChangeSelector()
	{
		m_selectImage.rectTransform.anchoredPosition = m_iconPosition[m_circularSaw.MiningType];
	}


	// �߂Â�����L�����o�X��\������
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �����������̂��v���C���[�ł͂Ȃ�
		if (!collision.gameObject.CompareTag("Player"))
			return;

		// �L�����o�X��\������
		m_canvas.gameObject.SetActive(true);

		// �o�[�`�����J������L���ɂ���
		m_virtualCamera.enabled = true;

		// �I���ʒu�ݒ�
		ChangeSelector();
	}

	// ���ꂽ��L�����o�X���\���ɂ���
	private void OnTriggerExit2D(Collider2D collision)
	{
		// ���ꂽ���̂��v���C���[�ł͂Ȃ�
		if (!collision.gameObject.CompareTag("Player"))
			return;

		// �L�����o�X���\���ɂ���
		m_canvas.gameObject.SetActive(false);

		// �o�[�`�����J�����𖳌��ɂ���
		m_virtualCamera.enabled = false;
	}
}
