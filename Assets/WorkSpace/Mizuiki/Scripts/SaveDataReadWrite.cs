using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveDataReadWrite : MonoBehaviour
{
	// �Z�[�u�@�\�̃C���X�^���X
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
	// �_���W�����̏��
	[System.Serializable]
	public class DungeonState
	{
		// �_���W�����̃��x��
		public int dungeonLevel = 1;
		// �_���W�����̃N���A��
		public bool dungeonClear = false;
		// �_���W�����̃^�[���o��
		public int turn = 0;
		// �e���x���̃N���A��
		public int[] clearCount = new int[11];
		// �_���W�����̃u���b�N�z�u
		public List<List<BlockData.BlockType>> blockList = new();
	}
	// �ۑ��`��
	[System.Serializable]
	public class SaveData
	{
		// ����
		public MiningData.MiningType miningType = MiningData.MiningType.BALANCE;

		// �A�C�e��������
		public List<KeyValue<ItemData.ItemType, int>> itemData = new();

		// �̌@����̃��x��
		public List<KeyValue<MiningData.MiningType, int>> miningLevel = new();

		// �_���W�����̏��
		public DungeonState[] dungeonStates = new DungeonState[DungeonDataBase.DUNGEON_COUNT];
	}

	[Header("�t�@�C����(�g���q�͕t���Ȃ�)")]
	[SerializeField] private string m_fileName = "Data/SaveData";

	[Header("�ǂݏ����Ώۃf�[�^")]
	[SerializeField] private bool m_isCSV = false;

	// ����
	private MiningData.MiningType m_miningType = MiningData.MiningType.BALANCE;
	// �A�C�e��������
	private Dictionary<ItemData.ItemType, int> m_items = new();
	// �̌@���x��
	private Dictionary<MiningData.MiningType, int> m_miningLevels = new();
	// �_���W�����̏��
	private DungeonState[] m_dungeonStates = new DungeonState[DungeonDataBase.DUNGEON_COUNT];




	private void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else
		{
			// �������ꂽ�ꍇ�͔j������
			Destroy(gameObject);
			return;
		}

		// �t�@�C�����̐ݒ�
		string filePath = Application.dataPath + "/" + m_fileName + ".json";

		// �t�@�C�������݂���ꍇ�͓ǂݍ���ł���
		if (File.Exists(filePath))
		{
			Load();
			return;
		}

		Debug.Log("�t�@�C�����Ȃ���");

		// �t�@�C�����Ȃ�����f�[�^�����
		CreateNewData();

	}

	// ****************************** �f�[�^�̓ǂݍ��� ****************************** //
	// �ǂݍ���
	public void Load()
	{
		// �t�@�C���p�X
		string path = Application.dataPath + "/" + m_fileName;

		// Json�t�@�C���ǂݏo��
		string json = MyFunction.Reader(path + ".json");
		// �f�[�^�`���ɕϊ�
		SaveData saveData = JsonUtility.FromJson<SaveData>(json);

		// �_���W�����̃f�[�^�ǂݏo��
		if (m_isCSV)
		{
			for (int i = 0; i < DungeonDataBase.DUNGEON_COUNT; i++)
			{
				// CSV�t�@�C���ǂݏo��
				string csv = MyFunction.Reader(path + i + ".csv");
				// �f�[�^�ݒ�
				saveData.dungeonStates[i].blockList = WriteReadCSV.ReadCSV<BlockData.BlockType>(csv);
			}
		}

		// �f�[�^�ݒ�
		SetData(saveData);

		Debug.Log("�ǂݏo������");
	}

	// ****************************** �f�[�^�̏������� ****************************** //
	// ��������
	public void Save()
	{
		Save(m_miningType, m_items, m_miningLevels, m_dungeonStates, m_fileName);
	}
	public void Save(MiningData.MiningType mining, Dictionary<ItemData.ItemType, int> items, Dictionary<MiningData.MiningType, int> miningLevels, DungeonState[] dungeonStates, string fileName)
	{
		string path = Application.dataPath + "/" + fileName;

		// �f�[�^�擾
		SaveData saveData = GetSaveData(mining, items, miningLevels, dungeonStates);
		// �������݌`���ɕϊ�
		string json = JsonUtility.ToJson(saveData);
		// �f�[�^��������
		MyFunction.Writer(path + ".json", json);

		Debug.Log("�������݊���");
	}

	// �X�e�[�W�̕ۑ�
	public void SaveBlocks(int stageNum)
	{
		// �t�@�C����
		string fileName = m_fileName + stageNum + ".csv";
		// �f�[�^��������
		WriteReadCSV.WriteCSV(m_dungeonStates[stageNum].blockList, fileName);
	}

	// ****************************** �f�[�^�̍쐬 ****************************** //
	[ContextMenu("CreateNewFile")]
	public void CreateNewFile()
	{
		// ����
		MiningData.MiningType miningType = MiningData.MiningType.BALANCE;
		// �A�C�e��
		Dictionary<ItemData.ItemType, int> items = new();
		foreach (ItemData.ItemType type in Enum.GetValues(typeof(ItemData.ItemType)))
		{
			items[type] = 0;
		}
		// �̌@����
		Dictionary<MiningData.MiningType, int> miningLevels = new();
		foreach (MiningData.MiningType type in Enum.GetValues(typeof(MiningData.MiningType)))
		{
			miningLevels[type] = 0;
		}
		// �_���W�����̏��
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
		// ��������
		Save(miningType, items, miningLevels, dungeonState, m_fileName);
	}

	public void CreateNewData()
	{
		// ����
		m_miningType = MiningData.MiningType.BALANCE;
		// �A�C�e��
		foreach (ItemData.ItemType type in Enum.GetValues(typeof(ItemData.ItemType)))
		{
			m_items[type] = 0;
		}
		// �̌@����
		foreach (MiningData.MiningType type in Enum.GetValues(typeof(MiningData.MiningType)))
		{
			m_miningLevels[type] = 0;
		}
		// �_���W�����̏��
		for (int i = 0; i < DungeonDataBase.DUNGEON_COUNT; i++)
		{
			DungeonState ds = m_dungeonStates[i] = new();
			ds.blockList = new();
			for (int j = 0; j < 2; j++)
			{
				ds.blockList.Add(new List<BlockData.BlockType> { BlockData.BlockType.STONE, BlockData.BlockType.STONE, BlockData.BlockType.STONE});
			}
		}
		// ��������
		Save();
	}

	// ****************************** �f�[�^�̐ݒ� ****************************** //
	// �_���W�����̃u���b�N�z�u�ݒ�
	public void SetBlocks(Block[,] blocks, int stageNum)
	{
		List<List<BlockData.BlockType>> blockTypes = new();

		for (int y = 0; y < blocks.GetLength(0); y++)
		{
			List<BlockData.BlockType> blockType = new();

			for (int x = 0; x < blocks.GetLength(1); x++)
			{
				// �u���b�N�����݂��Ȃ�
				if (blocks[y, x] == null)
				{
					blockType.Add(BlockData.BlockType.OVER);
					continue;
				}
				// �u���b�N�̎�ގ擾
				blockType.Add(blocks[y, x].BlockData.Type);
			}
			// ���X�g�ǉ�
			blockTypes.Add(blockType);
		}
		// �_���W�����̏�Ԃ�ݒ肷��
		m_dungeonStates[stageNum].blockList = blockTypes;

	}

	// �N���A�ݒ�
	public void SetClear(int stageNum)
	{
		m_dungeonStates[stageNum].dungeonClear = true;
	}

	// �_���W�����̃��x�����グ��
	public void AddLevel(int stageNum)
	{
		m_dungeonStates[stageNum].dungeonLevel++;
	}

	// �_���W�����̃��x�����擾
	public int GetDungeonLevel(int dungeon)
	{
		return m_dungeonStates[dungeon].dungeonLevel;
	}


	// ����
	public MiningData.MiningType MiningType
	{
		get { return m_miningType; }
		set { m_miningType = value; }
	}
	// �A�C�e��������
	public Dictionary<ItemData.ItemType, int> Items
	{
		get { return m_items; }
		set { m_items = value; }
	}
	// �̌@���x��
	public Dictionary<MiningData.MiningType, int> MiningLevel
	{
		get { return m_miningLevels; }
		set { m_miningLevels = value; }
	}
	// �_���W�����̏��
	public DungeonState[] DungeonStates
	{
		get { return m_dungeonStates; }
		set { m_dungeonStates = value; }
	}
	// �t�@�C����
	public string FileName
	{
		get { return m_fileName; }
	}



	// �f�[�^�̐ݒ�
	private void SetData(SaveData data)
	{
		// ����
		m_miningType = data.miningType;
		// �A�C�e��������
		m_items.Clear();
		foreach (KeyValue<ItemData.ItemType, int> item in data.itemData)
		{
			m_items[item.key] = item.value;
		}
		// �̌@���x��
		m_miningLevels.Clear();
		foreach (KeyValue<MiningData.MiningType, int> level in data.miningLevel)
		{
			m_miningLevels[level.key] = level.value;
		}
		// �_���W�����̏��
		m_dungeonStates = data.dungeonStates;
	}

	// �f�[�^�̎擾
	private SaveData GetSaveData()
	{
		return GetSaveData(m_miningType, m_items, m_miningLevels, m_dungeonStates);
	}
	private SaveData GetSaveData(MiningData.MiningType mining, Dictionary<ItemData.ItemType, int> items, Dictionary<MiningData.MiningType, int> miningLevels, DungeonState[] dungeonStates)
	{
		// �f�[�^�쐬
		SaveData saveData = new();

		// �����ݒ�
		saveData.miningType = mining;
		// �A�C�e��������
		foreach (KeyValuePair<ItemData.ItemType, int> item in items)
		{
			saveData.itemData.Add(new KeyValue<ItemData.ItemType, int>(item.Key, item.Value));
		}
		// �̌@���x��
		foreach (KeyValuePair<MiningData.MiningType, int> level in miningLevels)
		{
			saveData.miningLevel.Add(new KeyValue<MiningData.MiningType, int>(level.Key, level.Value));
		}
		// �_���W�����̏��
		saveData.dungeonStates = dungeonStates;

		// �Z�[�u�f�[�^��Ԃ�
		return saveData;
	}
}

