using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class DungeonGenerator : MonoBehaviour
{
    //"ステージ(0～)
    private int m_stageNum;

    [Header("ダンジョンデータベース")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

	[Header("チャンクのサイズ")]
	[SerializeField] private int m_chunkSize = 10;
	[Header("表示チャンク数")]
	[SerializeField] private int m_activeChunk = 3;

	private GameObject[,] m_blocks = null;
	// ブロックのリスト
	private List<Block> m_blocksList = new();

	// チャンクの二次元配列
	private List<List<GameObject>> m_chunk = new();

	[System.Serializable]
    public class BlockOdds
    {
        [Header("種類")]
        public BlockData.BlockType type;       // 種類
        [Header("確率")]
        public int odds;       // 確率
    }
	[System.Serializable]
	public struct BlockGenerateData
	{
		[Header("生成するブロックの種類")]
		public BlockData.BlockType blockType;
		[Header("ブロックの生成範囲(コアからの距離)")]
		public MyFunction.MinMax range;
		[Header("ブロックの生成率")]
		[Range(0.0f, 1.0f)] public float rateMin;
		[Range(0.0f, 1.0f)] public float rateMax;
		[Header("ノイズのスケール"), Min(0.0f), Tooltip("値が大きいほど細かくなる")]
		public float noiseScale;
		// ノイズのオフセット
		public float offset;
	}

	[Header("核からプレイヤーの出現しない距離")]
    [SerializeField] private int m_playerLength = 35;
    [Header("プレイヤー")]
    [SerializeField] private GameObject m_player;
    [Header("プレイシーンマネージャー")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    [Header("地面背景")]
    [SerializeField] private GameObject m_ground;

    //コアの位置
    private Vector2Int m_corePos;
    //プレイヤーの位置
    private Vector2 m_playerPos;
    //ブロックの親
    private GameObject m_parent;

    //ブロック生成スクリプト
    private BlockGenerator m_blockGenerator;

	[System.Serializable]
	public struct Generator
	{
		public DungeonData.Pattern pattern;
		public DungeonGeneratorBase generator;
	}

	[Header("ダンジョン生成スクリプト")]
    [SerializeField] private Generator[] m_generators = null;
	private readonly Dictionary<DungeonData.Pattern, DungeonGeneratorBase> m_dungeonGenerators = new();


    private void Awake()
    {
		// ブロックジェネレータの取得
        m_blockGenerator = GetComponent<BlockGenerator>();
		// 親になるオブジェクトを生成
        m_parent = new GameObject("Blocks");
		// ダンジョン生成クラス
		foreach (Generator generator in m_generators)
		{
			// 上書き防止
			if (m_dungeonGenerators.ContainsKey(generator.pattern))
				continue;
			// ジェネレータの設定
			m_dungeonGenerators[generator.pattern] = generator.generator;
		}
    }

	private void Update()
	{
		// プレイヤーのいるチャンク
		Vector2Int playerChunk = new((int)m_player.transform.position.x / m_chunkSize, (int)m_player.transform.position.y / m_chunkSize);

		for (int y = 0; y < m_chunk.Count; y++)
		{
			for (int x = 0; x < m_chunk[y].Count; x++)
			{
				// プレイヤーチャンクとの距離
				float distance = Vector2Int.Distance(playerChunk, new Vector2Int(x, y));

				// 表示チャンク内
				if (distance < m_activeChunk)
				{
					if (m_chunk[y][x].activeSelf == false)
					{
						m_chunk[y][x].SetActive(true);
					}
				}
				// 表示チャンク外
				else
				{
					if (m_chunk[y][x].activeSelf == true)
					{
						m_chunk[y][x].SetActive(false);
					}
				}
			}
		}

	}

	/// <summary>
	/// ステージ作成
	/// </summary>
	public void CreateStage()
    {
        // ダンジョンのデータ取得
        DungeonData dungeonData = m_dungeonDataBase.dungeonDatas[m_stageNum];

        // 生成パターン取得
        DungeonData.Pattern pattern = dungeonData.DungeonPattern;

		// ダンジョンのマップ取得
		List<List<string>> mapList = m_dungeonGenerators[pattern].GenerateDungeon(dungeonData);

		// ダンジョンのサイズ
		Vector2Int dungeonSize = new(mapList[0].Count, mapList.Count);

		// コアの生成座標決定(無限ループにならないように回数制限をつける)
		for (int i = 0; i < 1000000; i++)
		{
			// コアの生成位置をランダムで取得
			Vector2Int pos = new(Random.Range(0, dungeonSize.x), Random.Range(0, dungeonSize.y));
			// 生成位置がブロックなら設定してループを抜ける
			if (mapList[pos.y][pos.x] == "1")
			{
				m_corePos = pos;
				break;
			}
		}

		int roop_error = 0;

		//プレイヤーとコアの位置が離れるまで繰り返す
		do
		{
			m_playerPos = new Vector2(Random.Range(0, dungeonSize.x), Random.Range(0, dungeonSize.y));

			if (roop_error > 100)
			{
				Debug.Log("コアとプレイヤーが近すぎます。間隔を見直してください");
				break;
			}
			roop_error++;

		}
		// コアに近い限りループ
		while (MyFunction.DetectCollision(m_playerPos, m_corePos, new Vector2(m_playerLength, m_playerLength))
		//m_playerPos.x < m_corePos.x + m_playerLength &&
		//m_playerPos.x > m_corePos.x - m_playerLength &&
		//m_playerPos.y < m_corePos.y + m_playerLength &&
		//m_playerPos.y > m_corePos.y - m_playerLength
		);

		//  プレイヤーの生成
		GameObject pl = Instantiate(m_player, m_playerPos, Quaternion.identity);
		m_blockGenerator.SetPlayerTransform(pl.transform);

		// coreの生成
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y), null);


		//  プレイシーンマネージャーが無かったら格納しない
		if (m_playSceneManager == null)
		{
			Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:DungeonManager");
		}
		else
		{
			m_playSceneManager.SetPlayer(pl);
			m_playSceneManager.SetCore(co);
		}

		// ブロック生成
		CreateBlock(mapList, dungeonData.BlockGenerateData);

		//岩盤で囲う
		CreateBedrock(dungeonSize);

		//地面の生成
		//CreateGround(dungeonSize);

    }




	private int LotteryBlock(List<BlockOdds> blockOddsList)
    {
        //確率の抽選
        List<int> oddsList = new ();

        //全ての確率合算
        int allOdds = 0;

        //ブロックの種類の数
        for (int i = 0; i < blockOddsList.Count; i++)
        {
            //ブロックの確率
            for (int j = 0; j < blockOddsList[i].odds; j++)
            {
                oddsList.Add(i);
            }
            //ブロックの確率を加算
            allOdds += blockOddsList[i].odds;
        }
        //抽選
        return oddsList[Random.Range(0, allOdds)];
    }

	// ブロックの情報生成
	private bool IsCreateBlock(Vector2 pos, BlockGenerateData data)
	{
		float dis = Vector2.Distance(m_corePos, pos);

		// 生成範囲内
		if (data.range.Within(dis))
		{
			// ノイズの取得
			float noise = MyFunction.GetNoise(pos * data.noiseScale, data.offset);
			// 生成範囲の中央値
			float center = (data.range.min + data.range.max) / 2.0f;
			// 生成範囲の中央からの距離
			float centerDis = Mathf.Abs(center - dis);
			// 生成の幅
			float wid = data.range.max - data.range.min;
			// ラープの値
			float t = 1.0f - (centerDis / (wid / 2.0f));
			// 生成率の取得
			float rate = Mathf.Lerp(data.rateMin, data.rateMax, t);

			// 鉱石
			if (noise < rate)
			{
				return true;
			}
			// 石
			else
			{
				return false;
			}
		}
		// 生成範囲外
		else
		{
			return false;
		}
	}

	// ブロックの生成
	private void CreateBlock(List<List<string>> mapList, BlockGenerateData[] blockGenerateData)
	{
		// ブロック生成用のランダムなオフセット設定
		for (int i = 0; i < blockGenerateData.Length; i++)
		{
			blockGenerateData[i].offset = Random.value;
		}

		// チャンクの生成
		for (int y = 0; y < mapList.Count / m_chunkSize; y++)
		{
			m_chunk.Add(new());
			for (int x = 0; x < mapList[y].Count / m_chunkSize; x++)
			{
				m_chunk[y].Add(new GameObject("(" + x + ", " + y + ")"));
				m_chunk[y][x].transform.parent = m_parent.transform;
			}
		}

		// 生成ブロック配列
		m_blocks = new GameObject[mapList.Count, mapList[0].Count];

		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				// チャンク取得
				Transform chunk = m_chunk[y / m_chunkSize][x / m_chunkSize].transform;

				// 生成する座標
				Vector2 position = new(x, y);
				//プレイヤー
				if ((int)m_playerPos.x == x && (int)m_playerPos.y == y)
				{
					// 空のブロックを生成
					m_blocks[y,x] = m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position, chunk);
					continue;
				}
				//コアを生成
				if (m_corePos.x == x && m_corePos.y == y)
				{
					continue;
				}
				// ブロックの生成位置
				if (mapList[y][x] == "1")
				{
					// 生成するブロックの種類
					BlockData.BlockType type = CreateBlockType(blockGenerateData, new Vector2(x, y));
					// ブロック生成
					m_blocks[y, x] = m_blockGenerator.GenerateBlock(type, position, chunk);
					// ブロックリストに追加
					m_blocksList.Add(m_blocks[y, x].GetComponent<Block>());
				}
				else
				{
					// 空のブロックを生成
					m_blocks[y, x] = m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position, chunk);
				}
			}
		}

	}

	// 生成するブロック
	private BlockData.BlockType CreateBlockType(BlockGenerateData[] blockGenerateData, Vector2 pos)
	{
		// 生成するブロックの種類
		BlockData.BlockType type = BlockData.BlockType.STONE;
		// 生成ブロック種類分ループ
		foreach (BlockGenerateData data in blockGenerateData)
		{
			// ブロックを生成する
			if (IsCreateBlock(pos, data))
			{
				// 生成する場合は上書きしていく
				type = data.blockType;
			}
		}
		// 最終的な生成ブロックの種類を返す
		return type;
	}

	// 岩盤の生成
	private void CreateBedrock(Vector2Int size)
	{
		//岩盤で囲う
		for (int x = 0; x < size.x; x++)
		{
			// 上下
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, -1    , 0), m_parent.transform);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, size.y, 0), m_parent.transform);
		}
		for (int y = 0; y < size.y; y++)
		{
			// 左右
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1    , y, 0), m_parent.transform);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(size.y, y, 0), m_parent.transform);
		}
	}

	// ステージ番号取得
	public int GetStageNum()
    {
        return m_stageNum;
    }
    //  ステージ番号設定
    public bool SetStageNum(int num)
    {
        //  ステージ番号が範囲外だったらエラー
        if (num < 0 || num >= m_dungeonDataBase.dungeonDatas.Count)
        {
            Debug.Log("ステージが存在しない番号です。num = " + num);
            m_stageNum = int.MaxValue;
            return false;
        }
        else
        {
            m_stageNum = num;
            return true;
        }
    }

	// ダンジョンデータ取得
	public DungeonData GetDungeonData()
	{
		return m_dungeonDataBase.dungeonDatas[m_stageNum];
	}
}
