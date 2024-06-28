using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SaveDataReadWrite;

public class _TestJson : MonoBehaviour
{
	[System.Serializable]
	public class JsonList
	{
		// ダンジョンのレベル
		public int dungeonLevel = 1;
		// ダンジョンのクリア状況
		public bool dungeonClear = false;
		// ダンジョンのターン経過
		public int turn = 0;
		// ダンジョンのブロック配置
		public List<List<BlockData.BlockType>> blockList = new();
	}

	[Header("CSV")]
	[SerializeField] private TextAsset m_csvData = null;

	[ContextMenu("TestSave")]
	public void SaveTest()
	{
		// データ取得
		JsonList data = new();

		for (int i = 0; i < 10; i++)
		{
			List<BlockData.BlockType> list = new();

			for (int j = 0; j < 10; j++)
			{
				list.Add(BlockData.BlockType.STONE);
			}
			data.blockList.Add(list);
		}

		// CSVの作成
		WriteReadCSV.WriteCSV(data.blockList, "Data/TestData.csv");

		// 書き込み形式に変換
		string json = JsonUtility.ToJson(data);
		Debug.Log(json);
		// 書き込むファイルを開く
		StreamWriter writer = new(Application.dataPath + "/Data/TestData.json");
		// 書き込み
		writer.Write(json);
		// ファイルを閉じる
		writer.Close();
	}

	[ContextMenu("TestLoad")]
	public void LoadTest()
	{
		// ファイル読み込み
		StreamReader reader = new(Application.dataPath + "/Data/TestData.json");
		// データ読み取り
		string json = reader.ReadToEnd();
		// ファイルを閉じる
		reader.Close();
		// データ形式に変換
		List<List<int>> saveData = JsonUtility.FromJson<List<List<int>>>(json);
		Debug.Log(saveData);
	}

	[ContextMenu("TestLoadCSV")]
	public void LoadTestCSV()
	{
		var data = WriteReadCSV.ReadCSV<BlockData.BlockType>(m_csvData);

		Debug.Log(data);
	}
}

