using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEntryBlock : CheckBlock
{
	[Header("---------- 入口 ----------")]

	[Header("ステージ番号")]
	[SerializeField] private int m_stageNum = 0;

	[Header("意思確認キャンバス")]
	[SerializeField] private StageEntryCanvas m_entryCanvas = null;



	// ダメージを与えられたら確認キャンバスを出す
	public override bool AddMiningDamage(float power, int dropCount = 1)
	{
		// ステージ番号設定
		m_entryCanvas.StageNum = m_stageNum;

		// 表示テキスト
		m_entryCanvas.Text = "ステージ" + m_stageNum;

		// キャンバス表示
		m_entryCanvas.SetEnabled(true);

		return false;
	}


	// ステージ番号
	public int StageNum
	{
		set { m_stageNum = value; }
	}
	// キャンバス
	public StageEntryCanvas StageEntryCanvas
	{
		set { m_entryCanvas = value; }
	}
}
