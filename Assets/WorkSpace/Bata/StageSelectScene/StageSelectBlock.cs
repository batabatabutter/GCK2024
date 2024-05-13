using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StageSelectBlock : Block
{
    //  ステージ番号選択用
    [Header("ステージ番号")]
    [SerializeField] StageNumScriptableObject m_stageNumObj;
    //  ステージ番号
    [SerializeField] int m_stageNum;

    public override void DropItem(int stageID = 1)
    {
        m_stageNumObj.stageNum = m_stageNum;
        SceneManager.LoadScene("PlayScene");
    }
}
