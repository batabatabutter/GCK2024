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

	[Header("�v���C���[(�g�����X�t�H�[���p)")]
	[SerializeField] private GameObject m_player = null;

	[Header("�_���W�����A�^�b�J�[(�R�A�ݒ�p)")]
	[SerializeField] private DungeonAttacker m_dungeonAttacker = null;

	// ��������u���b�N�̎�ލs��
	private List<List<BlockData.BlockType>> m_blockTypes = new();

	// ���������u���b�N�̏��
	private readonly List<Block> m_objectBlock = new();



	void Start()
	{
		// �v���C���[�̃g�����X�t�H�[���ݒ�
		if (m_player != null)
		{
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

	// �u���b�N�̎�ސݒ�
	private void SetBlockType(List<List<string>> mapList)
	{
		// �u���b�N�̎�ނ̃��X�g
		List<List<BlockData.BlockType>> typeLists = new();

		// �u���b�N�����p�̃����_���ȃI�t�Z�b�g�ݒ�
		for (int i = 0; i < m_generateBlocks.Length; i++)
		{
			m_generateBlocks[i].offset = Random.value;
		}

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
					foreach (DungeonGenerator.BlockGenerateData blocks in m_generateBlocks)
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

				GameObject obj;

				// �R�A�̐���
				if (new Vector2Int(x, y) == m_corePosition)
				{
					obj = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, pos);
					m_dungeonAttacker.CorePosition = obj.transform;
				}
				// �u���b�N�̐���
				else
				{
					obj = m_blockGenerator.GenerateBlock(name, pos);
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
			float noise = Mathf.PerlinNoise((pos.x * data.noiseScale) + data.offset, (pos.y * data.noiseScale) + data.offset);
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
