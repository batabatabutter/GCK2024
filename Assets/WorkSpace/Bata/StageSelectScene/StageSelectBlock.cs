using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StageSelectBlock : Block
{
    //  �X�e�[�W�ԍ��I��p
    [Header("�X�e�[�W�ԍ�")]
    [SerializeField] StageNumScriptableObject m_stageNumObj;

    //  �e�L�X�g
    [Header("�e�L�X�g")]
    [SerializeField] Text m_stageText;

    //  �X�e�[�W�ԍ�
    [SerializeField] int m_stageNum;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		
	}

	public void SetStageNum(int num)
    {
        m_stageNum = num;
        m_stageText.text = "Stage: " + (m_stageNum + 1).ToString();
    }

    public override void DropItem(int stageID = 1)
    {
        m_stageNumObj.stageNum = m_stageNum;
        SceneManager.LoadScene("PlayScene");
    }

    private void ChangeScene()
    {
		m_stageNumObj.stageNum = m_stageNum;
		SceneManager.LoadScene("PlayScene");
	}

}
