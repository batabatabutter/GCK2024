using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.UI.Image;

public class DungeonGenerator : MonoBehaviour
{
    [Header("���邳������(�f�o�b�O)")]
    [SerializeField] private bool m_isBrightness;

    [Header("�X�e�[�W(0�`)")]
    [SerializeField] private int m_stageNum;

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



    private void Awake()
    {
        m_blockGenerator = GetComponent<BlockGenerator>();

        m_parent = new GameObject("Blocks");

        Vector2 dungeonSize = m_dungeonDataBase.dungeonDatas[m_stageNum].dungeonSize;

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

        //�P�O���P�O�̃��X�g�Ǘ��p
        List<List<List<string>>> mapListManager = new List<List<List<string>>>();

        //�f�[�^�x�[�X���烊�X�g�̎擾
        List<TextAsset> dungeonCSV = m_dungeonDataBase.dungeonDatas[m_stageNum].dungeonCSV;

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

            // �t�@�C�������
            //streamReader.Close();

        }

        //�u���b�N����
        for (int y = 0; y < (int)dungeonSize.y; y++)
        {
            for (int x = 0; x < (int)dungeonSize.x; x++)
            {
                int random = Random.Range(0, dungeonCSV.Count);
                Make10_10Block(mapListManager[random], x * 10, y * 10);

            }
        }

        //��Ղň͂�
        for (int i = 0; i < (int)dungeonSize.y * 10; i++)
        {
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3((int)dungeonSize.y * 10, i, 0), m_parent.transform, m_isBrightness);
        }
        for (int i = 0; i < (int)dungeonSize.x * 10; i++)
        {
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, (int)dungeonSize.y * 10, 0), m_parent.transform, m_isBrightness);
        }

        //�n�ʂ̐���
        for (int y = 0; y < (int)dungeonSize.y * 10; ++y)
        {
            for (int x = 0; x < (int)dungeonSize.x * 10; ++x)
            {
                // �������W
                Vector3 pos = new(x, y, 0.0f);

                // �u���b�N�̐���
                GameObject block = Instantiate<GameObject>(m_ground, pos, Quaternion.identity);

                block.transform.parent = m_parent.transform;

            }
        }

    }


    void Make10_10Block(List<List<string>> mapList,int originX, int originY)
    {
        // �ǂ݂������f�[�^�����ƂɃ_���W��������������
        for (int y = 0; y < mapList.Count; y++)
        {
            for (int x = 0; x < mapList[y].Count; x++)
            {
                //�v���C���[
                if((int)m_playerPos.x == x + originX && (int)m_playerPos.y == y + originY)
                {
                    continue;
                }

                //�R�A�𐶐�
                if(m_corePosX == x + originX && m_corePosY == y + originY)
                {
                    continue;
                }
                // 0 �̏ꍇ�͉����������Ȃ�
                if (mapList[y][x] == "0" || mapList[y][x] == "")
                    continue;

                // �������W
                Vector3 pos = new(originX + x,originY + y, 0.0f);

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
        return oddsList[Random.Range(0,allOdds)];
    }


    public int GetStageNum()
    {
        return m_stageNum;
    }
}
