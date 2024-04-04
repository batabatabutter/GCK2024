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
	[SerializeField] private int m_dungeonSizeX;
	[SerializeField] private int m_dungeonSizeY;

	[Header("生成ブロック")]
	[SerializeField] private GameObject m_block = null;
	[SerializeField] private GameObject m_blockBedrock = null;
	[SerializeField] private GameObject m_blockCore = null;

	[Header("何の変哲もないブロック")]
	[SerializeField] private string m_blockNameNormal = "1";
	[Header("破壊不可能ブロック")]
	[SerializeField] private string m_blockNameBedrock = "2";
	[Header("ダンジョンの核")]
	[SerializeField] private string m_blockNameCore = "3";

	[SerializeField] GameObject m_player = null;

	// Start is called before the first frame update
	void Start()
	{
		// プレイヤーが設定されていれば生成
		if (m_player != null)
		{
			Instantiate(m_player);
		}

		// SCV読み込み
		GenerateSCV();

		// ダンジョンらしいダンジョン生成
		//GenerateRoom();

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
				// 0 の場合は何も生成しない
				if (mapList[y][x] == "0" || mapList[y][x] == "")
					continue;

				// 生成座標
				Vector3 pos = new(x, y, 0.0f);

				// ブロックの生成
				GameObject block = null;

				// 普通のブロック
				if (mapList[y][x] == m_blockNameNormal)
				{
					block = Instantiate(m_block, pos, Quaternion.identity);
				}
				// 破壊不可能ブロックにする
				else if (mapList[y][x] == m_blockNameBedrock)
				{
					// 岩盤ブロック生成
					block = Instantiate(m_blockBedrock, pos, Quaternion.identity);
				}
				// 核にする
				else if (mapList[y][x] == m_blockNameCore)
				{
					// 核ブロック生成
					block = Instantiate(m_blockCore, pos, Quaternion.identity);
				}

				// 松明対応
				block.AddComponent<ChangeBrightness>();

			}
		}

	}

	// CSV読み込みの生成
	private void GenerateSCV()
	{
		// ファイルがなければマップ読み込みの処理をしない
		if (!m_dungeonData)
			return;

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

		// マップの生成
		Generate(mapList);

	}


	struct Room
	{
		int topLeftX;   // 左上座標X
		int topLeftY;   // 左上座標Y
		int width;		// 幅
		int height;		// 高さ

	}

	// 部屋分けダンジョン生成
	private void GenerateRoom()
	{
		// マップ
		List<List<string>> mapList = new ();

		// マップの初期化
		for (int y = 0; y < m_dungeonSizeY; y++)
		{
			mapList.Add(new List<string>());

			for (int x = 0; x < m_dungeonSizeX; x++)
			{
				// 通常ブロックで埋める
				mapList[y].Add(m_blockNameNormal);
			}
		}

		// 部屋の生成数(5 ~ 10)
		int generateRoomCount = Random.Range(5, 10);



	}

}
