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
    private int m_corePosX;
    private int m_corePosY;
    //�v���C���[�̈ʒu
    private Vector2 m_playerPos;
    //�u���b�N�̐e
    private GameObject m_parent;

    //�u���b�N�����X�N���v�g
    private BlockGenerator m_blockGenerator;

    [Header("�_���W���������X�N���v�g")]
    [SerializeField] private TestDungeonGenerator1 m_dungeonGeneratorDig = new();


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

        // �R�A�̐������W����
        m_corePosX = Random.Range(0, (int)dungeonSize.x * 10);
        m_corePosY = Random.Range(0, (int)dungeonSize.y * 10);

        int roop_error = 0;

        //�v���C���[�ƃR�A�̈ʒu�������܂ŌJ��Ԃ�
        do
        {
            m_playerPos = new Vector2(Random.Range(0, (int)dungeonSize.x * 10), Random.Range(0, (int)dungeonSize.y * 10));

            if (roop_error > 100)
            {
                Debug.Log("�R�A�ƃv���C���[���߂����܂��B�Ԋu���������Ă�������");
                break;
            }
            roop_error++;

        }
        while (
        m_playerPos.x < m_corePosX + m_playerLength &&
        m_playerPos.x > m_corePosX - m_playerLength &&
        m_playerPos.y < m_corePosY + m_playerLength &&
        m_playerPos.y > m_corePosY - m_playerLength
        );

        //  �v���C���[�̐���
        GameObject pl = Instantiate<GameObject>(m_player, m_playerPos, Quaternion.identity);

        // core�̐���
        GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePosX, m_corePosY), null, m_isBrightness);


        //  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
        if (m_playSceneManager == null)
            Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:DungeonManager");
        else
        {
            m_playSceneManager.SetPlayer(pl);
            m_playSceneManager.SetCore(co);
        }

        // �����p�^�[������
        switch (pattern)
        {
            case DungeonData.Pattern.TEN_X_TEN:
                Generate10to10(dungeonData, dungeonSize);
                break;

            case DungeonData.Pattern.DIGGING:
                GenerateDigging(dungeonData, dungeonSize);
                break;
        }


    }

    void Generate10to10(DungeonData dungeonData, Vector2 size)
    {
		//�P�O���P�O�̃��X�g�Ǘ��p
		List<List<List<string>>> mapListManager = new List<List<List<string>>>();

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
			List<List<string>> mapList = new List<List<string>>();

			// �t�@�C���̓��e��1�s������
			string[] lines = dungeonCSV[i].text.Split('\n');
			foreach (string line in lines)
			{
				string[] values = line.Split(',');

				// �e�s�̃f�[�^���i�[���郊�X�g
				List<string> rowData = new List<string>();

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
				GameObject block = Instantiate<GameObject>(m_ground, pos, Quaternion.identity);

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
                if (m_corePosX == x + originX && m_corePosY == y + originY)
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

    void GenerateDigging(DungeonData dungeonData, Vector2 size)
    {
        List<List<string>> mapList = m_dungeonGeneratorDig.GenerateDungeon(new Vector2Int((int)size.x, (int)size.y));

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
				if (m_corePosX == x && m_corePosY == y)
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
		for (int i = 0; i < (int)size.y; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3((int)size.y, i, 0), m_parent.transform, m_isBrightness);
		}
		for (int i = 0; i < (int)size.x; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, (int)size.y, 0), m_parent.transform, m_isBrightness);
		}

		//�n�ʂ̐���
		for (int y = 0; y < (int)size.y; ++y)
		{
			for (int x = 0; x < (int)size.x; ++x)
			{
				// �������W
				Vector3 pos = new(x, y, 0.0f);

				// �u���b�N�̐���
				GameObject block = Instantiate<GameObject>(m_ground, pos, Quaternion.identity);

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
