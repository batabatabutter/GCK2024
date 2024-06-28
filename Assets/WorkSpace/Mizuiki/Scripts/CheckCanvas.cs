using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCanvas : MonoBehaviour
{
	[Header("�������g�̃L�����o�X")]
	[SerializeField] private Canvas m_canvas = null;


	private void Awake()
	{
		// �L�����o�X���ݒ肳��Ă��Ȃ���Ύ擾
		if (m_canvas == null)
		{
			m_canvas = GetComponent<Canvas>();
		}
		// ��\��
		SetEnabled(false);
	}

	// ����
	public virtual void Decision()
	{

	}

	// �L�����Z��
	public virtual void Cancel()
	{
		// �L�����o�X��\��
		SetEnabled(false);
	}

	// �L�����o�X�\��
	public virtual void SetEnabled(bool enabled)
	{
		m_canvas.gameObject.SetActive(enabled);
	}


}
