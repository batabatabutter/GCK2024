using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestDungeonGenerator : MonoBehaviour
{
	[Header("生成するダンジョンのパス")]
	[SerializeField] private string m_dungeonPath = "Assets/DungeonData/Dungeon.csv";

	[Header("ダンジョンのサイズ")]
	[SerializeField] private int m_dungeonSizeX;
	[SerializeField] private int m_dungeonSizeY;

	[Header("生成ブロック")]
	[SerializeField] private GameObject m_block = null;

	//[Header("何の変哲もないブロック")]
	//[SerializeField] private string m_blockNormal = "1";
	[Header("破壊不可能ブロック")]
	[SerializeField] private string m_blockDontBroken = "2";
	[Header("ダンジョンの核")]
	[SerializeField] private string m_blockCore = "3";

	[SerializeField] GameObject m_player = null;

	// Start is called before the first frame update
	void Start()
	{
		if (m_player != null)
		{
			Instantiate<GameObject>(m_player);
		}

		// ファイルがなければマップ読み込みの処理をしない
		if (!File.Exists(m_dungeonPath))
			return;

		// マップのリスト
		List<List<string>> mapList = new List<List<string>>();

		// ファイル読み込み
		StreamReader streamReader = new StreamReader(m_dungeonPath);

		// 改行区切りで読み出す
		foreach (string line in streamReader.ReadToEnd().Split("\n"))
		{
			// 行が存在しなければループを抜ける
			if (line == "")
				break;

			string lin = line.Remove(line.Length - 1);

			List<string> list = new List<string>();

			// カンマ区切りで読み出す
			foreach (string line2 in lin.Split(","))
			{
				list.Add(line2);
			}

			mapList.Add(list);
		}

		// ファイルを閉じる
		streamReader.Close();

		// 読みだしたデータをもとにダンジョン生成をする
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				// 0 の場合は何も生成しない
				if (mapList[y][x] == "0" || mapList[y][x] == "")
					continue;

				// 生成座標
				Vector3 pos = new(x, y, 0.0f);

				// ブロックの生成
				GameObject block = Instantiate<GameObject>(m_block, pos, Quaternion.identity);
				// ブロックスクリプトをつける
				block.AddComponent<Block>();

				// 破壊不可能ブロックにする
				if (mapList[y][x] == m_blockDontBroken)
				{
					// 分かりやすいようにとりあえず色を変える
					block.GetComponent<SpriteRenderer>().color = Color.gray;
					// 破壊不可能にする
					block.GetComponent<Block>().DontBroken = true;
				}

				// 核にする
				if (mapList[y][x] == m_blockCore)
				{
					// 分かりやすいようにとりあえず色を変える
					block.GetComponent<SpriteRenderer>().color = Color.red;
					// ダンジョンスクリプトをつける
					block.AddComponent<Dungeon>();
				}


			}
		}


	}

	// Update is called once per frame
	void Update()
	{

	}
}
