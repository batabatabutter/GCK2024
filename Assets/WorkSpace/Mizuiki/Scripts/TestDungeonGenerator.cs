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

	[Header("ブロックジェネレータ")]
	[SerializeField] private BlockGenerator m_blockGenerator;

	[Header("生成方式のインデックス")]
	[SerializeField] private int m_dungeonIndex = 0;
	[Header("生成スクリプトの配列")]
	[SerializeField] private DungeonGeneratorBase[] m_dungeonGenerators;

	[Header("コアの座標")]
	[SerializeField] private Vector2Int m_corePosition = Vector2Int.zero;

	[Header("各ブロックの生成情報")]
	[SerializeField] private DungeonGenerator.BlockGenerateData[] m_generateBlocks;

	[Header("プレイヤー(トランスフォーム用)")]
	[SerializeField] private GameObject m_player = null;

	[Header("ダンジョンアタッカー(コア設定用)")]
	[SerializeField] private DungeonAttacker m_dungeonAttacker = null;

	// 生成するブロックの種類行列
	private List<List<BlockData.BlockType>> m_blockTypes = new();

	// 生成したブロックの情報
	private readonly List<Block> m_objectBlock = new();



	void Start()
	{
		// プレイヤーのトランスフォーム設定
		if (m_player != null)
		{
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

		if (m_player.TryGetComponent(out SearchBlock search))
		{
			search.SetSearchBlocks(m_objectBlock);
		}
	}

	// ブロックの種類設定
	private void SetBlockType(List<List<string>> mapList)
	{
		// ブロックの種類のリスト
		List<List<BlockData.BlockType>> typeLists = new();

		// ブロック生成用のランダムなオフセット設定
		for (int i = 0; i < m_generateBlocks.Length; i++)
		{
			m_generateBlocks[i].offset = Random.value;
		}

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
					foreach (DungeonGenerator.BlockGenerateData blocks in m_generateBlocks)
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

				GameObject obj;

				// コアの生成
				if (new Vector2Int(x, y) == m_corePosition)
				{
					obj = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, pos);
					m_dungeonAttacker.CorePosition = obj.transform;
				}
				// ブロックの生成
				else
				{
					obj = m_blockGenerator.GenerateBlock(name, pos);
				}

				// ブロックがあれば追加
				if (obj.TryGetComponent(out Block block))
				{
					m_objectBlock.Add(block);
				}


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
			float noise = Mathf.PerlinNoise((pos.x * data.noiseScale) + data.offset, (pos.y * data.noiseScale) + data.offset);
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


	// 鉱石
	private BlockData.BlockType CreateOre()
	{
		int rand = Random.Range((int)BlockData.BlockType.ORE_BEGIN + 1, (int)BlockData.BlockType.ORE_END);

		return (BlockData.BlockType)rand;
	}

}
