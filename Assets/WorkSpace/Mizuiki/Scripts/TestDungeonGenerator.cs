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

	[Header("�u���b�N�W�F�l���[�^")]
	[SerializeField] private BlockGenerator m_blockGenerator;

	[Header("���������̃C���f�b�N�X")]
	[SerializeField] private int m_dungeonIndex = 0;
	[Header("�����X�N���v�g�̔z��")]
	[SerializeField] private DungeonGeneratorBase[] m_dungeonGenerators;

	[Header("�R�A�̍��W")]
	[SerializeField] private Vector2Int m_corePosition = Vector2Int.zero;


	[System.Serializable]
	public struct Blocks
	{
		public BlockData.BlockType type;
		public DungeonGenerator.BlockGenerateData data;
	}
	[SerializeField] private Blocks[] m_generateBlocks;
	private readonly Dictionary<BlockData.BlockType, DungeonGenerator.BlockGenerateData> m_blocks = new();

	[Header("���C�g�t����")]
	[SerializeField] private bool m_light = true;

	[Header("�v���C���[(�g�����X�t�H�[���p)")]
	[SerializeField] private GameObject m_player = null;

	private List<List<BlockData.BlockType>> m_blockTypes = new();


	// Start is called before the first frame update
	void Start()
	{
		// �u���b�N�̐ݒ�
		for (int i = 0; i < m_generateBlocks.Length; i++)
		{
			Blocks block = m_generateBlocks[i];

			// �㏑���h�~
			if (m_blocks.ContainsKey(block.type))
				continue;

			// �u���b�N�̎�ސݒ�
			m_blocks[block.type] = block.data;
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
		List<List<BlockData.BlockType>> typeLists = new();

		for (int y = 0; y < mapList.Count; y++)
		{
			List<BlockData.BlockType> typeList = new();

			for (int x = 0; x < mapList[y].Count; x++)
			{
				string name = mapList[y][x];

				// �u���b�N�i�V
				if (name == "0")
				{
					typeList.Add(BlockData.BlockType.OVER);
				}
				// �u���b�N�A��
				else if (name == "1")
				{
					// ��������u���b�N�̎��
					BlockData.BlockType type = BlockData.BlockType.STONE;
					// �����u���b�N��ޕ����[�v
					foreach (DungeonGenerator.BlockGenerateData blocks in m_blocks.Values)
					{
						// �u���b�N�𐶐�����
						if (GenerateBlock(new Vector2(x, y), blocks))
						{
							// ��������ꍇ�͏㏑�����Ă���
							type = blocks.blockType;
						}
					}
					// �ŏI�I�Ȍ��ʂ𐶐��u���b�N�Ƃ��Ēǉ�
					typeList.Add(type);
				}
				// �m��z��
				else if (name == "2")
				{
					typeList.Add(CreateOre());
				}
				// �O�̂��߂��̑�
				else
				{
					typeList.Add(BlockData.BlockType.OVER);
				}
			}
			// �ǉ�
			typeLists.Add(typeList);
		}

		m_blockTypes = typeLists;
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

	// �u���b�N�̏�񐶐�
	private bool GenerateBlock(Vector2 pos, DungeonGenerator.BlockGenerateData data)
	{
		float dis = Vector2.Distance(m_corePosition, pos);

		// �����͈͓�
		if (data.range.Within(dis))
		{
			// �m�C�Y�̎擾
			float noise = Mathf.PerlinNoise(pos.x * data.noiseScale, pos.y * data.noiseScale);
			// �����͈͂̒����l
			float center = (data.range.min + data.range.max) / 2.0f;
			// �����͈͂̒�������̋���
			float centerDis = Mathf.Abs(center - dis);
			// �����̕�
			float wid = data.range.max - data.range.min;
			// ���[�v�̒l
			float t = 1.0f - (centerDis / (wid / 2.0f));
			// �������̎擾
			float rate = Mathf.Lerp(data.rateMin, data.rateMax, t);

			// �z��
			if (noise < rate)
			{
				return true;
			}
			// ��
			else
			{
				return false;
			}
		}
		// �����͈͊O
		else
		{
			return false;
		}
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

		return (BlockData.BlockType)rand;
	}

}
