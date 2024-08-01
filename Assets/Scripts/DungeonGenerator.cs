using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
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

    // ステージ番号(0～)
    private int m_stageNum;

    [Header("ダンジョンデータベース")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

	[Header("ブロック生成スクリプト")]
    [SerializeField] private BlockGenerator m_blockGenerator;
	[Header("核からプレイヤーの出現しない距離")]
    [SerializeField] private int m_playerLength = 35;
    [Header("プレイヤー")]
    [SerializeField] private GameObject m_playerPrefab;
    //[Header("プレイシーンマネージャー")]
    //[SerializeField] private PlaySceneManager m_playSceneManager;

    [Header("地面")]
    [SerializeField] private GameObject m_ground;

    //コアの位置
    private Vector2Int m_corePos;
    //プレイヤーの位置
    private Vector2 m_playerPos;

	// コア
	private Block m_dungeonCore;

	// ブロックの配列
	private Block[,] m_blocks;


	[Header("ダンジョン生成スクリプト")]
    [SerializeField] private List<DungeonGeneratorBase> m_generators = null;
	private readonly Dictionary<DungeonData.Pattern, DungeonGeneratorBase> m_dungeonGenerators = new();




	/// <summary>
	/// ステージ作成
	/// </summary>
	public void CreateStage(int stageNum)
    {
        // ブロックジェネレータの取得
        m_blockGenerator = GetComponent<BlockGenerator>();

		if (m_dungeonGenerators.Count == 0)
		{
			// ダンジョン生成クラス
			foreach (DungeonGeneratorBase generator in m_generators)
			{
				// 上書き防止
				if (m_dungeonGenerators.ContainsKey(generator.Pattern))
					continue;
				// ジェネレータの設定
				m_dungeonGenerators[generator.Pattern] = generator;
			}
		}

        // ステージ番号の設定
        m_stageNum = stageNum;

        // ダンジョンのデータ取得
        DungeonData dungeonData = m_dungeonDataBase.dungeonDatas[m_stageNum];

        // 生成パターン取得
        DungeonData.Pattern pattern = dungeonData.DungeonPattern;

		// ダンジョンのサイズ
		Vector2Int dungeonSize = new(dungeonData.Size.x, dungeonData.Size.y);
		// ブロック配列のサイズ決定
		m_blocks = new Block[dungeonSize.y, dungeonSize.x];

		// セーブデータ取得
		SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;

		// セーブデータが存在している
		if (saveData)
		{
			// ダンジョンのレベル取得
			int dungeonLevel = saveData.DungeonStates[stageNum].clearCount[0];
		}

		// ダンジョンのマップ取得
		List<List<string>> mapList = m_dungeonGenerators[pattern].GenerateDungeon(dungeonData);

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
		);

		// coreの生成
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y));
		// スプライトの設定
		co.GetComponent<SpriteRenderer>().sprite = dungeonData.CoreSprite;
		// コア設定
		m_dungeonCore = co.GetComponent<Block>();
		// ブロック配列に代入
		m_blocks[m_corePos.y, m_corePos.x] = m_dungeonCore;

		// ブロック生成
		GenerateBlock(mapList, dungeonData.BlockGenerateData);

		//岩盤で囲う
		CreateBedrock(dungeonSize);

	}

	// プレイヤーのトランスフォーム設定
	public void SetPlayerTransform(Transform player)
	{
		m_blockGenerator.SetPlayerTransform(player);
	}

	//public PlaySceneManager PlaySceneManager
	//{
	//	set { m_playSceneManager = value; }
	//}

	// コア
	public Block DungeonCore
	{
		get { return m_dungeonCore; }
	}
	// プレイヤーの位置
	public Vector3 PlayerPosition
	{
		get { return m_playerPos; }
	}
	// ブロック
	public Block[,] Blocks
	{
		get { return m_blocks; }
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
	private void GenerateBlock(List<List<string>> mapList, BlockGenerateData[] blockGenerateData)
	{
		// ブロック生成用のランダムなオフセット設定
		for (int i = 0; i < blockGenerateData.Length; i++)
		{
			blockGenerateData[i].offset = Random.value;
		}

		// ブロック生成
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				// 生成する座標
				Vector2 position = new(x, y);
				// 生成したブロック
				Block block = null;
				//プレイヤー
				if ((int)m_playerPos.x == x && (int)m_playerPos.y == y)
				{
					// 空のブロックを生成
					m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position);
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
					block = m_blockGenerator.GenerateBlock(type, position).GetComponent<Block>();
				}
				else
				{
					// 空のブロックを生成
					m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position);
				}
				// ブロック配列に代入
				m_blocks[y, x] = block;
			}
		}
	}
	public void GenerateBlock(List<List<BlockData.BlockType>> blocks)
	{
		for (int y = 0; y < blocks.Count; y++)
		{
			for (int x = 0; x < blocks[y].Count; x++)
			{
				// 種類
				BlockData.BlockType type = blocks[y][x];
				// 位置
				Vector2 pos = new(x, y);

				// 生成ブロック
				GameObject block = m_blockGenerator.GenerateBlock(type, pos);
				m_blocks[y, x] = block.GetComponent<Block>();
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
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, -1    , 0));
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, size.y, 0));
		}
		for (int y = 0; y < size.y; y++)
		{
			// 左右
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1    , y, 0));
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(size.y, y, 0));
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
