using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "APT_", menuName = "CreateDataBase/Dungeon/Attack/AttackTable")]
public class DungeonAttackTableData : ScriptableObject
{
	[Header("�U���e�[�u����")]
	[SerializeField] private string tableName;
	[Header("臒l")]
	[SerializeField] private float overNum;
	[Header("�p�^�[���̃��X�g")]
	[SerializeField] private List<DungeonAttackPattern> pattern;


	public string TableName => tableName;
	public float OverNum => overNum;
	public List<DungeonAttackPattern> Pattern => pattern;
}
