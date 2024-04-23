using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData_", menuName = "CreateDataBase/Dungeon/DungeonDataDig")]
public class DungeonDataDigging : DungeonData
{
	[Header("個別")]

	[Header("部屋のサイズの最小値と最大値")]
	[SerializeField] private MyFunction.MinMax roomRange;
	[Header("道の長さの範囲")]
	[SerializeField] private MyFunction.MinMax pathRange;
	[Header("生成の最大数"), Min(0)]
	[SerializeField] private int maxCount = 100;
	[Header("部屋の生成率")]
	[SerializeField, Range(0.0f, 1.0f)] private float roomGenerateRate = 0.5f;
	[Header("通路が埋まっている割合")]
	[SerializeField, Range(0.0f, 1.0f)] private float pathFillRate = 0.5f;

	public MyFunction.MinMax RoomRange => roomRange;
	public MyFunction.MinMax PathRange => pathRange;
	public int MaxCount => maxCount;
	public float RoomGenerateRate => roomGenerateRate;
	public float PathFillRate => pathFillRate;

}