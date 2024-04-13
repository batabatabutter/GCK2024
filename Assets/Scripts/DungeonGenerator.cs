using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class DungeonGenerator : MonoBehaviour
{
    [Header("���邳������(�f�o�b�O)")]
    [SerializeField] private bool m_isBrightness;

    //"�X�e�[�W(0�`)
    private int m_stageNum;

    [Header("�_���W�����f�[�^�x�[�X")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

    [System.Serializable]
    public class BlockOdds
    {
        [Header("���")]
        public BlockData.BlockType type;       // ���
        [Header("�m��")]
        public int odds;       // �m��
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

    [Header("�_���W���������X�N���v�g")]
    [SerializeField] private TestDungeonGenerator1 m_dungeonGeneratorDig = null;


    private void Awake()
    {
        m_blockGenerator = GetComponent<BlockGenerator>();
        m_parent = new GameObject("Blocks");
    }

    /// <summary>
    /// �X�e�[�W�쐬
    /// </summary>
    public void CreateStage()
    {
        // �_���W�����̃f�[�^�擾
        DungeonData dungeonData = m_dungeonDataBase.dungeonDatas[m_stageNum];

        // �_���W�����̃T�C�Y�擾
        Vector2 dungeonSize = dungeonData.dungeonSize;

        // �����p�^�[���擾
        DungeonData.Pattern pattern = dungeonData.pattern;

        // �����p�^�[������
        switch (pattern)
        {
            case DungeonData.Pattern.TEN_X_TEN:
                Generate10to10(dungeonData, dungeonSize);
                break;

            case DungeonData.Pattern.DIGGING:
                GenerateDigging(dungeonData);
                break;
        }


    }

    void Generate10to10(DungeonData dungeonData, Vector2 size)
    {
		// �R�A�̐������W����
		m_corePos.x = Random.Range(0, (int)size.x * 10);
		m_corePos.y = Random.Range(0, (int)size.y * 10);

		int roop_error = 0;

		//�v���C���[�ƃR�A�̈ʒu�������܂ŌJ��Ԃ�
		do
		{
			m_playerPos = new Vector2(Random.Range(0, (int)size.x * 10), Random.Range(0, (int)size.y * 10));

			if (roop_error > 100)
			{
				Debug.Log("�R�A�ƃv���C���[���߂����܂��B�Ԋu���������Ă�������");
				break;
			}
			roop_error++;

		}
		while (
		m_playerPos.x < m_corePos.x + m_playerLength &&
		m_playerPos.x > m_corePos.x - m_playerLength &&
		m_playerPos.y < m_corePos.y + m_playerLength &&
		m_playerPos.y > m_corePos.y - m_playerLength
		);

		//  �v���C���[�̐���
		GameObject pl = Instantiate<GameObject>(m_player, m_playerPos, Quaternion.identity);

		// core�̐���
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y), null, m_isBrightness);


		//  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
		if (m_playSceneManager == null)
			Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:DungeonManager");
		else
		{
			m_playSceneManager.SetPlayer(pl);
			m_playSceneManager.SetCore(co);
		}

		//�P�O���P�O�̃��X�g�Ǘ��p
		List<List<List<string>>> mapListManager = new ();

		//�f�[�^�x�[�X���烊�X�g�̎擾
		List<TextAsset> dungeonCSV = dungeonData.dungeonCSV;

		for (int i = 0; i < dungeonCSV.Count; i++)
		{
			// �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
			if (dungeonCSV[i] == null)
			{
				Debug.Log("CSV file is not assigned");
				return;

			}

			// �}�b�v�̃��X�g
			List<List<string>> mapList = new ();

			// �t�@�C���̓��e��1�s������
			string[] lines = dungeonCSV[i].text.Split('\n');
			foreach (string line in lines)
			{
				string[] values = line.Split(',');

				// �e�s�̃f�[�^���i�[���郊�X�g
				List<string> rowData = new ();

				// �e��̒l����������
				foreach (string value in values)
				{
					// �f�[�^�����X�g�ɒǉ�
					rowData.Add(value);
				}

				// �s�̃f�[�^��CSV�f�[�^�ɒǉ�
				mapList.Add(rowData);
			}


			//�R�����ɓ����
			mapListManager.Add(mapList);

		}

		//�u���b�N����
		for (int y = 0; y < (int)size.y; y++)
		{
			for (int x = 0; x < (int)size.x; x++)
			{
				int random = Random.Range(0, dungeonCSV.Count);
				Make10_10Block(mapListManager[random], x * 10, y * 10);

			}
		}

		//��Ղň͂�
		for (int i = 0; i < (int)size.y * 10; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3((int)size.y * 10, i, 0), m_parent.transform, m_isBrightness);
		}
		for (int i = 0; i < (int)size.x * 10; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, (int)size.y * 10, 0), m_parent.transform, m_isBrightness);
		}

		//�n�ʂ̐���
		for (int y = 0; y < (int)size.y * 10; ++y)
		{
			for (int x = 0; x < (int)size.x * 10; ++x)
			{
				// �������W
				Vector3 pos = new(x, y, 0.0f);

				// �u���b�N�̐���
				GameObject block = Instantiate(m_ground, pos, Quaternion.identity);

				block.transform.parent = m_parent.transform;

			}
		}

	}
	void Make10_10Block(List<List<string>> mapList, int originX, int originY)
    {
        // �ǂ݂������f�[�^�����ƂɃ_���W��������������
        for (int y = 0; y < mapList.Count; y++)
        {
            for (int x = 0; x < mapList[y].Count; x++)
            {
                //�v���C���[
                if ((int)m_playerPos.x == x + originX && (int)m_playerPos.y == y + originY)
                {
                    continue;
                }

                //�R�A�𐶐�
                if (m_corePos.x == x + originX && m_corePos.y == y + originY)
                {
                    continue;
                }
                // 0 �̏ꍇ�͉����������Ȃ�
                if (mapList[y][x] == "0" || mapList[y][x] == "")
                    continue;

                // �������W
                Vector3 pos = new(originX + x, originY + y, 0.0f);

                // �u���b�N�̐���
                m_blockGenerator.GenerateBlock(
                    m_dungeonDataBase.dungeonDatas[m_stageNum].dungeonOdds
                    [LotteryBlock()].type,
                    pos,
                    m_parent.transform,
                    m_isBrightness
                    );
            }
        }
    }

    void GenerateDigging(DungeonData dungeonData)
    {
		// �h���N���X�ɃL���X�g����
		DungeonDataDigging dig = dungeonData as DungeonDataDigging;
		// �L���X�g�ł��Ȃ�����
		if (dungeonData == null)
			return;

		// �_���W�����̃T�C�Y�擾
		Vector2Int dungeonSize = dig.Size;

		// �f�[�^�̐ݒ�
		m_dungeonGeneratorDig.SetDungeonData(dig);
		// �}�b�v�̎擾
		List<List<string>> mapList = m_dungeonGeneratorDig.GenerateDungeon(dungeonSize);

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
			m_playerPos = new Vector2(Random.Range(0.0f, dungeonSize.x), Random.Range(0.0f, dungeonSize.y));

			if (roop_error > 100)
			{
				Debug.Log("�R�A�ƃv���C���[���߂����܂��B�Ԋu���������Ă�������");
				break;
			}
			roop_error++;

		}
		// �R�A�ɋ߂����胋�[�v
		while (
		m_playerPos.x < m_corePos.x + m_playerLength &&
		m_playerPos.x > m_corePos.x - m_playerLength &&
		m_playerPos.y < m_corePos.y + m_playerLength &&
		m_playerPos.y > m_corePos.y - m_playerLength
		);

		//  �v���C���[�̐���
		GameObject pl = Instantiate(m_player, m_playerPos, Quaternion.identity);

		// core�̐���
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y), null, m_isBrightness);


		//  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
		if (m_playSceneManager == null)
			Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:DungeonManager");
		else
		{
			m_playSceneManager.SetPlayer(pl);
			m_playSceneManager.SetCore(co);
		}

        // �u���b�N����
        for (int y = 0; y < mapList.Count; y++)
        {
            for (int x = 0; x < mapList[y].Count; x++)
            {
				//�v���C���[
				if ((int)m_playerPos.x == x && (int)m_playerPos.y == y)
				{
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
                    // ����������
                    BlockData.BlockType type = dungeonData.dungeonOdds[LotteryBlock()].type;
                    // ����������W
                    Vector2 position = new(x, y);
                    // �u���b�N����
                    m_blockGenerator.GenerateBlock(type, position, m_parent.transform, m_isBrightness);
                }
			}
		}

		//��Ղň͂�
		for (int i = 0; i < dungeonSize.y; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(dungeonSize.y, i, 0), m_parent.transform, m_isBrightness);
		}
		for (int i = 0; i < dungeonSize.x; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, dungeonSize.y, 0), m_parent.transform, m_isBrightness);
		}

		//�n�ʂ̐���
		for (int y = 0; y < dungeonSize.y; ++y)
		{
			for (int x = 0; x < dungeonSize.x; ++x)
			{
				// �������W
				Vector3 pos = new(x, y, 0.0f);

				// �u���b�N�̐���
				GameObject block = Instantiate(m_ground, pos, Quaternion.identity);

				block.transform.parent = m_parent.transform;

			}
		}

	}

	int LotteryBlock()
    {
        //�m���̒��I
        List<int> oddsList = new List<int>();

        //�S�Ă̊m�����Z
        int allOdds = 0;

        List<BlockOdds> blockOddsList = m_dungeonDataBase.dungeonDatas[m_stageNum].dungeonOdds;


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
