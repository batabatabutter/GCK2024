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

	[Header("�`�����N�@�\�L��")]
	[SerializeField] private bool m_isChunk = true;
	[Header("�`�����N�̃T�C�Y")]
	[SerializeField] private int m_chunkSize = 10;
	[Header("�\���`�����N��")]
	[SerializeField] private int m_activeChunk = 5;

	[Header("�u���b�N�W�F�l���[�^")]
	[SerializeField] private BlockGenerator m_blockGenerator;

	[Header("���������̃C���f�b�N�X")]
	[SerializeField] private int m_dungeonIndex = 0;
	[Header("�����X�N���v�g�̔z��")]
	[SerializeField] private DungeonGeneratorBase[] m_dungeonGenerators;

	[Header("�R�A�̍��W")]
	[SerializeField] private Vector2Int m_corePosition = Vector2Int.zero;

	[Header("�e�u���b�N�̐������")]
	[SerializeField] private DungeonGenerator.BlockGenerateData[] m_generateBlocks;
	// �C���X�y�N�^�[�Őݒ肵���f�[�^�������z��ɂ���
	private readonly Dictionary<BlockData.BlockType, DungeonGenerator.BlockGenerateData> m_blocks = new();

	[Header("�v���C���[(�g�����X�t�H�[���p)")]
	[SerializeField] private GameObject m_player = null;

	// ��������u���b�N�̎�ލs��
	private List<List<BlockData.BlockType>> m_blockTypes = new();

	// ���������u���b�N�̏��
	private readonly List<Block> m_objectBlock = new();

	// �`�����N�̓񎟌��z��
	private List<List<GameObject>> m_chunk = new();



	void Start()
	{
		// �u���b�N�̐ݒ�
		for (int i = 0; i < m_generateBlocks.Length; i++)
		{
			DungeonGenerator.BlockGenerateData block = m_generateBlocks[i];

			// �㏑���h�~
			if (m_blocks.ContainsKey(block.blockType))
				continue;

			// �u���b�N�̎�ސݒ�
			m_blocks[block.blockType] = block;
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

		if (m_player.TryGetComponent(out SearchBlock search))
		{
			search.SetSearchBlocks(m_objectBlock);
		}
	}

	private void Update()
	{
		// �`�����N�@�\����
		if (m_isChunk == false)
		{
			return;
		}

		Vector2Int playerChunk = new((int)m_player.transform.position.x / m_chunkSize, (int)m_player.transform.position.y / m_chunkSize);

		for (int y = 0; y < m_chunk.Count; y++)
		{
			for (int x = 0; x < m_chunk[y].Count; x++)
			{
				// �v���C���[�`�����N�Ƃ̋���
				float distance = Vector2Int.Distance(playerChunk, new Vector2Int(x, y));

				// �\���`�����N��
				if (distance < m_activeChunk)
				{
					if (m_chunk[y][x].activeSelf == false)
					{
						m_chunk[y][x].SetActive(true);
					}
				}
				// �\���`�����N�O
				else
				{
					if (m_chunk[y][x].activeSelf == true)
					{
						m_chunk[y][x].SetActive(false);
					}
				}
			}
		}

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
		// �`�����N�̐���
		for (int y = 0; y < mapList.Count / m_chunkSize; y++)
		{
			m_chunk.Add(new());
			for (int x = 0; x < mapList[y].Count / m_chunkSize; x++)
			{
				m_chunk[y].Add(new GameObject("(" + x + ", " + y + ")"));
			}
		}

		// �ǂ݂������f�[�^�����ƂɃ_���W��������������
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				BlockData.BlockType name = mapList[y][x];

				// �������W
				Vector3 pos = new(x, y, 0.0f);

				GameObject obj;

				// �R�A�̐���
				if (new Vector2Int(x, y) == m_corePosition)
				{
					obj = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, pos, m_chunk[y / m_chunkSize][x / m_chunkSize].transform);

				}
				// �u���b�N�̐���
				else
				{
					obj = m_blockGenerator.GenerateBlock(name, pos, m_chunk[y / m_chunkSize][x / m_chunkSize].transform);
				}

				// �u���b�N������Βǉ�
				if (obj.TryGetComponent(out Block block))
				{
					m_objectBlock.Add(block);
				}


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


	// �z��
	private BlockData.BlockType CreateOre()
	{
		int rand = Random.Range((int)BlockData.BlockType.ORE_BEGIN + 1, (int)BlockData.BlockType.ORE_END);

		return (BlockData.BlockType)rand;
	}

}
