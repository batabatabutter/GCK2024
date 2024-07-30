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

	[Header("���x���ύX�{�^��")]
	[SerializeField] private Image m_buttonLevelDown = null;
	[SerializeField] private Image m_buttonLevelUp = null;

	[Header("���x���\���e�L�X�g")]
	[SerializeField] private InputField m_inputFieldLevel = null;


	private void OnEnable()
	{
		// �ő僌�x��
		int maxLevel = SaveDataReadWrite.m_instance.GetDungeonLevel(m_stageNum);
		SetStageLevel(maxLevel);
	}

	private void Start()
	{
		ChangeStageLevel();
	}

	// ����
	public override void Decision()
	{
		base.Decision();

		// �V�[���؂�ւ�
		m_stageSelectManager.ChangeScene(m_stageNum);
	}

	// �X�e�[�W���x���ύX
	public void ChangeStageLevel()
	{
		ChangeStageLevel(0);
	}
	public void ChangeStageLevel(int change)
	{
		// ���݂̃X�e�[�W���x��
		int now = int.Parse(m_inputFieldLevel.text);
		// �ϓ��ʂ����Z
		now += change;
		// ���x���ݒ�
		SetStageLevel(now);
	}
	// �X�e�[�W���x���ݒ�
	public void SetStageLevel(int level)
	{
		// �ő僌�x��
		int maxLevel = MyFunction.MAX_STAGE_LEVEL;
		if (SaveDataReadWrite.m_instance)
		{
			maxLevel = Mathf.Min(SaveDataReadWrite.m_instance.GetDungeonLevel(m_stageNum));
		}
		// �X�e�[�W���x���ŃN�����v(�ő�l�𒴂��Ȃ��悤��)
		level = Mathf.Clamp(level, 1, maxLevel);
		// �X�e�[�W���x����������
		m_inputFieldLevel.text = level.ToString();

		// �\���ؑ�
		ChangeArrow(level, maxLevel);
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



	// ���̕\���ύX
	private void ChangeArrow(int level, int max)
	{
		if (level == 1)
		{
			// ���x�������{�^����\��
			m_buttonLevelDown.gameObject.SetActive(false);
		}
        else
        {
			// ���x�������{�^���\��
			m_buttonLevelDown.gameObject.SetActive(true);
        }

		if (level == MyFunction.MAX_STAGE_LEVEL)
		{
			// ���x�������{�^����\��
			m_buttonLevelUp.gameObject.SetActive(false);
		}
		else
		{
			// ���x�������{�^���\��
			m_buttonLevelUp.gameObject.SetActive(true);
		}

		if (level < max)
		{
			// �܂��オ���
			m_buttonLevelUp.color = Color.white;
		}
		else
		{
			// ���͂����܂�
			m_buttonLevelUp.color = Color.gray;
		}

    }

}
