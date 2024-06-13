using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StageSelectBlock : Block
{
    //  ステージ番号選択用
    [Header("ステージ番号")]
    [SerializeField] StageNumScriptableObject m_stageNumObj;

    //  テキスト
    [Header("テキスト")]
    [SerializeField] Text m_stageText;

    //  ステージ番号
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
