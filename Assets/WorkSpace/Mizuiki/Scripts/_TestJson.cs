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
		// �_���W�����̃��x��
		public int dungeonLevel = 1;
		// �_���W�����̃N���A��
		public bool dungeonClear = false;
		// �_���W�����̃^�[���o��
		public int turn = 0;
		// �_���W�����̃u���b�N�z�u
		public List<List<BlockData.BlockType>> blockList = new();
	}

	[Header("CSV")]
	[SerializeField] private TextAsset m_csvData = null;

	[ContextMenu("TestSave")]
	public void SaveTest()
	{
		// �f�[�^�擾
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

		// CSV�̍쐬
		WriteReadCSV.WriteCSV(data.blockList, "Data/TestData.csv");

		// �������݌`���ɕϊ�
		string json = JsonUtility.ToJson(data);
		Debug.Log(json);
		// �������ރt�@�C�����J��
		StreamWriter writer = new(Application.dataPath + "/Data/TestData.json");
		// ��������
		writer.Write(json);
		// �t�@�C�������
		writer.Close();
	}

	[ContextMenu("TestLoad")]
	public void LoadTest()
	{
		// �t�@�C���ǂݍ���
		StreamReader reader = new(Application.dataPath + "/Data/TestData.json");
		// �f�[�^�ǂݎ��
		string json = reader.ReadToEnd();
		// �t�@�C�������
		reader.Close();
		// �f�[�^�`���ɕϊ�
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

