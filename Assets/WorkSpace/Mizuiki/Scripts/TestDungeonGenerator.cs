using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class TestDungeonGenerator : MonoBehaviour
{
	[Header("��������_���W�����̃f�[�^")]
	[SerializeField] private Object m_dungeonData = null;

	[Header("�_���W�����̃T�C�Y")]
	[SerializeField] private int m_dungeonSizeX;
	[SerializeField] private int m_dungeonSizeY;

	[System.Serializable]
	struct MapBlock
	{
		public string mapName;
		public BlockData.BlockType blockType;
	}

	[Header("�����u���b�N")]
	[SerializeField] private MapBlock[] m_setBlocks;
	private Dictionary<string, BlockData.BlockType> m_blocks = new();

	[Header("�u���b�N�W�F�l���[�^")]
	[SerializeField] private BlockGenerator m_blockGenerator;

	[SerializeField] GameObject m_player = null;


	// Start is called before the first frame update
	void Start()
	{
		// �u���b�N�̐ݒ�
		for (int i = 0; i < m_setBlocks.Length; i++)
		{
			MapBlock mapBlock = m_setBlocks[i];

			// �㏑���h�~
			if (m_blocks.ContainsKey(mapBlock.mapName))
				continue;

			// �u���b�N�̎�ސݒ�
			m_blocks[mapBlock.mapName] = mapBlock.blockType;
		}

		// �v���C���[���ݒ肳��Ă���ΐ���
		if (m_player != null)
		{
			Instantiate(m_player);
		}

		// SCV�ǂݍ���
		GenerateSCV();

		// �_���W�����炵���_���W��������
		//GenerateRoom();

	}

	// Update is called once per frame
	void Update()
	{

	}

	// �_���W�����̐���
	private void Generate(List<List<string>> mapList)
	{
		// �ǂ݂������f�[�^�����ƂɃ_���W��������������
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				string name = mapList[y][x];

				// �L�[�����݂��Ȃ��ꍇ�͉����������Ȃ�
				if (!m_blocks.ContainsKey(name))
					continue;

				// �������W
				Vector3 pos = new(x, y, 0.0f);

				// �u���b�N�̐���
				GameObject block = m_blockGenerator.GenerateBlock(m_blocks[name], pos);

			}
		}

	}

	// CSV�ǂݍ��݂̐���
	private void GenerateSCV()
	{
		// �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
		if (!m_dungeonData)
			return;

		// �}�b�v�̃��X�g
		List<List<string>> mapList = new ();

		// ���s��؂�œǂݏo��
		foreach (string line in m_dungeonData.ToString().Split("\n"))
		{
			// �s�����݂��Ȃ���΃��[�v�𔲂���
			if (line == "")
				break;

			string lin = line.Remove(line.Length - 1);

			List<string> list = new ();

			// �J���}��؂�œǂݏo��
			foreach (string line2 in lin.Split(","))
			{
				list.Add(line2);
			}

			mapList.Add(list);
		}

		// �}�b�v�̐���
		Generate(mapList);

	}


	struct Room
	{
		int topLeftX;   // ������WX
		int topLeftY;   // ������WY
		int width;		// ��
		int height;		// ����

	}

	// ���������_���W��������
	private void GenerateRoom()
	{
		// �}�b�v
		List<List<string>> mapList = new ();

		// �}�b�v�̏�����
		for (int y = 0; y < m_dungeonSizeY; y++)
		{
			mapList.Add(new List<string>());

			for (int x = 0; x < m_dungeonSizeX; x++)
			{
				// �ʏ�u���b�N�Ŗ��߂�
				mapList[y].Add("1");
			}
		}

		// �����̐�����(5 ~ 10)
		int generateRoomCount = Random.Range(5, 10);



	}

}
