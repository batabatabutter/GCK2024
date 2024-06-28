using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttackTable
{
	// 攻撃パターンのインデックス
	private int m_patternIndex;

	// 攻撃テーブル
	private DungeonAttackTableData m_table;


	public int PatternIndex
	{
		get { return m_patternIndex; }
		set { m_patternIndex = value; }
	}
	public DungeonAttackTableData Table
	{
		get { return m_table; }
		set { m_table = value; }
	}

}
