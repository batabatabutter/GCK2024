using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DunegonData_", menuName = "CreateDungeonDataCSV")]
public class DungeonDataCSV : DungeonData
{
	[Header("個別")]

	[Header("ダンジョンのCSV")]
	[SerializeField] private List<TextAsset> dungeonCSV;

	public List<TextAsset> DungeonCSV => dungeonCSV;
}
