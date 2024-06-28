using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageEntryCanvas : CheckCanvas
{
	[Header("---------- 入口 ----------")]

	[Header("ステージ番号")]
	[SerializeField] private int m_stageNum = 0;

	[Header("シーンマネージャ"), Tooltip("シーン切り替え用")]
	[SerializeField] private StageSelectManager m_stageSelectManager = null;

	[Header("テキスト")]
	[SerializeField] private Text m_text;


	// 決定
	public override void Decision()
	{
		// シーン切り替え
		m_stageSelectManager.ChangeScene(m_stageNum);
	}

	// ステージ番号
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
