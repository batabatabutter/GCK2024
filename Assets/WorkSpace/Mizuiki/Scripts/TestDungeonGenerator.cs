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
	[SerializeField] private Vector2Int m_dungeonSize;

	[System.Serializable]
	struct MapBlock
	{
		public string mapName;
		public BlockData.BlockType blockType;
	}

	[Header("�����u���b�N")]
	[SerializeField] private MapBlock[] m_setBlocks;
	private readonly Dictionary<string, BlockData.BlockType> m_blocks = new();

	[Header("�u���b�N�W�F�l���[�^")]
	[SerializeField] private BlockGenerator m_blockGenerator;

	[Header("���������̃C���f�b�N�X")]
	[SerializeField] private int m_dungeonIndex = 0;
	[Header("�����X�N���v�g�̔z��")]
	[SerializeField] private DungeonGeneratorBase[] m_dungeonGenerators;

	[Header("���C�g�ƕt����")]
	[SerializeField] private bool m_light = true;

	[Header("�v���C���[(�g�����X�t�H�[���p)")]
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
			//Instantiate(m_player);
			m_blockGenerator.SetPlayerTransform(m_player.transform);
		}

		List<List<string>> mapList;

		// ����
		if (m_dungeonGenerators.Length > 0)
		{
			mapList = m_dungeonGenerators[m_dungeonIndex].GenerateDungeon(m_dungeonSize);
		}
		else
		{
			// SCV�ǂݍ���
			mapList = GenerateSCV();
		}

		// �}�b�v�̐���
		Generate(mapList);

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

				// �������W
				Vector3 pos = new(x, y, 0.0f);

				// �L�[�����݂��Ȃ��ꍇ�͒n�ʂ���
				if (!m_blocks.ContainsKey(name))
				{
					m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, pos, null, m_light);
					continue;
				}

				// �u���b�N�̐���
				m_blockGenerator.GenerateBlock(m_blocks[name], pos, null, m_light);

			}
		}

	}

	// CSV�ǂݍ��݂̐���
	private List<List<string>> GenerateSCV()
	{
		// �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
		if (!m_dungeonData)
			return new();

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

		return mapList;
	}


}
