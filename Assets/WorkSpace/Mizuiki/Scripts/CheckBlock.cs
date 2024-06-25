using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : Block
{
	[Header("�ӎv�m�F�L�����o�X")]
	[SerializeField] private CheckCanvas m_checkCanvas = null;

	[Header("�R�A�A�C�R���̃X�v���C�g")]
	[SerializeField] private SpriteRenderer m_spriteRenderer = null;



	// �_���[�W��^����ꂽ��m�F�L�����o�X���o��
	public override bool AddMiningDamage(float power, int dropCount = 1)
	{
		// �L�����o�X�\��
		m_checkCanvas.SetEnabled(true);

		return false;
	}


	// �L�����o�X
	public CheckCanvas CheckCanvas
	{
		set { m_checkCanvas = value; }
	}
	// �R�A�A�C�R��
	public SpriteRenderer CoreIcon
	{
		get { return m_spriteRenderer; }
		set { m_spriteRenderer = value; }
	}

}
