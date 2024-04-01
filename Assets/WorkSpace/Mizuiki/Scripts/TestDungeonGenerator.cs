using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		if (m_player != null)
		{
			Instantiate(m_player);
		}

		// ファイルがなければマップ読み込みの処理をしない
		if (!m_dungeonData)
			return;

		// マップのリスト
		List<List<string>> mapList = new List<List<string>>();

		//// ファイル読み込み
		//StreamReader streamReader = new StreamReader(m_dungeonData.ToString());

		// 改行区切りで読み出す
		foreach (string line in /*streamReader.ReadToEnd()*/m_dungeonData.ToString().Split("\n"))
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

		//// ファイルを閉じる
		//streamReader.Close();

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

	// Update is called once per frame
	void Update()
	{

	}
}
