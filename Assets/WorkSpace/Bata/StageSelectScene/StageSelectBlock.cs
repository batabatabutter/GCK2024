using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StageSelectBlock : Block
{
    [Header("---------- �X�e�[�W�I�� ----------")]

    //  �X�e�[�W�ԍ��I��p
    [Header("�X�e�[�W�ԍ�")]
    [SerializeField] StageNumScriptableObject m_stageNumObj;

    [Header("�}�l�[�W��")]
    [SerializeField] private StageSelectManager m_stageSelectManager = null;

    //  �e�L�X�g
    [Header("�e�L�X�g")]
    [SerializeField] Text m_stageText;

    //  �X�e�[�W�ԍ�
    [SerializeField] int m_stageNum;

	public void SetStageNum(int num)
    {
        m_stageNum = num;
        m_stageText.text = "Stage: " + (m_stageNum + 1).ToString();
    }

    public override void DropItem(int stageID = 1)
    {
        m_stageSelectManager.ChangeScene(m_stageNum);
    }

    // �}�l�[�W��
    public StageSelectManager StageSelectManager
    {
        set { m_stageSelectManager = value; }
    }

    //   private void ChangeScene()
    //   {
    //       // �X�e�[�W�ԍ��ݒ�
    //	m_stageNumObj.stageNum = m_stageNum;
    //       // �����ݒ�
    //       SaveDataReadWrite.m_instance.MiningType = ;
    //       // �Z�[�u
    //       SaveDataReadWrite.m_instance.Save();
    //       // �V�[���ǂݍ���
    //	SceneManager.LoadScene("PlayScene");
    //}

}
