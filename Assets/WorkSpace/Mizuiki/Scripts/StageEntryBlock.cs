using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEntryBlock : CheckBlock
{
	[Header("---------- ���� ----------")]

	[Header("�X�e�[�W�ԍ�")]
	[SerializeField] private int m_stageNum = 0;

	[Header("�ӎv�m�F�L�����o�X")]
	[SerializeField] private StageEntryCanvas m_entryCanvas = null;



	// �_���[�W��^����ꂽ��m�F�L�����o�X���o��
	public override bool AddMiningDamage(float power, int dropCount = 1)
	{
		// �X�e�[�W�ԍ��ݒ�
		m_entryCanvas.StageNum = m_stageNum;

		// �\���e�L�X�g
		m_entryCanvas.Text = "�X�e�[�W" + m_stageNum;

		// ���N���X
		base.AddMiningDamage(power, dropCount);

		return false;
	}

	// �v���C���[�����ꂽ��L�����o�X��\��
	private void OnTriggerExit2D(Collider2D collision)
	{
		// ���ꂽ�̂��v���C���[
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// �L�����o�X��\��
			m_entryCanvas.SetEnabled(false);
		}
	}


	// �X�e�[�W�ԍ�
	public int StageNum
	{
		set { m_stageNum = value; }
	}
	// �L�����o�X
	public StageEntryCanvas StageEntryCanvas
	{
		set { m_entryCanvas = value; }
	}
}
