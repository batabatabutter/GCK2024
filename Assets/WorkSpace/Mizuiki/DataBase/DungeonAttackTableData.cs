using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "APT_", menuName = "CreateDataBase/Dungeon/Attack/AttackTable")]
public class DungeonAttackTableData : ScriptableObject
{
	[Header("攻撃テーブル名")]
	[SerializeField] private string tableName;
	[Header("閾値")]
	[SerializeField] private float overNum;
	[Header("パターンのリスト")]
	[SerializeField] private List<DungeonAttackPattern> pattern;


	public string TableName => tableName;
	public float OverNum => overNum;
	public List<DungeonAttackPattern> Pattern => pattern;
}
