using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DungeonGeneratorMaze : DungeonGeneratorBase
{
	[Header("生成の種類")]
	[SerializeField] private DungeonDataMaze.MazeType m_mazeType;

	[Header("通路の幅")]
	[SerializeField, Min(1)] private int m_pathWidth = 1;
	[SerializeField, Min(1)] private int m_pathHeight = 1;

	// マップのサイズ
	private Vector2Int m_mapSize;

	// 迷路行列
	readonly List<List<string>> m_mazeList = new();
	// マップリスト
	readonly List<List<string>> m_mapList = new();



	// データをもとに生成
	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
	{
		// データを変換
		DungeonDataMaze data = dungeonData as DungeonDataMaze;
		// データがあれば設定する
		if (data)
		{
			SetDungeonData(data);
		}
		// サイズをもとにダンジョン生成
		return GenerateDungeon(dungeonData.Size);
	}

	// サイズをもとに生成
	public override List<List<string>> GenerateDungeon(Vector2Int size)
	{
		// タイルマップ初期化
		for (int y = 0; y < size.y; y++)
		{
			List<string> list = new();
			for (int x = 0; x < size.x; x++)
			{
				// 列の追加
				list.Add("1");
			}
			// 行の追加
			m_mapList.Add(list);
		}

		// マップのサイズ取得
		m_mapSize.x = size.x / m_pathWidth;
		m_mapSize.y = size.y / m_pathHeight;

		// 偶数サイズの場合は奇数にする
		if (m_mapSize.x % 2 == 0)
		{
			m_mapSize.x--;
		}
		if (m_mapSize.y % 2 == 0)
		{
			m_mapSize.y--;
		}

		// サイズが5未満の場合は返す
		if (m_mapSize.x < 5 || m_mapSize.y < 5)
			return m_mapList;

		// 生成法則ごと
		switch (m_mazeType)
		{
			case DungeonDataMaze.MazeType.Bar:
				// 棒倒し法
				CreateBar();
				break;

			case DungeonDataMaze.MazeType.Wall:
				// 壁伸ばし法
				CreateWall();
				break;

			case DungeonDataMaze.MazeType.Dig:
				// 穴掘り法
				CreateDig();
				break;
		}

		// 迷路の情報をもとにマップを作る
		CreateMazeMap();

		// 生成したマップ情報を返す
		return m_mapList;
	}

	// ダンジョンデータの設定
	private void SetDungeonData(DungeonDataMaze dungeonData)
	{
		// 生成タイプ
		m_mazeType = dungeonData.Type;
		// 通路の幅
		m_pathWidth = dungeonData.PathWidth;
	}

	// 棒倒し法
	private void CreateBar()
	{
		// 外周を壁にする
		for (int y = 0; y < m_mapSize.y; y++)
		{
			List<string> maze = new();
			for (int x = 0; x < m_mapSize.x; x++)
			{
				// 外周
				if (x == 0 || y == 0 ||
					x == m_mapSize.x - 1 || y == m_mapSize.y - 1)
				{
					maze.Add("1");
				}
				// 内側
				else
				{
					maze.Add("0");
				}
			}
			m_mazeList.Add(maze);
		}

		// 点々の壁を作ってそこから伸ばす
		for (int y = 2; y < m_mapSize.y - 1; y += 2)
		{
			for (int x = 2; x < m_mapSize.x - 1; x += 2)
			{
				// 棒(壁)を立てる
				m_mazeList[y][x] = "1";

				// 念のための無限ループ回避用
				int count = 0;

				// 倒せるまで繰り返す
				while (true)
				{
					// 1行目の場合は上にも倒せる
					int direction;
					if (y == 2)
					{
						direction = Random.Range(0, 4);
					}
					else
					{
						direction = Random.Range(0, 3);
					}

					// 棒を倒す方向
					Vector2Int bar = new (x, y);
					switch (direction)
					{
						case 0:
							// 上
							bar.y++;
							break;

						case 1:
							// 右
							bar.x++;
							break;

						case 2:
							// 下
							bar.y--;
							break;

						case 3:
							// 左
							bar.x--;
							break;
					}

					// 倒す方向が壁じゃない
					if (m_mazeList[bar.y][bar.x] != "1")
					{
						// 指定方向に倒して抜ける
						m_mazeList[bar.y][bar.x] = "1";
						break;
					}

					// 100万回ループしたら強制終了
					count++;
					if (count > 1000000)
						break;
				}
			}
		}
	}

	// 壁伸ばし法
	private void CreateWall()
	{

	}

	// 穴掘り法
	private void CreateDig()
	{

	}

	// 迷路をもとにマップを作る
	private void CreateMazeMap()
	{
		int height = 0;

		for (int y = 0; y < m_mapList.Count; y++)
		{
			if (y % m_pathHeight == 0 && y != 0)
			{
				height++;
			}
			if (height >= m_mapSize.y)
			{
				break;
			}


			int width = 0;

			for (int x = 0; x < m_mapList[y].Count; x++)
			{
				if (x % m_pathWidth == 0 && x != 0)
				{
					width++;
				}
				if (width >= m_mapSize.x)
				{
					break;
				}

				m_mapList[y][x] = m_mazeList[height][width];
			}
		}
	}

}
