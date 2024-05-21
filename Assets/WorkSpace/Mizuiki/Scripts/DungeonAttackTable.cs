using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttackTable
{
	// 攻撃パターンのインデックス
	private int m_patternIndex;

	// 攻撃テーブル
	private DungeonAttackData.AttackTable m_table;


	public int PatternIndex
	{
		get { return m_patternIndex; }
		set { m_patternIndex = value; }
	}
	public DungeonAttackData.AttackTable Table
	{
		get { return m_table; }
		set { m_table = value; }
	}

}
