using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageEntryCanvas : CheckCanvas
{
	[Header("---------- ���� ----------")]

	[Header("�X�e�[�W�ԍ�")]
	[SerializeField] private int m_stageNum = 0;

	[Header("�V�[���}�l�[�W��"), Tooltip("�V�[���؂�ւ��p")]
	[SerializeField] private StageSelectManager m_stageSelectManager = null;

	[Header("�e�L�X�g")]
	[SerializeField] private Text m_text;


	// ����
	public override void Decision()
	{
		// �V�[���؂�ւ�
		m_stageSelectManager.ChangeScene(m_stageNum);
	}

	// �X�e�[�W�ԍ�
	public int StageNum
	{
		get { return m_stageNum; }
		set { m_stageNum = value; }
	}
	public string Text
	{
		set { m_text.text = value; }
	}
}
