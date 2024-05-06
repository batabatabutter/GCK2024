using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class TestDungeonGenerator : MonoBehaviour
{
	[Header("生成するダンジョンのデータ")]
	[SerializeField] private Object m_dungeonData = null;

	[Header("ダンジョンのサイズ")]
	[SerializeField] private Vector2Int m_dungeonSize;

	[System.Serializable]
	struct MapBlock
	{
		public string mapName;
		public BlockData.BlockType blockType;
	}

	[Header("生成ブロック")]
	[SerializeField] private MapBlock[] m_setBlocks;

	[Header("ブロックジェネレータ")]
	[SerializeField] private BlockGenerator m_blockGenerator;

	[Header("生成方式のインデックス")]
	[SerializeField] private int m_dungeonIndex = 0;
	[Header("生成スクリプトの配列")]
	[SerializeField] private DungeonGeneratorBase[] m_dungeonGenerators;

	[Header("コアの座標")]
	[SerializeField] private Vector2Int m_corePosition = Vector2Int.zero;


	[System.Serializable]
	public struct Blocks
	{
		public BlockData.BlockType type;
		public DungeonGenerator.BlockGenerateData data;
	}
	[SerializeField] private Blocks[] m_generateBlocks;
	private readonly Dictionary<BlockData.BlockType, DungeonGenerator.BlockGenerateData> m_blocks = new();

	[Header("ライト付ける")]
	[SerializeField] private bool m_light = true;

	[Header("プレイヤー(トランスフォーム用)")]
	[SerializeField] private GameObject m_player = null;

	private List<List<BlockData.BlockType>> m_blockTypes = new();


	// Start is called before the first frame update
	void Start()
	{
		// ブロックの設定
		for (int i = 0; i < m_generateBlocks.Length; i++)
		{
			Blocks block = m_generateBlocks[i];

			// 上書き防止
			if (m_blocks.ContainsKey(block.type))
				continue;

			// ブロックの種類設定
			m_blocks[block.type] = block.data;
		}

		// プレイヤーが設定されていれば生成
		if (m_player != null)
		{
			//Instantiate(m_player);
			m_blockGenerator.SetPlayerTransform(m_player.transform);
		}

		List<List<string>> mapList;

		// 生成
		if (m_dungeonGenerators.Length > 0)
		{
			mapList = m_dungeonGenerators[m_dungeonIndex].GenerateDungeon(m_dungeonSize);
		}
		else
		{
			// SCV読み込み
			mapList = GenerateSCV();
		}

		// マップの生成
		SetBlockType(mapList);
		Generate(m_blockTypes);

	}

	// ブロックの種類設定
	private void SetBlockType(List<List<string>> mapList)
	{
		List<List<BlockData.BlockType>> typeLists = new();

		for (int y = 0; y < mapList.Count; y++)
		{
			List<BlockData.BlockType> typeList = new();

			for (int x = 0; x < mapList[y].Count; x++)
			{
				string name = mapList[y][x];

				// ブロックナシ
				if (name == "0")
				{
					typeList.Add(BlockData.BlockType.OVER);
				}
				// ブロックアリ
				else if (name == "1")
				{
					// 生成するブロックの種類
					BlockData.BlockType type = BlockData.BlockType.STONE;
					// 生成ブロック種類分ループ
					foreach (DungeonGenerator.BlockGenerateData blocks in m_blocks.Values)
					{
						// ブロックを生成する
						if (GenerateBlock(new Vector2(x, y), blocks))
						{
							// 生成する場合は上書きしていく
							type = blocks.blockType;
						}
					}
					// 最終的な結果を生成ブロックとして追加
					typeList.Add(type);
				}
				// 確定鉱石
				else if (name == "2")
				{
					typeList.Add(CreateOre());
				}
				// 念のためその他
				else
				{
					typeList.Add(BlockData.BlockType.OVER);
				}
			}
			// 追加
			typeLists.Add(typeList);
		}

		m_blockTypes = typeLists;
	}

	// ダンジョンの生成
	private void Generate(List<List<BlockData.BlockType>> mapList)
	{
		// 読みだしたデータをもとにダンジョン生成をする
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				BlockData.BlockType name = mapList[y][x];

				// 生成座標
				Vector3 pos = new(x, y, 0.0f);

				// ブロックの生成
				m_blockGenerator.GenerateBlock(name, pos, null, m_light);

			}
		}

	}

	// CSV読み込みの生成
	private List<List<string>> GenerateSCV()
	{
		// ファイルがなければマップ読み込みの処理をしない
		if (!m_dungeonData)
			return new();

		// マップのリスト
		List<List<string>> mapList = new ();

		// 改行区切りで読み出す
		foreach (string line in m_dungeonData.ToString().Split("\n"))
		{
			// 行が存在しなければループを抜ける
			if (line == "")
				break;

			string lin = line.Remove(line.Length - 1);

			List<string> list = new ();

			// カンマ区切りで読み出す
			foreach (string line2 in lin.Split(","))
			{
				list.Add(line2);
			}

			mapList.Add(list);
		}

		return mapList;
	}

	// ブロックの情報生成
	private bool GenerateBlock(Vector2 pos, DungeonGenerator.BlockGenerateData data)
	{
		float dis = Vector2.Distance(m_corePosition, pos);

		// 生成範囲内
		if (data.range.Within(dis))
		{
			// ノイズの取得
			float noise = Mathf.PerlinNoise(pos.x * data.noiseScale, pos.y * data.noiseScale);
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


	// ブロック
	private BlockData.BlockType CreateBlock()
	{
		return BlockData.BlockType.STONE;
	}

	// 鉱石
	private BlockData.BlockType CreateOre()
	{
		int rand = Random.Range((int)BlockData.BlockType.ORE_BEGIN + 1, (int)BlockData.BlockType.ORE_END);

		return (BlockData.BlockType)rand;
	}

}
