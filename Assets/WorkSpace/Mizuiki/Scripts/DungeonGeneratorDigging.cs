using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 参考 : https://qiita.com/gis/items/253cb6af577ec11e79cf
/// </summary>
public class DungeonGeneratorDigging : DungeonGeneratorBase
{
    // 方向
    enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,

        OVER,
    }
    // タイル
    enum Tile
    {
        BLOCK,
        ROOM,
        PATH,
        WALL,

        OVER,
    }

    // 部屋(通路にも使用)
    struct Room
    {
        public Vector2Int pos;    // 部屋の座標
        public Vector2Int size;   // 部屋のサイズ

        public Tile tile;
    }
    [Header("部屋のサイズの最小値と最大値")]
    [SerializeField] private MyFunction.MinMax m_roomRange;

    [Header("通路の長さの範囲")]
    [SerializeField] private MyFunction.MinMax m_pathRange;

    [Header("生成の最大数"), Min(0)]
    [SerializeField] private int m_maxCount = 100;

    [Header("部屋の生成率")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_roomGenerateRate = 0.5f;

    [Header("通路が埋まっている割合")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_pathFillRate = 0.5f;

    [Header("部屋に鉱石の塊ができる確率")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_roomOreChunkRate = 0.0f;
    [Header("部屋の鉱石生成数")]
    [SerializeField] private MyFunction.MinMax m_roomOreChunkCount;

	// タイルマップ
	readonly List<List<Tile>> m_tiles = new();
    readonly List<List<string>> m_mapList = new();

    // 部屋の配列
    private readonly List<Room> m_rooms = new();
    // 通路の生成可能位置
    private readonly List<Room> m_root = new();

    // マップのサイズ
    private Vector2Int m_mapSize;

    // ダンジョンデータの設定
    public void SetDungeonData(DungeonDataDigging data)
    {
		// 部屋のサイズ
		m_roomRange = data.RoomRange;
		// 通路の長さ
		m_pathRange = data.PathRange;
		// 生成の最大数
		m_maxCount = data.MaxCount;
		// 部屋の生成率
		m_roomGenerateRate = data.RoomGenerateRate;
		// 通路の埋め立て
		m_pathFillRate = data.PathFillRate;

	}

	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
    {
		// マップのサイズ取得
		m_mapSize = dungeonData.Size;

		// タイルマップ初期化
		for (int y = 0; y < m_mapSize.y; y++)
        {
            List<Tile> tiles = new();
            for (int x = 0; x < m_mapSize.x; x++)
            {
                // 列の追加
                tiles.Add(Tile.BLOCK);
            }
            // 行の追加
            m_tiles.Add(tiles);
        }

        // データのキャスト
		DungeonDataDigging data = dungeonData as DungeonDataDigging;

        // データの設定
        if (data)
        {
            SetDungeonData(data);
        }

        // 最初の部屋の座標
        Vector2Int firstRoomPos = new(Random.Range(0, m_mapSize.x - m_roomRange.max), Random.Range(0, m_mapSize.y - m_roomRange.max));

        // 最初の部屋を作る
        CreateRoom(firstRoomPos, Direction.OVER);

        // 繋がる部屋を作る
        for (int i = 0; i < m_maxCount; i++)
        {
            // これ以上作れない
            if (!CreateSpace())
            {
                Debug.Log("これ以上作れないよ -> " + i.ToString() + " 回生成");
                break;
            }
        }

        // 生成したタイルマップをもとにブロックの配置を行う
        foreach (List<Tile> tiles in m_tiles)
        {
            List<string> list = new();
            foreach (Tile tile in tiles)
            {
                if (tile == Tile.BLOCK)
                {
                    list.Add("1");
                }
                else if (tile == Tile.WALL)
                {
                    list.Add("1");
                }
                else if (tile == Tile.PATH)
                {
                    // 通路が埋まっている確率
                    float rand = Random.Range(0.0f, 1.0f);
                    if (rand <= m_pathFillRate)
                    {
                        // 通路の埋め立て
                        list.Add("1");
					}
                    else
                    {
                        list.Add("0");
                    }
				}
                else
                {
                    list.Add("0");
                }
            }
            m_mapList.Add(list);
        }

        // 鉱石生成
        CreateOre();

        return m_mapList;
    }


    // 部屋の作成
    private bool CreateRoom(Vector2Int position, Direction direction)
    {
        // 部屋の作成
        Room room = new()
        {
            pos = position,
			size = new Vector2Int(Random.Range(m_roomRange.min, m_roomRange.max), Random.Range(m_roomRange.min, m_roomRange.max)),
            tile = Tile.ROOM,
		};

        // 部屋の飛び出す方向
        switch (direction)
        {
            case Direction.UP:
                room.pos.x -= room.size.x / 2;
                room.pos.y += 1;
                break;

            case Direction.DOWN:
                room.pos.x -= room.size.x / 2;
                room.pos.y -= room.size.y;
                break;

            case Direction.LEFT:
                room.pos.x -= room.size.x;
                room.pos.y -= room.size.y / 2;
                break;

            case Direction.RIGHT:
                room.pos.x += 1;
                room.pos.y -= room.size.y / 2;
                break;
        }

        // ほかの部屋や通路にかぶっていないか
        if (CheckCreate(room))
        {
			// 部屋の追加
			m_rooms.Add(room);

            // 部屋の周りに通路と部屋を生成可能にする
            SetWall(room, direction, Tile.ROOM);
            // 部屋の作成完了
            return true;
        }
        // 部屋が作られなかった
        return false;
	}

    // 道の作成
    private bool CreatePath(Vector2Int pos, Direction direction)
    {
        Room path = new()
        {
            pos = pos,
            tile = Tile.PATH,
        };

        // 左右に伸ばす
        if (Random.Range(0, 2) == 0)
        {
            // 伸ばす長さ
            path.size.x = Random.Range(m_pathRange.min, m_pathRange.max);
            path.size.y = 1;
            // 伸ばす方向
            switch (direction)
            {
                case Direction.UP:
                    // 確率で左にずらす
                    if (Random.Range(0, 2) == 0)
                    {
                        path.pos.x -= path.size.x - 1;
                    }
                    // 上に伸ばす
                    path.pos.y += 1;
                    break;

                case Direction.DOWN:
					// 確率で左にずらす
					if (Random.Range(0, 2) == 0)
					{
						path.pos.x -= path.size.x - 1;
					}
                    // 下に伸ばす
                    path.pos.y -= 1;
					break;

                case Direction.LEFT:
                    // 左に伸ばす
                    path.pos.x -= path.size.x;
                    break;

                case Direction.RIGHT:
                    // 右に伸ばす
                    path.pos.x += 1;
                    break;
            }

        }
        // 上下に伸ばす
        else
        {
            // 伸ばす長さ
            path.size.x = 1;
            path.size.y = Random.Range(m_pathRange.min, m_pathRange.max);
            // 伸ばす方向
            switch (direction)
            {
                case Direction.UP:
                    // 上に伸ばす
                    path.pos.y += 1;
                    break;

                case Direction.DOWN:
                    // 下に伸ばす
                    path.pos.y -= path.size.y;
                    break;

                case Direction.LEFT:
                    // 左に伸ばす
                    path.pos.x -= 1;
                    // 確率で上にずらす
                    if (Random.Range(0, 2) == 0)
                    {
                        path.pos.y -= path.size.y - 1;
                    }
                    break;

                case Direction.RIGHT:
                    // 右に伸ばす
                    path.pos.x += 1;
                    // 確率で上にずらす
                    if (Random.Range(0, 2) == 0)
                    {
                        path.pos.y -= path.size.y - 1;
                    }
                    break;
            }
        }
		// ほかの部屋や通路にかぶっていないか
		if (CheckCreate(path))
		{
			// 通路の周りに通路と部屋を生成可能にする
			SetWall(path, direction, Tile.PATH);
			// 通路の作成完了
			return true;
		}
		// 通路が作られなかった
		return false;

	}

	// 周辺を生成可能な壁にする
	private void SetWall(Room room, Direction direction, Tile tile)
    {
		// 部屋の周りに通路と部屋を生成可能にする
		if (direction != Direction.UP)
		{
			// 部屋の上に通路を作成可能
			Room root = new()
			{
				pos = new(room.pos.x, room.pos.y + room.size.y),
				size = new(room.size.x, 1),
				tile = tile,
			};
			// 付け根に追加
			m_root.Add(root);
		}
		if (direction != Direction.DOWN)
		{
			// 部屋の上に通路を作成可能
			Room root = new()
			{
				pos = new(room.pos.x, room.pos.y - 1),
				size = new(room.size.x, 1),
				tile = tile,
			};
			// 付け根に追加
			m_root.Add(root);
		}
		if (direction != Direction.LEFT)
		{
			// 部屋の左に通路を作成可能
			Room root = new()
			{
				pos = new(room.pos.x - 1, room.pos.y),
				size = new(1, room.size.y),
				tile = tile,
			};
			// 付け根に追加
			m_root.Add(root);
		}
		if (direction != Direction.RIGHT)
		{
			// 部屋の右に通路を作成可能
			Room root = new()
			{
				pos = new(room.pos.x + room.size.x, room.pos.y),
				size = new(1, room.size.y),
				tile = tile,
			};
			// 付け根に追加
			m_root.Add(root);
		}

	}


	// ダンジョンを広げる
	private bool CreateSpace()
    {
        // 無限ループ防止用のカウンタ
        int count = 0;
        while (true)
        {
            // 付け根がない
            if (m_root.Count <= 0)
                return false;

            // 伸ばす付け根決定
            int index = Random.Range(0, m_root.Count);
            // 伸ばし始める位置決定
            Vector2Int pos = new()
            {
                x = Random.Range(m_root[index].pos.x, m_root[index].pos.x + m_root[index].size.x - 1),
                y = Random.Range(m_root[index].pos.y, m_root[index].pos.y + m_root[index].size.y - 1),
            };
            // 伸ばす方向
            for (Direction dir = Direction.UP; dir < Direction.OVER; dir++)
            {
                // 今の方向には伸ばせない
                if (!CreateSpace(pos, dir))
                {
                    continue;
                }
                // 付け根から削除
                m_root.RemoveAt(index);
                // ダンジョンが広がった
                return true;
            }

            count++;
            if (count >= 1000000)
                return false;
        }
    }
    private bool CreateSpace(Vector2Int position, Direction direction)
    {
        // 一マス戻った座標
        Vector2Int backPos = position;
        switch (direction)
        {
            case Direction.UP:      // 下に伸ばす
                backPos.y -= 1;
                break;

            case Direction.DOWN:    // 上に伸ばす
                backPos.y += 1;
                break;

            case Direction.LEFT:    // 右に伸ばす
                backPos.x += 1;
                break;

            case Direction.RIGHT:   // 左に伸ばす
                backPos.x -= 1;
                break;
        }
        // 戻った先が範囲外
        if (backPos.x < 0 ||              // 左
            backPos.y < 0 ||              // 下
            backPos.x >= m_mapSize.x ||   // 右
            backPos.y >= m_mapSize.y)     // 上
        {
            // 伸ばせないよ
            return false;
        }
        // 反対側が部屋か通路以外
        if (!(m_tiles[backPos.y][backPos.x] == Tile.ROOM || m_tiles[backPos.y][backPos.x] == Tile.PATH))
        {
            return false;
        }

        // 部屋の生成確率
        float rand = Random.Range(0.0f, 1.0f);

        // 確率で部屋か通路
        if (rand <= m_roomGenerateRate)
        {
            // 部屋を作れなかった
            if (!CreateRoom(position, direction))
            {
                return false;
            }
            // 引数の位置を通路にする
            m_tiles[position.y][position.x] = Tile.PATH;
            // ダンジョンが広がった
            return true;
        }
        else
        {
            // 道を作れなかった
            if (!CreatePath(position, direction))
            {
                return false;
            }
            // 引数の位置を通路にする
            m_tiles[position.y][position.x] = Tile.PATH;
            // ダンジョンが広がった
            return true;
        }
    }

    // 部屋を作成可能かチェック
    private bool CheckCreate(Room room)
    {
        // マップ範囲外
        if (room.pos.x <= 0 ||  // 左
			room.pos.y <= 0 ||  // 下
            room.pos.x + room.size.x >= m_mapSize.x ||  // 右
            room.pos.y + room.size.y >= m_mapSize.y)    // 上
        {
            return false;
        }
        // 作成予定地がすべてブロックか確認
        for (int y = room.pos.y; y < room.pos.y + room.size.y; y++)
        {
            for (int x = room.pos.x; x < room.pos.x + room.size.x; x++)
            {
				// ブロックじゃないタイルがある
				if (m_tiles[y][x] != Tile.BLOCK)
                {
                    // 部屋が作れない
                    return false;
                }
            }
        }
        // 部屋の周りを壁にする
        for (int y = room.pos.y - 1; y < room.pos.y + room.size.y + 1; y++)
        {
            for (int x = room.pos.x - 1; x < room.pos.x + room.size.x + 1; x++)
            {
                // 部屋の外周
                if (x < room.pos.x ||   // 左
                    y < room.pos.y ||   // 下
                    x >= room.pos.x + room.size.x ||    // 右
                    y >= room.pos.y + room.size.y)      // 上
                {
                    // 壁タイルにする
                    m_tiles[y][x] = Tile.WALL;
                }
                else
                {
                    // 指定タイルにする(ROOM or PATH)
                    m_tiles[y][x] = room.tile;
                }
            }
        }
        // 部屋を作れる
        return true;
    }

    // 鉱石の生成
    private void CreateOre()
    {
        // まずは生成する部屋を決める
		foreach (Room room in m_rooms)
		{
            // 乱数取得
            float rand = Random.Range(0.0f, 1.0f);

            // 鉱石を生成しない
            if (rand > m_roomOreChunkRate)
                continue;

            // 生成数の決定
            int count = Random.Range(m_roomOreChunkCount.min, m_roomOreChunkCount.max + 1);
            // 鉱石の塊を生成
            for (int i = 0; i < count; i++)
            {
                // 生成位置決定
                Vector2Int pos = new(Random.Range(room.pos.x, room.pos.x + room.size.x), Random.Range(room.pos.y, room.pos.y + room.size.y));

                CreateOre(pos);
            }
		}
	}
    private void CreateOre(Vector2Int pos)
    {


        m_mapList[pos.y][pos.x] = "2";
        
    }

}
