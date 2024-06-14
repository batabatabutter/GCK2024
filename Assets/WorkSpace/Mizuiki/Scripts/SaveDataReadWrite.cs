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
		// �A�C�e��������
		public List<KeyValue<ItemData.ItemType, int>> itemData = new();

		// �̌@����̃��x��
		public List<KeyValue<MiningData.MiningType, int>> miningLevel = new();

		// �_���W�����̃��x��
		public int[] dungeonLevel = new int[5];
		// �_���W�����̃N���A��
		public bool[] dungeonClear = new bool[5];

	}

	[Header("�t�@�C����")]
	[SerializeField] private string m_filePath = "Data/SaveData.json";

	// �A�C�e��������
	private Dictionary<ItemData.ItemType, int> m_items = new();
	// �̌@���x��
	private Dictionary<MiningData.MiningType, int> m_miningLevels = new();
	// �_���W�����̃��x��
	private int[] m_dungeonLevel = new int[5];
	// �_���W�����̃N���A��
	private bool[] m_dungeonClear = new bool[5];



	private void Awake()
	{
		// �j������Ȃ��I�u�W�F�N�g�ɂ���
		DontDestroyOnLoad(this);

		// �t�@�C�����̐ݒ�
		m_filePath = Application.dataPath + "/" + m_filePath;

		// �t�@�C�������݂���ꍇ�͓ǂݍ���ł���
		if (File.Exists(m_filePath))
			Read();

		Debug.Log("�t�@�C�����Ȃ���");

		// �t�@�C�����Ȃ�����f�[�^�����
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

		Write();

	}


	// �ǂݍ���
	public void Read()
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
	public void Write()
	{
		// �f�[�^�擾
		SaveData saveData = GetData();
		// �������݌`���ɕϊ�
		string json = JsonUtility.ToJson(saveData);
		// �������ރt�@�C�����J��
		StreamWriter writer = new(m_filePath);
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
		// �_���W�������x��
		m_dungeonLevel = data.dungeonLevel;
		// �_���W�����̃N���A��
		m_dungeonClear = data.dungeonClear;
	}

	// �f�[�^�̎擾
	private SaveData GetData()
	{
		// �f�[�^�쐬
		SaveData saveData = new();

		// �A�C�e��������
		foreach (KeyValuePair<ItemData.ItemType, int> item in m_items)
		{
			saveData.itemData.Add(new KeyValue<ItemData.ItemType, int>(item.Key, 0));
		}
		// �̌@���x��
		foreach (KeyValuePair<MiningData.MiningType, int> level in m_miningLevels)
		{
			saveData.miningLevel.Add(new KeyValue<MiningData.MiningType, int>(level.Key, 0));
		}
		// �_���W�������x��
		saveData.dungeonLevel = m_dungeonLevel;
		// �_���W�����̃N���A��
		saveData.dungeonClear = m_dungeonClear;

		// �Z�[�u�f�[�^��Ԃ�
		return saveData;
	}
}

