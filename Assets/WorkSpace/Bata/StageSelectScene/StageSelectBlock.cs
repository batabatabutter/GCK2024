using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StageSelectBlock : Block
{
    [Header("---------- ステージ選択 ----------")]

    //  ステージ番号選択用
    [Header("ステージ番号")]
    [SerializeField] StageNumScriptableObject m_stageNumObj;

    [Header("マネージャ")]
    [SerializeField] private StageSelectManager m_stageSelectManager = null;

    //  テキスト
    [Header("テキスト")]
    [SerializeField] Text m_stageText;

    //  ステージ番号
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

    // マネージャ
    public StageSelectManager StageSelectManager
    {
        set { m_stageSelectManager = value; }
    }

    //   private void ChangeScene()
    //   {
    //       // ステージ番号設定
    //	m_stageNumObj.stageNum = m_stageNum;
    //       // 装備設定
    //       SaveDataReadWrite.m_instance.MiningType = ;
    //       // セーブ
    //       SaveDataReadWrite.m_instance.Save();
    //       // シーン読み込み
    //	SceneManager.LoadScene("PlayScene");
    //}

}
