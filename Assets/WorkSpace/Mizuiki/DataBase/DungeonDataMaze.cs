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

	[Header("Maze")]

	[Header("���H�̎��")]
	[SerializeField] private string strType;
	private MazeType type;
	[Header("�ʘH�̕�"), Min(1)]
	[SerializeField] private int pathWidth = 1;


	public MazeType Type => type;
	public int PathWidth => pathWidth;


	private void OnEnable()
	{
		type = SerializeUtil.Restore<MazeType>(strType);
	}

}