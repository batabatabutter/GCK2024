using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveDataReadWrite : MonoBehaviour
{
	// セーブ機能のインスタンス
	public static SaveDataReadWrite m_instance;

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
	// ダンジョンの状態
	[System.Serializable]
	public class DungeonState
	{
		// ダンジョンのレベル
		public int dungeonLevel = 1;
		// ダンジョンのクリア状況
		public bool dungeonClear = false;
		// ダンジョンのターン経過
		public int turn = 0;
		// 各レベルのクリア回数
		public int[] clearCount = new int[11];
		// ダンジョンのブロック配置
		public List<List<BlockData.BlockType>> blockList = new();
	}
	// 保存形式
	[System.Serializable]
	public class SaveData
	{
		// 装備
		public MiningData.MiningType miningType = MiningData.MiningType.BALANCE;

		// アイテム所持数
		public List<KeyValue<ItemData.ItemType, int>> itemData = new();

		// 採掘道具のレベル
		public List<KeyValue<MiningData.MiningType, int>> miningLevel = new();

		// ダンジョンの状態
		public DungeonState[] dungeonStates = new DungeonState[DungeonDataBase.DUNGEON_COUNT];
	}

	[Header("ファイル名(拡張子は付けない)")]
	[SerializeField] private string m_fileName = "Data/SaveData";

	[Header("読み書き対象データ")]
	[SerializeField] private bool m_isCSV = false;

	// 装備
	private MiningData.MiningType m_miningType = MiningData.MiningType.BALANCE;
	// アイテム所持数
	private Dictionary<ItemData.ItemType, int> m_items = new();
	// 採掘レベル
	private Dictionary<MiningData.MiningType, int> m_miningLevels = new();
	// ダンジョンの状態
	private DungeonState[] m_dungeonStates = new DungeonState[DungeonDataBase.DUNGEON_COUNT];




	private void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else
		{
			// 複数作られた場合は破棄する
			Destroy(gameObject);
			return;
		}

		// ファイル名の設定
		string filePath = Application.dataPath + "/" + m_fileName + ".json";

		// ファイルが存在する場合は読み込んでおく
		if (File.Exists(filePath))
		{
			Load();
			return;
		}

		Debug.Log("ファイルがないよ");

		// ファイルがないからデータを作る
		CreateNewData();

	}

	// ****************************** データの読み込み ****************************** //
	// 読み込み
	public void Load()
	{
		// ファイルパス
		string path = Application.dataPath + "/" + m_fileName;

		// Jsonファイル読み出し
		string json = MyFunction.Reader(path + ".json");
		// データ形式に変換
		SaveData saveData = JsonUtility.FromJson<SaveData>(json);

		// ダンジョンのデータ読み出し
		if (m_isCSV)
		{
			for (int i = 0; i < DungeonDataBase.DUNGEON_COUNT; i++)
			{
				// CSVファイル読み出し
				string csv = MyFunction.Reader(path + i + ".csv");
				// データ設定
				saveData.dungeonStates[i].blockList = WriteReadCSV.ReadCSV<BlockData.BlockType>(csv);
			}
		}

		// データ設定
		SetData(saveData);

		Debug.Log("読み出し完了");
	}

	// ****************************** データの書き込み ****************************** //
	// 書き込み
	public void Save()
	{
		Save(m_miningType, m_items, m_miningLevels, m_dungeonStates, m_fileName);
	}
	public void Save(MiningData.MiningType mining, Dictionary<ItemData.ItemType, int> items, Dictionary<MiningData.MiningType, int> miningLevels, DungeonState[] dungeonStates, string fileName)
	{
		string path = Application.dataPath + "/" + fileName;

		// データ取得
		SaveData saveData = GetSaveData(mining, items, miningLevels, dungeonStates);
		// 書き込み形式に変換
		string json = JsonUtility.ToJson(saveData);
		// データ書き込み
		MyFunction.Writer(path + ".json", json);

		Debug.Log("書き込み完了");
	}

	// ステージの保存
	public void SaveBlocks(int stageNum)
	{
		// ファイル名
		string fileName = m_fileName + stageNum + ".csv";
		// データ書き込み
		WriteReadCSV.WriteCSV(m_dungeonStates[stageNum].blockList, fileName);
	}

	// ****************************** データの作成 ****************************** //
	[ContextMenu("CreateNewFile")]
	public void CreateNewFile()
	{
		// 装備
		MiningData.MiningType miningType = MiningData.MiningType.BALANCE;
		// アイテム
		Dictionary<ItemData.ItemType, int> items = new();
		foreach (ItemData.ItemType type in Enum.GetValues(typeof(ItemData.ItemType)))
		{
			items[type] = 0;
		}
		// 採掘道具
		Dictionary<MiningData.MiningType, int> miningLevels = new();
		foreach (MiningData.MiningType type in Enum.GetValues(typeof(MiningData.MiningType)))
		{
			miningLevels[type] = 0;
		}
		// ダンジョンの状態
		DungeonState[] dungeonState = new DungeonState[DungeonDataBase.DUNGEON_COUNT];
		for (int i = 0; i < DungeonDataBase.DUNGEON_COUNT; i++)
		{
			DungeonState ds = dungeonState[i] = new();
			ds.blockList = new();
			for (int j = 0; j < 2; j++)
			{
				ds.blockList.Add(new List<BlockData.BlockType> { BlockData.BlockType.STONE, BlockData.BlockType.STONE, BlockData.BlockType.STONE });
			}
		}
		// 書き込み
		Save(miningType, items, miningLevels, dungeonState, m_fileName);
	}

	public void CreateNewData()
	{
		// 装備
		m_miningType = MiningData.MiningType.BALANCE;
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
		// ダンジョンの状態
		for (int i = 0; i < DungeonDataBase.DUNGEON_COUNT; i++)
		{
			DungeonState ds = m_dungeonStates[i] = new();
			ds.blockList = new();
			for (int j = 0; j < 2; j++)
			{
				ds.blockList.Add(new List<BlockData.BlockType> { BlockData.BlockType.STONE, BlockData.BlockType.STONE, BlockData.BlockType.STONE});
			}
		}
		// 書き込み
		Save();
	}

	// ****************************** データの設定 ****************************** //
	// ダンジョンのブロック配置設定
	public void SetBlocks(Block[,] blocks, int stageNum)
	{
		List<List<BlockData.BlockType>> blockTypes = new();

		for (int y = 0; y < blocks.GetLength(0); y++)
		{
			List<BlockData.BlockType> blockType = new();

			for (int x = 0; x < blocks.GetLength(1); x++)
			{
				// ブロックが存在しない
				if (blocks[y, x] == null)
				{
					blockType.Add(BlockData.BlockType.OVER);
					continue;
				}
				// ブロックの種類取得
				blockType.Add(blocks[y, x].BlockData.Type);
			}
			// リスト追加
			blockTypes.Add(blockType);
		}
		// ダンジョンの状態を設定する
		m_dungeonStates[stageNum].blockList = blockTypes;

	}

	// クリア設定
	public void SetClear(int stageNum)
	{
		m_dungeonStates[stageNum].dungeonClear = true;
	}

	// ダンジョンのレベルを上げる
	public void AddLevel(int stageNum)
	{
		m_dungeonStates[stageNum].dungeonLevel++;
	}

	// ダンジョンのレベルを取得
	public int GetDungeonLevel(int dungeon)
	{
		return m_dungeonStates[dungeon].dungeonLevel;
	}


	// 装備
	public MiningData.MiningType MiningType
	{
		get { return m_miningType; }
		set { m_miningType = value; }
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
	// ダンジョンの状態
	public DungeonState[] DungeonStates
	{
		get { return m_dungeonStates; }
		set { m_dungeonStates = value; }
	}
	// ファイル名
	public string FileName
	{
		get { return m_fileName; }
	}



	// データの設定
	private void SetData(SaveData data)
	{
		// 装備
		m_miningType = data.miningType;
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
		// ダンジョンの状態
		m_dungeonStates = data.dungeonStates;
	}

	// データの取得
	private SaveData GetSaveData()
	{
		return GetSaveData(m_miningType, m_items, m_miningLevels, m_dungeonStates);
	}
	private SaveData GetSaveData(MiningData.MiningType mining, Dictionary<ItemData.ItemType, int> items, Dictionary<MiningData.MiningType, int> miningLevels, DungeonState[] dungeonStates)
	{
		// データ作成
		SaveData saveData = new();

		// 装備設定
		saveData.miningType = mining;
		// アイテム所持数
		foreach (KeyValuePair<ItemData.ItemType, int> item in items)
		{
			saveData.itemData.Add(new KeyValue<ItemData.ItemType, int>(item.Key, item.Value));
		}
		// 採掘レベル
		foreach (KeyValuePair<MiningData.MiningType, int> level in miningLevels)
		{
			saveData.miningLevel.Add(new KeyValue<MiningData.MiningType, int>(level.Key, level.Value));
		}
		// ダンジョンの状態
		saveData.dungeonStates = dungeonStates;

		// セーブデータを返す
		return saveData;
	}
}

