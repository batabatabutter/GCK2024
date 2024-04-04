using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.UI.Image;

public class DungeonGenerator : MonoBehaviour
{
    [Header("���邳������(�f�o�b�O)")]
    [SerializeField] private bool m_isBrightness;

    [Header("��������_���W������CSV�t�@�C��")]
    [SerializeField] private List<TextAsset> m_dungeonPath;

	[Header("�_���W�����̃T�C�Y(10*10�łP�T�C�Y)")]
    [SerializeField] private int m_dungeonSizeX;
    [SerializeField] private int m_dungeonSizeY;

    [System.Serializable]
    public class BlockOdds
    {
        [Header("���")]
        public BlockData.BlockType type;       // ���
        [Header("�m��")]
        public int odds;       // �m��
    }

    [SerializeField]
    List<BlockOdds> m_blockOddsList;

    [Header("�j����v���C���[�̏o�����Ȃ�����")]
    [SerializeField] private int m_playerLength = 35;
    [Header("�v���C���[")]
    [SerializeField] private GameObject m_player;
    [Header("�v���C�V�[���}�l�[�W���[")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    [Header("�n�ʔw�i")]
    [SerializeField] private GameObject m_ground;


    private int m_corePosX;
    private int m_corePosY;

    private Vector2 m_playerPos;

    private GameObject m_parent;

    private BlockGenerator m_blockGenerator;



    private void Awake()
    {
        m_blockGenerator = GetComponent<BlockGenerator>();

        m_parent = new GameObject("Blocks");

        m_corePosX = Random.Range(0, m_dungeonSizeX * 10);
        m_corePosY = Random.Range(0, m_dungeonSizeY * 10);

        //�v���C���[�ƃR�A�̈ʒu�������܂ŌJ��Ԃ�
        do
        {
            m_playerPos = new Vector2(Random.Range(0, m_dungeonSizeX * 10), Random.Range(0, m_dungeonSizeY * 10));

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
        GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePosX, m_corePosY), m_parent.transform, m_isBrightness);

        //GameObject co = Instantiate<GameObject>(m_blockCore, new Vector3(m_corePosX,m_corePosY), Quaternion.identity);
        //co.transform.parent = m_parent.transform;
        //���邳�̒ǉ�
        //if (m_isBrightness)
        //    co.AddComponent<ChangeBrightness>();


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


        for (int i = 0; i < m_dungeonPath.Count; i++)
        {
            // �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
            if (m_dungeonPath[i] == null)
            {
                Debug.Log("CSV file is not assigned");
                return;

            }

            // �}�b�v�̃��X�g
            List<List<string>> mapList = new List<List<string>>();

            // �t�@�C���̓��e��1�s������
            string[] lines = m_dungeonPath[i].text.Split('\n');
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

            {
                //// �t�@�C���ǂݍ���
                //StreamReader streamReader = new StreamReader(m_dungeonPath[i].text);

                //// ���s��؂�œǂݏo��
                //foreach (string line in streamReader.ReadToEnd().Split("\n"))
                //{
                //    // �s�����݂��Ȃ���΃��[�v�𔲂���
                //    if (line == "")
                //        break;

                //    string lin = line.Remove(line.Length - 1);

                //    List<string> list = new List<string>();

                //    // �J���}��؂�œǂݏo��
                //    foreach (string line2 in lin.Split(","))
                //    {
                //        list.Add(line2);
                //    }

                //    mapList.Add(list);
                //}
            }


            //�R�����ɓ����
            mapListManager.Add(mapList);

            // �t�@�C�������
            //streamReader.Close();

        }

        //�u���b�N����
        for (int y = 0; y < m_dungeonSizeY; y++)
        {
            for (int x = 0; x < m_dungeonSizeX; x++)
            {
                int random = Random.Range(0, m_dungeonPath.Count);
                Make10_10Block(mapListManager[random], x * 10, y * 10);

            }
        }

        //��Ղň͂�
        for (int i = 0; i < m_dungeonSizeY * 10; i++)
        {
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(m_dungeonSizeY * 10, i, 0), m_parent.transform, m_isBrightness);
        }
        for (int i = 0; i < m_dungeonSizeX * 10; i++)
        {
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, m_dungeonSizeY * 10, 0), m_parent.transform, m_isBrightness);
        }

        //�n�ʂ̐���
        for (int y = 0; y < m_dungeonSizeY * 10; ++y)
        {
            for (int x = 0; x < m_dungeonSizeX * 10; ++x)
            {
                // �������W
                Vector3 pos = new(x, y, 0.0f);

                // �u���b�N�̐���
                GameObject block = Instantiate<GameObject>(m_ground, pos, Quaternion.identity);

                block.transform.parent = m_parent.transform;

            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
                m_blockGenerator.GenerateBlock(m_blockOddsList[LotteryBlock()].type, pos, m_parent.transform, m_isBrightness);
            }
        }
    }


    int LotteryBlock()
    {
        //�m���̒��I
        List<int> oddsList = new List<int>();

        int allOdds = 0;

        //�u���b�N�̎�ނ̐�
        for (int i = 0; i < m_blockOddsList.Count; i++)
        {
            //�u���b�N�̊m��
            for (int j = 0; j < m_blockOddsList[i].odds; j++)
            {
                oddsList.Add(i);
            }
            //�u���b�N�̊m�������Z
            allOdds += m_blockOddsList[i].odds;
        }
        //���I
        return oddsList[Random.Range(0,allOdds)];
    }

}
