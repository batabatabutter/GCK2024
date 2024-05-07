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

	[Header("Maze")]

	[Header("–À˜H‚ÌŽí—Þ")]
	[SerializeField] private string strType;
	private MazeType type;
	[Header("’Ê˜H‚Ì•"), Min(1)]
	[SerializeField] private int pathWidth = 1;


	public MazeType Type => type;
	public int PathWidth => pathWidth;


	private void OnEnable()
	{
		type = SerializeUtil.Restore<MazeType>(strType);
	}

}