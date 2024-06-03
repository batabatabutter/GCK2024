using UnityEngine;



[CreateAssetMenu(fileName = "DungeonData_", menuName = "CreateDataBase/Dungeon/DungeonDataMaze")]
public class DungeonDataMaze : DungeonData
{
	public enum MazeType
	{
		Bar,		// �_�|���@
		Wall,		// �ǐL�΂��@
		Dig,		// ���@��@
	}

	[Header("---------- Maze ----------")]

	[Header("���H�̎��"), CustomEnum(typeof(MazeType))]
	[SerializeField] private string typeStr;
	private MazeType type;
	[Header("�ǂ̕�")]
	[SerializeField, Min(1)] private int wallWidth = 1;
	[SerializeField, Min(1)] private int wallHeight = 1;

	[Header("�ʘH�̕�")]
	[SerializeField, Min(1)] private int pathWidth = 1;
	[SerializeField, Min(1)] private int pathHeight = 1;


	public MazeType Type => type;
	public int WallWidth => wallWidth;
	public int WallHeight => wallHeight;
	public int PathWidth => pathWidth;
	public int PathHeight => pathHeight;


	private void OnEnable()
	{
		type = SerializeUtil.Restore<MazeType>(typeStr);
	}

}