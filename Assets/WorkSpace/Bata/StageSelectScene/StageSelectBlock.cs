using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StageSelectBlock : Block
{
    //  �X�e�[�W�ԍ��I��p
    [Header("�X�e�[�W�ԍ�")]
    [SerializeField] StageNumScriptableObject m_stageNumObj;
    //  �X�e�[�W�ԍ�
    [SerializeField] int m_stageNum;

    public override void DropItem(int stageID = 1)
    {
        m_stageNumObj.stageNum = m_stageNum;
        SceneManager.LoadScene("PlayScene");
    }
}
