

using UnityEngine;

[CreateAssetMenu(fileName = "DungeonDataBase", menuName = "CreateDungeonDataBase")]
public class DungeonDataBase : ScriptableObject
{
	[Header("ダンジョンのCSV")]
	public Object[] dungeonCSV;

}