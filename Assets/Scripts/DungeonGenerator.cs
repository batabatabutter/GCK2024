using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class DungeonGenerator : MonoBehaviour
{
    [Header("���邳������(�f�o�b�O)")]
    [SerializeField] private bool m_isBlockBrightness;
    [SerializeField] private bool m_isGroundBrightness;

    //"�X�e�[�W(0�`)
    private int m_stageNum;

    [Header("�_���W�����f�[�^�x�[�X")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

	private GameObject[,] m_blocks = null;
	// �u���b�N�̎�ނ��Ƃ̃��X�g
	private Dictionary<BlockData.BlockType, List<Block>> m_blocksList;

    [System.Serializable]
    public class BlockOdds
    {
        [Header("���")]
        public BlockData.BlockType type;       // ���
        [Header("�m��")]
        public int odds;       // �m��
    }
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

	[Header("�j����v���C���[�̏o�����Ȃ�����")]
    [SerializeField] private int m_playerLength = 35;
    [Header("�v���C���[")]
    [SerializeField] private GameObject m_player;
    [Header("�v���C�V�[���}�l�[�W���[")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    [Header("�n�ʔw�i")]
    [SerializeField] private GameObject m_ground;

    //�R�A�̈ʒu
    private Vector2Int m_corePos;
    //�v���C���[�̈ʒu
    private Vector2 m_playerPos;
    //�u���b�N�̐e
    private GameObject m_parent;

    //�u���b�N�����X�N���v�g
    private BlockGenerator m_blockGenerator;

	[System.Serializable]
	public struct Generator
	{
		public DungeonData.Pattern pattern;
		public DungeonGeneratorBase generator;
	}

	[Header("�_���W���������X�N���v�g")]
    [SerializeField] private Generator[] m_generators = null;
	private readonly Dictionary<DungeonData.Pattern, DungeonGeneratorBase> m_dungeonGenerators = new();


    private void Awake()
    {
        m_blockGenerator = GetComponent<BlockGenerator>();
        m_parent = new GameObject("Blocks");

		foreach (Generator generator in m_generators)
		{
			// �㏑���h�~
			if (m_dungeonGenerators.ContainsKey(generator.pattern))
				continue;
			// �W�F�l���[�^�̐ݒ�
			m_dungeonGenerators[generator.pattern] = generator.generator;
		}
    }

    /// <summary>
    /// �X�e�[�W�쐬
    /// </summary>
    public void CreateStage()
    {
        // �_���W�����̃f�[�^�擾
        DungeonData dungeonData = m_dungeonDataBase.dungeonDatas[m_stageNum];

        // �����p�^�[���擾
        DungeonData.Pattern pattern = dungeonData.DungeonPattern;

		// �_���W�����̃}�b�v�擾
		List<List<string>> mapList = m_dungeonGenerators[pattern].GenerateDungeon(dungeonData);

		// �_���W�����̃T�C�Y
		Vector2Int dungeonSize = new(mapList[0].Count, mapList.Count);

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
		//m_playerPos.x < m_corePos.x + m_playerLength &&
		//m_playerPos.x > m_corePos.x - m_playerLength &&
		//m_playerPos.y < m_corePos.y + m_playerLength &&
		//m_playerPos.y > m_corePos.y - m_playerLength
		);

		//  �v���C���[�̐���
		GameObject pl = Instantiate(m_player, m_playerPos, Quaternion.identity);
		m_blockGenerator.SetPlayerTransform(pl.transform);

		// core�̐���
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y), null, m_isBlockBrightness, m_isGroundBrightness);


		//  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
		if (m_playSceneManager == null)
		{
			Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:DungeonManager");
		}
		else
		{
			m_playSceneManager.SetPlayer(pl);
			m_playSceneManager.SetCore(co);
		}

		// �u���b�N����
		CreateBlock(mapList, dungeonData.BlockGenerateData);

		//��Ղň͂�
		CreateBedrock(dungeonSize);

		//�n�ʂ̐���
		//CreateGround(dungeonSize);

    }




	private int LotteryBlock(List<BlockOdds> blockOddsList)
    {
        //�m���̒��I
        List<int> oddsList = new ();

        //�S�Ă̊m�����Z
        int allOdds = 0;

        //�u���b�N�̎�ނ̐�
        for (int i = 0; i < blockOddsList.Count; i++)
        {
            //�u���b�N�̊m��
            for (int j = 0; j < blockOddsList[i].odds; j++)
            {
                oddsList.Add(i);
            }
            //�u���b�N�̊m�������Z
            allOdds += blockOddsList[i].odds;
        }
        //���I
        return oddsList[Random.Range(0, allOdds)];
    }

	// �u���b�N�̏�񐶐�
	private bool GenerateBlock(Vector2 pos, BlockGenerateData data)
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
	private void CreateBlock(List<List<string>> mapList, BlockGenerateData[] blockGenerateData)
	{
		// �u���b�N�����p�̃����_���ȃI�t�Z�b�g�ݒ�
		for (int i = 0; i < blockGenerateData.Length; i++)
		{
			blockGenerateData[i].offset = Random.value;
		}

		// �����u���b�N�z��
		m_blocks = new GameObject[mapList.Count, mapList[0].Count];

		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				// ����������W
				Vector2 position = new(x, y);
				//�v���C���[
				if ((int)m_playerPos.x == x && (int)m_playerPos.y == y)
				{
					// ��̃u���b�N�𐶐�
					m_blocks[y,x] = m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position, m_parent.transform, m_isBlockBrightness, m_isGroundBrightness);
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
					BlockData.BlockType type = CreateBlock(blockGenerateData, new Vector2(x, y));
					// �u���b�N����
					m_blocks[y, x] = m_blockGenerator.GenerateBlock(type, position, m_parent.transform, m_isBlockBrightness, m_isGroundBrightness);
					// �u���b�N���X�g�ɒǉ�
					//m_blocksList[type].Add(m_blocks[y, x].GetComponent<Block>());
				}
				else
				{
					// ��̃u���b�N�𐶐�
					m_blocks[y, x] = m_blockGenerator.GenerateBlock(BlockData.BlockType.OVER, position, m_parent.transform, m_isBlockBrightness, m_isGroundBrightness);
				}
			}
		}

	}

	// ��������u���b�N
	private BlockData.BlockType CreateBlock(BlockGenerateData[] blockGenerateData, Vector2 pos)
	{
		// ��������u���b�N�̎��
		BlockData.BlockType type = BlockData.BlockType.STONE;
		// �����u���b�N��ޕ����[�v
		foreach (BlockGenerateData data in blockGenerateData)
		{
			// �u���b�N�𐶐�����
			if (GenerateBlock(pos, data))
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
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, -1    , 0), m_parent.transform, m_isBlockBrightness, m_isGroundBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, size.y, 0), m_parent.transform, m_isBlockBrightness, m_isGroundBrightness);
		}
		for (int y = 0; y < size.y; y++)
		{
			// ���E
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1    , y, 0), m_parent.transform, m_isBlockBrightness, m_isGroundBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(size.y, y, 0), m_parent.transform, m_isBlockBrightness, m_isGroundBrightness);
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
}
