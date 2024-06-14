using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveDataReadWrite : MonoBehaviour
{
	[System.Serializable]
	public struct KeyValue<TKey, TValue>
	{
		public TKey key;
		public TValue value;

		public KeyValue(TKey k, TValue v)
		{
			key = k;
			value = v;
		}
	}

	[System.Serializable]
	public class SaveData
	{
		// アイテム所持数
		public List<KeyValue<ItemData.ItemType, int>> itemData = new();

		// 採掘道具のレベル
		public List<KeyValue<MiningData.MiningType, int>> miningLevel = new();

		// ダンジョンのレベル
		public int[] dungeonLevel = new int[5];
		// ダンジョンのクリア状況
		public bool[] dungeonClear = new bool[5];

	}

	[Header("ファイル名")]
	[SerializeField] private string m_filePath = "Data/SaveData.json";

	// アイテム所持数
	private Dictionary<ItemData.ItemType, int> m_items = new();
	// 採掘レベル
	private Dictionary<MiningData.MiningType, int> m_miningLevels = new();
	// ダンジョンのレベル
	private int[] m_dungeonLevel = new int[5];
	// ダンジョンのクリア状況
	private bool[] m_dungeonClear = new bool[5];



	private void Awake()
	{
		// 破棄されないオブジェクトにする
		DontDestroyOnLoad(this);

		// ファイル名の設定
		m_filePath = Application.dataPath + "/" + m_filePath;

		// ファイルが存在する場合は読み込んでおく
		if (File.Exists(m_filePath))
			Read();

		Debug.Log("ファイルがないよ");

		// ファイルがないからデータを作る
		// アイテム
		foreach (ItemData.ItemType type in Enum.GetValues(typeof(ItemData.ItemType)))
		{
			m_items[type] = 0;
		}
		// 採掘道具
		foreach (MiningData.MiningType type in Enum.GetValues(typeof(MiningData.MiningType)))
		{
			m_miningLevels[type] = 0;
		}

		Write();

	}


	// 読み込み
	public void Read()
	{
		// ファイル読み込み
		StreamReader reader = new(m_filePath);
		// データ読み取り
		string json = reader.ReadToEnd();
		// ファイルを閉じる
		reader.Close();
		// データ形式に変換
		SaveData saveData = JsonUtility.FromJson<SaveData>(json);
		// データ設定
		SetData(saveData);
	}

	// 書き込み
	public void Write()
	{
		// データ取得
		SaveData saveData = GetData();
		// 書き込み形式に変換
		string json = JsonUtility.ToJson(saveData);
		// 書き込むファイルを開く
		StreamWriter writer = new(m_filePath);
		// 書き込み
		writer.Write(json);
		// ファイルを閉じる
		writer.Close();
	}


	// アイテム所持数
	public Dictionary<ItemData.ItemType, int> Items
	{
		get { return m_items; }
		set { m_items = value; }
	}
	// 採掘レベル
	public Dictionary<MiningData.MiningType, int> MiningLevel
	{
		get { return m_miningLevels; }
		set { m_miningLevels = value; }
	}




	// データの設定
	private void SetData(SaveData data)
	{
		// アイテム所持数
		m_items.Clear();
		foreach (KeyValue<ItemData.ItemType, int> item in data.itemData)
		{
			m_items[item.key] = item.value;
		}
		// 採掘レベル
		m_miningLevels.Clear();
		foreach (KeyValue<MiningData.MiningType, int> level in data.miningLevel)
		{
			m_miningLevels[level.key] = level.value;
		}
		// ダンジョンレベル
		m_dungeonLevel = data.dungeonLevel;
		// ダンジョンのクリア状況
		m_dungeonClear = data.dungeonClear;
	}

	// データの取得
	private SaveData GetData()
	{
		// データ作成
		SaveData saveData = new();

		// アイテム所持数
		foreach (KeyValuePair<ItemData.ItemType, int> item in m_items)
		{
			saveData.itemData.Add(new KeyValue<ItemData.ItemType, int>(item.Key, 0));
		}
		// 採掘レベル
		foreach (KeyValuePair<MiningData.MiningType, int> level in m_miningLevels)
		{
			saveData.miningLevel.Add(new KeyValue<MiningData.MiningType, int>(level.Key, 0));
		}
		// ダンジョンレベル
		saveData.dungeonLevel = m_dungeonLevel;
		// ダンジョンのクリア状況
		saveData.dungeonClear = m_dungeonClear;

		// セーブデータを返す
		return saveData;
	}
}

