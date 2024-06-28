using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DungeonGeneratorMaze : DungeonGeneratorBase
{
	[Header("生成の種類")]
	[SerializeField] private DungeonDataMaze.MazeType m_mazeType;

	[Header("壁の幅")]
	[SerializeField, Min(1)] private int m_wallWidth = 1;
	[SerializeField, Min(1)] private int m_wallHeight = 1;

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
		m_mapSize.x = size.x / ((m_pathWidth + m_wallWidth) / 2);
		m_mapSize.y = size.y / ((m_pathHeight + m_wallHeight) / 2);

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
		// 壁の幅
		m_wallWidth = dungeonData.WallWidth;
		m_wallHeight = dungeonData.WallHeight;
		// 通路の幅
		m_pathWidth = dungeonData.PathWidth;
		m_pathHeight = dungeonData.PathHeight;
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
		// 壁を伸ばす候補地点
		List<Vector2Int> startPos = new();

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
					// 壁を伸ばす候補地点追加
					if (x % 2 == 0 && y % 2 == 0)
					{
						startPos.Add(new(x, y));
					}
				}
			}
			m_mazeList.Add(maze);
		}

		// 念のための無限ループ回避用
		int count = 0;
		// 壁を伸ばす候補地点がなくなるまでループ
		while (startPos.Count > 0)
		{
			// ランダムなインデックスを取得
			int index = Random.Range(0, startPos.Count);
			// 開始地点を取得
			Vector2Int start = startPos[index];
			// 候補地点から削除
			startPos.RemoveAt(index);

			// すでに壁になっている
			if (m_mazeList[start.y][start.x] == "1")
				continue;

			// 拡張中の壁の情報
			Stack<Vector2Int> extendWall = new();

			// 壁を伸ばしていく
			ExtendWall(start, extendWall);

			count++;
			if (count > 1000000)
				break;
		}
;

	}
	// 壁を伸ばす
	private void ExtendWall(Vector2Int start, Stack<Vector2Int> extendWall)
	{
		List<MyFunction.Direction> direction = new();
		// 上
		if (m_mazeList[start.y + 1][start.x] == "0" &&          // 一マス上が通路
			!extendWall.Contains(start + new Vector2Int(0, 2)))  // 二マス上が現在伸ばしている壁ではない
		{
			direction.Add(MyFunction.Direction.UP);
		}
		// 右
		if (m_mazeList[start.y][start.x + 1] == "0" &&          // 一マス右が通路
			!extendWall.Contains(start + new Vector2Int(2, 0))) // 二マス右が現在伸ばしている壁ではない
		{
			direction.Add(MyFunction.Direction.RIGHT);
		}
		// 下
		if (m_mazeList[start.y - 1][start.x] == "0" &&          // 一マス下が通路
			!extendWall.Contains(start + new Vector2Int(0, -2)))// 二マス下が現在伸ばしている壁ではない
		{
			direction.Add(MyFunction.Direction.DOWN);
		}
		// 左
		if (m_mazeList[start.y][start.x - 1] == "0" &&          // 一マス左が通路
			!extendWall.Contains(start + new Vector2Int(-2, 0)))// 二マス左が現在伸ばしている壁ではない
		{
			direction.Add(MyFunction.Direction.LEFT);
		}

		// 四方向のうちどれかには伸ばせる
		if (direction.Count > 0)
		{
			// 現在地点を壁にする
			m_mazeList[start.y][start.x] = "1";
			// 拡張中の壁に追加
			extendWall.Push(start);

			// 伸ばした先が通路
			bool path = false;
			// ランダムに伸ばす方向を決める
			int dir = Random.Range(0, direction.Count);
			// 伸ばす
			switch (direction[dir])
			{
				case MyFunction.Direction.UP:		// 上
					// 二マス上が通路
					path = m_mazeList[start.y + 2][start.x] == "0";
					// 一マス上を壁にする
					m_mazeList[++start.y][start.x] = "1";
					// 二マス上を壁にする
					m_mazeList[++start.y][start.x] = "1";
					extendWall.Push(start);
					break;

				case MyFunction.Direction.RIGHT:	// 右
					// 二マス右が通路
					path = m_mazeList[start.y][start.x + 2] == "0";
					// 一マス右を壁にする
					m_mazeList[start.y][++start.x] = "1";
					// 二マス右を壁にする
					m_mazeList[start.y][++start.x] = "1";
					extendWall.Push(start);
					break;

				case MyFunction.Direction.DOWN:     // 下
					// 二マス下が通路
					path = m_mazeList[start.y - 2][start.x] == "0";
					// 一マス下を壁にする
					m_mazeList[--start.y][start.x] = "1";
					// 二マス下を壁にする
					m_mazeList[--start.y][start.x] = "1";
					extendWall.Push(start);
					break;

				case MyFunction.Direction.LEFT:		// 左
					// 二マス左が通路
					path = m_mazeList[start.y][start.x - 2] == "0";
					// 一マス左を壁にする
					m_mazeList[start.y][--start.x] = "1";
					// 二マス左を壁にする
					m_mazeList[start.y][--start.x] = "1";
					extendWall.Push(start);
					break;
			}
			// 伸ばした先が通路
			if (path)
			{
				// さらに壁を伸ばす
				ExtendWall(start, extendWall);
			}
		}
		// どこにも伸ばせない
		else
		{
			// 戻って伸ばす処理を再開する
			Vector2Int back = extendWall.Pop();
			ExtendWall(back, extendWall);
		}
	}

	// 穴掘り法
	private void CreateDig()
	{

	}

	// 迷路をもとにマップを作る
	private void CreateMazeMap()
	{
		// 生成ブロック数
		Vector2Int count = Vector2Int.zero;
		// 端っこの壁の幅
		Vector2Int edgeWall = new()
		{
			x = m_mapList[0].Count / m_mazeList[0].Count,
			y = m_mapList.Count / m_mazeList.Count,
		};

		for (int y = 0; y < m_mazeList.Count; y++)
		{
			int height;
			// y が偶数の場合は壁の幅
			if (y % 2 == 0)
			{
				height = m_wallHeight;
			}
			// 偶数の場合は通路の幅
			else
			{
				height = m_pathHeight;
			}

			// 端っこ
			if (y == 0)
			{
				height = edgeWall.y;
			}

			// x のカウント初期化
			count.x = 0;

			for (int x = 0; x < m_mazeList[y].Count; x++)
			{
				int width;
				// x が偶数の場合は壁の幅で生成
				if (x % 2 == 0)
				{
					width = m_wallWidth;
				}
				// 奇数の場合は通路の幅で生成
				else
				{
					width = m_pathHeight;
				}

				// 端っこ
				if (x == 0)
				{
					width = edgeWall.x;
				}

				// ブロックの情報生成
				CreateBlocks(count, width, height, m_mazeList[y][x]);
				// カウント加算
				count.x += width;
			}
			// カウント加算
			count.y += height;
		}


		//int height = 0;
		//// マップの行
		//for (int y = 0; y < m_mapList.Count; y++)
		//{
		//	// 最初の行ははじいて、通路幅の倍数のときに高さを加算
		//	if (y % m_pathHeight == 0 && y != 0)
		//	{
		//		height++;
		//	}
		//	// 迷路のサイズ外になったらループを終了する
		//	if (height >= m_mapSize.y)
		//	{
		//		break;
		//	}
		//	// 幅の宣言
		//	int width = 0;
		//	// マップの列
		//	for (int x = 0; x < m_mapList[y].Count; x++)
		//	{
		//		// 最初の列をはじいて、通路の幅の倍数のときに幅を加算
		//		if (x % m_pathWidth == 0 && x != 0)
		//		{
		//			width++;
		//		}
		//		// 迷路のサイズ外になったらループを抜ける
		//		if (width >= m_mapSize.x)
		//		{
		//			break;
		//		}
		//		// マップに迷路の情報を代入
		//		m_mapList[y][x] = m_mazeList[height][width];
		//	}
		//}
	}

	// 迷路の一グリッド分のブロックを生成する
	private void CreateBlocks(Vector2Int start, int width, int height, string maze)
	{
		for (int y = 0; y < height; y++)
		{
			// 配列範囲外
			if (start.y + y >= m_mapList.Count)
				break;

			for (int x = 0; x < width; x++)
			{
				// 配列範囲外
				if (start.x + x >= m_mapList[start.y + y].Count)
					break;

				// 迷路の情報を入れる
				m_mapList[start.y + y][start.x + x] = maze;
			}
		}
	}

}
