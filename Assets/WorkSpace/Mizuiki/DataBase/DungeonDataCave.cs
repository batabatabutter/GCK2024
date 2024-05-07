using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData_", menuName = "CreateDataBase/Dungeon/DunegonDataCave")]
public class DungeonDataCave : DungeonData
{
	[Header("Cave")]

	[Header("‹ó“´—¦"), Range(0.0f, 1.0f)]
	[SerializeField] private float cavity = 0.5f;


	public float Cavity => cavity;

}