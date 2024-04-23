using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DunegonData_", menuName = "CreateDataBase/Dungeon/DungeonDataCSV")]
public class DungeonDataCSV : DungeonData
{
	[Header("ŒÂ•Ê")]

	[Header("ƒ_ƒ“ƒWƒ‡ƒ“‚ÌCSV")]
	[SerializeField] private List<TextAsset> dungeonCSV;

	public List<TextAsset> DungeonCSV => dungeonCSV;
}
