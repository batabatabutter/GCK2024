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

	[System.Serializable]
	public class DungeonState
	{
		// �_���W�����̃��x��
		public int dungeonLevel = 1;
		// �_���W�����̃N���A��
		public bool dungeonClear = false;
		// �_���W�����̃^�[���o��
		public int turn = 0;
		// �_���W�����̃u���b�N�z�u
		public List<List<BlockData.BlockType>> blockList = new();
	}

	[System.Serializable]
	public class SaveData
	{
		// �A�C�e��������
		public List<KeyValue<ItemData.ItemType, int>> itemData = new();

		// �̌@����̃��x��
		public List<KeyValue<MiningData.MiningType, int>> miningLevel = new();

		// �_���W�����̏��
		public DungeonState[] dungeonStates = new DungeonState[5];

		public Dictionary<int, string> hoge = new();
	}

	[Header("�t�@�C����")]
	[SerializeField] private string m_filePath = "Data/SaveData.json";

	// �A�C�e��������
	private Dictionary<ItemData.ItemType, int> m_items = new();
	// �̌@���x��
	private Dictionary<MiningData.MiningType, int> m_miningLevels = new();
	// �_���W�����̏��
	private DungeonState[] m_dungeonStates = new DungeonState[5];




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
		}

		// �t�@�C�����̐ݒ�
		m_filePath = Application.dataPath + "/" + m_filePath;

		// �t�@�C�������݂���ꍇ�͓ǂݍ���ł���
		if (File.Exists(m_filePath))
		{
			Load();
			return;
		}

		Debug.Log("�t�@�C�����Ȃ���");

		// �t�@�C�����Ȃ�����f�[�^�����
		CreateNewData();

	}

	[ContextMenu("CreateNewData")]
	public void CreateNewData()
	{
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
		DungeonState[] dungeonState = new DungeonState[5];
		//foreach (DungeonState ds in dungeonState)
		for (int i = 0; i < 5; i++)
		{
			DungeonState ds = dungeonState[i] = new();
			ds.blockList = new();
			for (int j = 0; j < 2; j++)
			{
				ds.blockList.Add(new List<BlockData.BlockType> { BlockData.BlockType.STONE, BlockData.BlockType.STONE, BlockData.BlockType.STONE});
			}
		}
		// �p�X�擾
		string path = Application.dataPath + "/" + m_filePath;

		// ��������
		Save(items, miningLevels, dungeonState, path);

	}

	// �ǂݍ���
	public void Load()
	{
		// �t�@�C���ǂݍ���
		StreamReader reader = new(m_filePath);
		// �f�[�^�ǂݎ��
		string json = reader.ReadToEnd();
		// �t�@�C�������
		reader.Close();
		// �f�[�^�`���ɕϊ�
		SaveData saveData = JsonUtility.FromJson<SaveData>(json);
		// �f�[�^�ݒ�
		SetData(saveData);
	}

	// ��������
	public void Save()
	{
		Save(m_items, m_miningLevels, m_dungeonStates, m_filePath);
	}
	public void Save(Dictionary<ItemData.ItemType, int> items, Dictionary<MiningData.MiningType, int> miningLevels, DungeonState[] dungeonStates, string path)
	{
		// �f�[�^�擾
		SaveData saveData = GetSaveData(items, miningLevels, dungeonStates);
		// �������݌`���ɕϊ�
		string json = JsonUtility.ToJson(saveData);
		// �������ރt�@�C�����J��
		StreamWriter writer = new(path);
		// ��������
		writer.Write(json);
		// �t�@�C�������
		writer.Close();
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




	// �f�[�^�̐ݒ�
	private void SetData(SaveData data)
	{
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
		return GetSaveData(m_items, m_miningLevels, m_dungeonStates);
	}
	private SaveData GetSaveData(Dictionary<ItemData.ItemType, int> items, Dictionary<MiningData.MiningType, int> miningLevels, DungeonState[] dungeonStates)
	{
		// �f�[�^�쐬
		SaveData saveData = new();

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

