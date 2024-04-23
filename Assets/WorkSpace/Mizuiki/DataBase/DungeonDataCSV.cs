using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DunegonData_", menuName = "CreateDataBase/Dungeon/DungeonDataCSV")]
public class DungeonDataCSV : DungeonData
{
	[Header("��")]

	[Header("�_���W������CSV")]
	[SerializeField] private List<TextAsset> dungeonCSV;

	public List<TextAsset> DungeonCSV => dungeonCSV;
}
