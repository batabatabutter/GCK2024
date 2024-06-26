using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DunegonData_", menuName = "CreateDataBase/Dungeon/DungeonDataCSV")]
public class DungeonDataCSV : DungeonData
{
	[Header("CSV")]

	[Header("ダンジョンのCSV")]
	[SerializeField] private List<TextAsset> dungeonCSV;

	public List<TextAsset> DungeonCSV => dungeonCSV;
}
