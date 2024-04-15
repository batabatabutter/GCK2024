using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorCSV : DungeonGeneratorBase
{
	// マップ
	private readonly List<List<string>> m_mapList = new();

	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
	{
		// マップの初期化
		for (int y = 0; y < dungeonData.Size.y; y++)
		{
			List<string> list = new();
			for (int x = 0; x < dungeonData.Size.x; x++)
			{
				list.Add("1");
			}
			m_mapList.Add(list);
		}

		// データをキャスト
		DungeonDataCSV data = dungeonData as DungeonDataCSV;

		// キャストできない
		if (data == null)
		{
			return m_mapList;
		}

		Vector2Int size = data.Size;

		//１０＊１０のリスト管理用
		List<List<List<string>>> mapListManager = new();

		//データベースからリストの取得
		List<TextAsset> dungeonCSV = data.DungeonCSV;

		for (int i = 0; i < dungeonCSV.Count; i++)
		{
			// ファイルがなければマップ読み込みの処理をしない
			if (dungeonCSV[i] == null)
			{
				Debug.Log("CSV file is not assigned");
				return new();

			}

			// ファイルを改行区切りで配列に格納
			string[] lines = dungeonCSV[i].text.Split('\n');
			// 読みだしたデータ格納用リスト
			List<List<string>> list = new();
			// ファイルの内容を1行ずつ処理
			foreach (string line in lines)
			{
				// 文字列がない
				if (line == "")
					break;

				// 文字列をカンマ区切りで配列に格納
				string[] values = line.Split(',');
				// 各行のデータを格納するリスト
				List<string> rowData = new();
				// 各列の値を処理する
				foreach (string value in values)
				{
					// データをリストに追加
					rowData.Add(value);
				}

				// 行のデータをCSVデータに追加
				list.Add(rowData);
			}
			//３次元に入れる
			mapListManager.Add(list);
		}

		//ブロック生成
		for (int y = 0; y < size.y / 10; y++)
		{
			for (int x = 0; x < size.x / 10; x++)
			{
				int random = Random.Range(0, dungeonCSV.Count);
				Make10_10Block(mapListManager[random], x, y);

			}
		}

		return m_mapList;
	}

	void Make10_10Block(List<List<string>> mapList, int originX, int originY)
	{
		// 読みだしたデータをもとにダンジョン生成をする
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				//// 0 の場合は何も生成しない
				//if (mapList[y][x] == "0" || mapList[y][x] == "")
				//	continue;

				// 生成座標
				Vector2Int pos = new((originX * 10) + x, (originY * 10) + y);
				// マップ追加
				m_mapList[pos.y][pos.x] = mapList[y][x];
			}
		}
	}

}
