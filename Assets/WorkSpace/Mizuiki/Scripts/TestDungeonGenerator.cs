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
	private readonly Dictionary<string, BlockData.BlockType> m_blocks = new();

	[Header("ブロックジェネレータ")]
	[SerializeField] private BlockGenerator m_blockGenerator;

	[Header("生成方式のインデックス")]
	[SerializeField] private int m_dungeonIndex = 0;
	[Header("生成スクリプトの配列")]
	[SerializeField] private DungeonGeneratorBase[] m_dungeonGenerators;

	[Header("プレイヤー(インスタンス用)")]
	[SerializeField] GameObject m_player = null;


	// Start is called before the first frame update
	void Start()
	{
		// ブロックの設定
		for (int i = 0; i < m_setBlocks.Length; i++)
		{
			MapBlock mapBlock = m_setBlocks[i];

			// 上書き防止
			if (m_blocks.ContainsKey(mapBlock.mapName))
				continue;

			// ブロックの種類設定
			m_blocks[mapBlock.mapName] = mapBlock.blockType;
		}

		// プレイヤーが設定されていれば生成
		if (m_player != null)
		{
			Instantiate(m_player);
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
		Generate(mapList);

	}

	// Update is called once per frame
	void Update()
	{

	}

	// ダンジョンの生成
	private void Generate(List<List<string>> mapList)
	{
		// 読みだしたデータをもとにダンジョン生成をする
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				string name = mapList[y][x];

				// キーが存在しない場合は何も生成しない
				if (!m_blocks.ContainsKey(name))
					continue;

				// 生成座標
				Vector3 pos = new(x, y, 0.0f);

				// ブロックの生成
				m_blockGenerator.GenerateBlock(m_blocks[name], pos);

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


}
