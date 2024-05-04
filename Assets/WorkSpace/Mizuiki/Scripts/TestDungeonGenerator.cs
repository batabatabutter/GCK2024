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

	[Header("�R�A�̍��W")]
	[SerializeField] private Vector2Int m_corePosition = Vector2Int.zero;

	[System.Serializable]
	public struct BlockGenerateData
	{
		[Header("�u���b�N�̐����͈�(�R�A����̋���)")]
		public MyFunction.MinMax range;
		[Header("�u���b�N�̐�����")]
		[Range(0.0f, 1.0f)] public float rateMin;
		[Range(0.0f, 1.0f)] public float rateMax;
		[Header("�m�C�Y�̃X�P�[��"), Min(0.0f)]
		public float noiseScale;
	}

	[SerializeField] private BlockGenerateData[] m_generateBlocks;

	[Header("�m�C�Y�̃X�P�[��"), Range(0.0f, 1.0f)]
	[SerializeField] private float m_noiseScale = 1.0f;
	[Header("�z�΂̐�����")]
	[SerializeField] private float m_oreGenerateRate = 0.3f;
	[Header("�z�΂̐����͈�")]
	[SerializeField] private MyFunction.MinMax m_oreGenerateRange = new();
	[Header("�z�΂̐������͈̔�")]
	[SerializeField] private Vector2 m_oreGenerateRateRange = new();

	[Header("���C�g�t����")]
	[SerializeField] private bool m_light = true;

	[Header("�v���C���[(�g�����X�t�H�[���p)")]
	[SerializeField] private GameObject m_player = null;

	private List<List<BlockData.BlockType>> m_blockTypes = new();


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
		SetBlockType(mapList);
		Generate(m_blockTypes);

	}

	// �u���b�N�̎�ސݒ�
	private void SetBlockType(List<List<string>> mapList)
	{
		List<List<BlockData.BlockType>> types = new();

		for (int y = 0; y < mapList.Count; y++)
		{
			List<BlockData.BlockType> type = new();

			for (int x = 0; x < mapList[y].Count; x++)
			{
				string name = mapList[y][x];

				// �u���b�N�i�V
				if (name == "0")
				{
					type.Add(BlockData.BlockType.OVER);
				}
				// �u���b�N�A��
				else if (name == "1")
				{
					float dis = Vector2.Distance(m_corePosition, new Vector2(x, y));

					// �����͈͓�
					if (m_oreGenerateRange.Within(dis))
					{
						// �m�C�Y�̎擾
						float noise = Mathf.PerlinNoise(x * m_noiseScale, y * m_noiseScale);
						// �����͈͂̒����l
						float center = (m_oreGenerateRange.min + m_oreGenerateRange.max) / 2.0f;
						// �����͈͂̒�������̋���
						float centerDis = Mathf.Abs(center - dis);
						// �����̕�
						float wid = m_oreGenerateRange.max - m_oreGenerateRange.min;
						// ���[�v�̒l
						float t = 1.0f - (centerDis / (wid / 2.0f));
						// �������̎擾
						float rate = Mathf.Lerp(m_oreGenerateRateRange.x, m_oreGenerateRateRange.y, t);

						// �z��
						if (noise < rate)
						{
							type.Add(CreateOre());
						}
						// ��
						else
						{
							type.Add(CreateBlock());
						}
					}
					// �����͈͊O
					else
					{
						type.Add(CreateBlock());
					}
				}
				// �m��z��
				else if (name == "2")
				{
					type.Add(CreateOre());
				}
				// �O�̂��߂��̑�
				else
				{
					type.Add(BlockData.BlockType.OVER);
				}
			}
			// �ǉ�
			types.Add(type);
		}

		m_blockTypes = types;
	}

	// �_���W�����̐���
	private void Generate(List<List<BlockData.BlockType>> mapList)
	{
		// �ǂ݂������f�[�^�����ƂɃ_���W��������������
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				BlockData.BlockType name = mapList[y][x];

				// �������W
				Vector3 pos = new(x, y, 0.0f);

				//// �L�[�����݂��Ȃ��ꍇ�͒n�ʂ���
				//if (!m_blocks.ContainsKey(name))
				//{
				//	m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, pos, null, m_light);
				//	continue;
				//}

				// �u���b�N�̐���
				m_blockGenerator.GenerateBlock(name, pos, null, m_light);

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

	// �u���b�N
	private BlockData.BlockType CreateBlock()
	{
		return BlockData.BlockType.STONE;
	}

	// �z��
	private BlockData.BlockType CreateOre()
	{
		int rand = Random.Range((int)BlockData.BlockType.ORE_BEGIN + 1, (int)BlockData.BlockType.ORE_END);

		return BlockData.BlockType.COAL;
		//return (BlockData.BlockType)rand;
	}

}
