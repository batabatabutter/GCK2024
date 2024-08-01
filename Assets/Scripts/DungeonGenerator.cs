using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
	[System.Serializable]
	public struct BlockGenerateData
	{
		[Header("��������u���b�N�̎��")]
		public BlockData.BlockType blockType;
		[Header("�u���b�N�̐����͈�(�R�A����̋���)")]
		public MyFunction.MinMax range;
		[Header("�u���b�N�̐�����")]
		[Range(0.0f, 1.0f)] public float rateMin;
		[Range(0.0f, 1.0f)] public float rateMax;
		[Header("�m�C�Y�̃X�P�[��"), Min(0.0f), Tooltip("�l���傫���قǍׂ����Ȃ�")]
		public float noiseScale;
		// �m�C�Y�̃I�t�Z�b�g
		public float offset;
	}

    // �X�e�[�W�ԍ�(0�`)
    private int m_stageNum;

    [Header("�_���W�����f�[�^�x�[�X")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

	[Header("�u���b�N�����X�N���v�g")]
    [SerializeField] private BlockGenerator m_blockGenerator;
	[Header("�j����v���C���[�̏o�����Ȃ�����")]
    [SerializeField] private int m_playerLength = 35;
    [Header("�v���C���[")]
    [SerializeField] private GameObject m_playerPrefab;
    //[Header("�v���C�V�[���}�l�[�W���[")]
    //[SerializeField] private PlaySceneManager m_playSceneManager;

    [Header("�n��")]
    [SerializeField] private GameObject m_ground;

    //�R�A�̈ʒu
    private Vector2Int m_corePos;
    //�v���C���[�̈ʒu
    private Vector2 m_playerPos;

	// �R�A
	private Block m_dungeonCore;

	// �u���b�N�̔z��
	private Block[,] m_blocks;


	[Header("�_���W���������X�N���v�g")]
    [SerializeField] private List<DungeonGeneratorBase> m_generators = null;
	private readonly Dictionary<DungeonData.Pattern, DungeonGeneratorBase> m_dungeonGenerators = new();




	/// <summary>
	/// �X�e�[�W�쐬
	/// </summary>
	public void CreateStage(int stageNum)
    {
        // �u���b�N�W�F�l���[�^�̎擾
        m_blockGenerator = GetComponent<BlockGenerator>();

		if (m_dungeonGenerators.Count == 0)
		{
			// �_���W���������N���X
			foreach (DungeonGeneratorBase generator in m_generators)
			{
				// �㏑���h�~
				if (m_dungeonGenerators.ContainsKey(generator.Pattern))
					continue;
				// �W�F�l���[�^�̐ݒ�
				m_dungeonGenerators[generator.Pattern] = generator;
			}
		}

        // �X�e�[�W�ԍ��̐ݒ�
        m_stageNum = stageNum;

        // �_���W�����̃f�[�^�擾
        DungeonData dungeonData = m_dungeonDataBase.dungeonDatas[m_stageNum];

        // �����p�^�[���擾
        DungeonData.Pattern pattern = dungeonData.DungeonPattern;

		// �_���W�����̃T�C�Y
		Vector2Int dungeonSize = new(dungeonData.Size.x, dungeonData.Size.y);
		// �u���b�N�z��̃T�C�Y����
		m_blocks = new Block[dungeonSize.y, dungeonSize.x];

		// �Z�[�u�f�[�^�擾
		SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;

		// �Z�[�u�f�[�^�����݂��Ă���
		if (saveData)
		{
			// �_���W�����̃��x���擾
			int dungeonLevel = saveData.DungeonStates[stageNum].clearCount[0];
		}

		// �_���W�����̃}�b�v�擾
		List<List<string>> mapList = m_dungeonGenerators[pattern].GenerateDungeon(dungeonData);

		// �R�A�̐������W����(�������[�v�ɂȂ�Ȃ��悤�ɉ񐔐���������)
		for (int i = 0; i < 1000000; i++)
		{
			// �R�A�̐����ʒu�������_���Ŏ擾
			Vector2Int pos = new(Random.Range(0, dungeonSize.x), Random.Range(0, dungeonSize.y));
			// �����ʒu���u���b�N�Ȃ�ݒ肵�ă��[�v�𔲂���
			if (mapList[pos.y][pos.x] == "1")
			{
				m_corePos = pos;
				break;
			}
		}

		int roop_error = 0;

		//�v���C���[�ƃR�A�̈ʒu�������܂ŌJ��Ԃ�
		do
		{
			m_playerPos = new Vector2(Random.Range(0, dungeonSize.x), Random.Range(0, dungeonSize.y));

			if (roop_error > 100)
			{
				Debug.Log("�R�A�ƃv���C���[���߂����܂��B�Ԋu���������Ă�������");
				break;
			}
			roop_error++;

		}
		// �R�A�ɋ߂����胋�[�v
		while (MyFunction.DetectCollision(m_playerPos, m_corePos, new Vector2(m_playerLength, m_playerLength))
		);

		// core�̐���
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y));
		// �X�v���C�g�̐ݒ�
		co.GetComponent<SpriteRenderer>().sprite = dungeonData.CoreSprite;
		// �R�A�ݒ�
		m_dungeonCore = co.GetComponent<Block>();
		// �u���b�N�z��ɑ��
		m_blocks[m_corePos.y, m_corePos.x] = m_dungeonCore;

		// �u���b�N����
		GenerateBlock(mapList, dungeonData.BlockGenerateData);

		//��Ղň͂�
		CreateBedrock(dungeonSize);

	}

	// �v���C���[�̃g�����X�t�H�[���ݒ�
	public void SetPlayerTransform(Transform player)
	{
		m_blockGenerator.SetPlayerTransform(player);
	}

	//public PlaySceneManager PlaySceneManager
	//{
	//	set { m_playSceneManager = value; }
	//}

	// �R�A
	public Block DungeonCore
	{
		get { return m_dungeonCore; }
	}
	// �v���C���[�̈ʒu
	public Vector3 PlayerPosition
	{
		get { return m_playerPos; }
	}
	// �u���b�N
	public Block[,] Blocks
	{
		get { return m_blocks; }
	}



	// �u���b�N�̏�񐶐�
	private bool IsCreateBlock(Vector2 pos, BlockGenerateData data)
	{
		float dis = Vector2.Distance(m_corePos, pos);

		// �����͈͓�
		if (data.range.Within(dis))
		{
			// �m�C�Y�̎擾
			float noise = MyFunction.GetNoise(pos * data.noiseScale, data.offset);
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

	// �u���b�N�̐���
	private void GenerateBlock(List<List<string>> mapList, BlockGenerateData[] blockGenerateData)
	{
		// �u���b�N�����p�̃����_���ȃI�t�Z�b�g�ݒ�
		for (int i = 0; i < blockGenerateData.Length; i++)
		{
			blockGenerateData[i].offset = Random.value;
		}

		// �u���b�N����
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				// ����������W
				Vector2 position = new(x, y);
				// ���������u���b�N
				Block block = null;
				//�v���C���[
				if ((int)m_playerPos.x == x && (int)m_playerPos.y == y)
				{
					// ��̃u���b�N�𐶐�
					m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position);
					continue;
				}
				//�R�A�𐶐�
				if (m_corePos.x == x && m_corePos.y == y)
				{
					continue;
				}
				// �u���b�N�̐����ʒu
				if (mapList[y][x] == "1")
				{
					// ��������u���b�N�̎��
					BlockData.BlockType type = CreateBlockType(blockGenerateData, new Vector2(x, y));
					// �u���b�N����
					block = m_blockGenerator.GenerateBlock(type, position).GetComponent<Block>();
				}
				else
				{
					// ��̃u���b�N�𐶐�
					m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position);
				}
				// �u���b�N�z��ɑ��
				m_blocks[y, x] = block;
			}
		}
	}
	public void GenerateBlock(List<List<BlockData.BlockType>> blocks)
	{
		for (int y = 0; y < blocks.Count; y++)
		{
			for (int x = 0; x < blocks[y].Count; x++)
			{
				// ���
				BlockData.BlockType type = blocks[y][x];
				// �ʒu
				Vector2 pos = new(x, y);

				// �����u���b�N
				GameObject block = m_blockGenerator.GenerateBlock(type, pos);
				m_blocks[y, x] = block.GetComponent<Block>();
			}
		}
	}

	// ��������u���b�N
	private BlockData.BlockType CreateBlockType(BlockGenerateData[] blockGenerateData, Vector2 pos)
	{
		// ��������u���b�N�̎��
		BlockData.BlockType type = BlockData.BlockType.STONE;
		// �����u���b�N��ޕ����[�v
		foreach (BlockGenerateData data in blockGenerateData)
		{
			// �u���b�N�𐶐�����
			if (IsCreateBlock(pos, data))
			{
				// ��������ꍇ�͏㏑�����Ă���
				type = data.blockType;
			}
		}
		// �ŏI�I�Ȑ����u���b�N�̎�ނ�Ԃ�
		return type;
	}

	// ��Ղ̐���
	private void CreateBedrock(Vector2Int size)
	{
		//��Ղň͂�
		for (int x = 0; x < size.x; x++)
		{
			// �㉺
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, -1    , 0));
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, size.y, 0));
		}
		for (int y = 0; y < size.y; y++)
		{
			// ���E
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1    , y, 0));
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(size.y, y, 0));
		}
	}

	// �X�e�[�W�ԍ��擾
	public int GetStageNum()
    {
        return m_stageNum;
    }
    //  �X�e�[�W�ԍ��ݒ�
    public bool SetStageNum(int num)
    {
        //  �X�e�[�W�ԍ����͈͊O��������G���[
        if (num < 0 || num >= m_dungeonDataBase.dungeonDatas.Count)
        {
            Debug.Log("�X�e�[�W�����݂��Ȃ��ԍ��ł��Bnum = " + num);
            m_stageNum = int.MaxValue;
            return false;
        }
        else
        {
            m_stageNum = num;
            return true;
        }
    }

	// �_���W�����f�[�^�擾
	public DungeonData GetDungeonData()
	{
		return m_dungeonDataBase.dungeonDatas[m_stageNum];
	}
}
