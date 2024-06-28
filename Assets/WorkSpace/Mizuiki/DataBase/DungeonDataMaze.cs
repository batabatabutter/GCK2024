using UnityEngine;



[CreateAssetMenu(fileName = "DungeonData_", menuName = "CreateDataBase/Dungeon/DungeonDataMaze")]
public class DungeonDataMaze : DungeonData
{
	public enum MazeType
	{
		Bar,		// –_“|‚µ–@
		Wall,		// •ÇL‚Î‚µ–@
		Dig,		// ŒŠŒ@‚è–@
	}

	[Header("---------- Maze ----------")]

	[Header("–À˜H‚ÌŽí—Þ"), CustomEnum(typeof(MazeType))]
	[SerializeField] private string typeStr;
	private MazeType type;
	[Header("•Ç‚Ì•")]
	[SerializeField, Min(1)] private int wallWidth = 1;
	[SerializeField, Min(1)] private int wallHeight = 1;

	[Header("’Ê˜H‚Ì•")]
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