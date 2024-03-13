using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class DungeonGenerator : MonoBehaviour
{
    [Header("���邳������(�f�o�b�O)")]
    [SerializeField] private bool m_isBrightness;


    [Header("��������_���W�����̃p�X")]
    [SerializeField] private List<string> m_dungeonPath;

	[Header("�_���W�����̃T�C�Y(10*10�łP�T�C�Y)")]
    [SerializeField] private int m_dungeonSizeX;
    [SerializeField] private int m_dungeonSizeY;

    [Header("�j�u���b�N")]
    [SerializeField] private�@GameObject m_blockCore;

    [Header("�����u���b�N")]
    [SerializeField] private List<GameObject> m_block = null;
    [Header("�����u���b�N�̊m�����i�O�`�P�O�O�j�i��̂Ɠ������Ԃˁj")]
    [SerializeField] private List<int> m_blockOdds = null;

    [Header("���̕ϓN���Ȃ��u���b�N")]
    //[SerializeField] private string m_blockNormalNum = "1";
    [Header("��Ճu���b�N")]
    [SerializeField] private string m_blockBedrockNum = "2";
    [Header("�_���W�����̊j")]
    [SerializeField] private string m_blockCoreNum = "3";

    [Header("�j����v���C���[�̏o�����Ȃ�����")]
    [SerializeField] private int m_playerLength = 35;
    [Header("�v���C���[")]
    [SerializeField] private GameObject m_player;
    [Header("�v���C�V�[���}�l�[�W���[")]
    [SerializeField] private PlaySceneManager m_playSceneManager;


    private int m_corePosX;
    private int m_corePosY;

    private Vector2 m_playerPos;



    private void Awake()
    {
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
        //  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
        if (m_playSceneManager == null)
            Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:DungeonManager");
        else
        {
            m_playSceneManager.SetPlayer(pl);
            m_playSceneManager.AddCore(m_blockCore);
        }
    }


    // Start is called before the first frame update
    void Start()
    {





        //�P�O���P�O�̃��X�g�Ǘ��p
        List<List<List<string>>> mapListManager = new List<List<List<string>>>();


        for (int i = 0; i < m_dungeonPath.Count; i++)
        {
            // �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
            if (!File.Exists(m_dungeonPath[i]))
            {
                Debug.Log(m_dungeonPath[i]);
                return;

            }

            // �}�b�v�̃��X�g
            List<List<string>> mapList = new List<List<string>>();

            // �t�@�C���ǂݍ���
            StreamReader streamReader = new StreamReader(m_dungeonPath[i]);

            // ���s��؂�œǂݏo��
            foreach (string line in streamReader.ReadToEnd().Split("\n"))
            {
                // �s�����݂��Ȃ���΃��[�v�𔲂���
                if (line == "")
                    break;

                string lin = line.Remove(line.Length - 1);

                List<string> list = new List<string>();

                // �J���}��؂�œǂݏo��
                foreach (string line2 in lin.Split(","))
                {
                    list.Add(line2);
                }

                mapList.Add(list);
            }
            //�R�����ɓ����
            mapListManager.Add(mapList);

            // �t�@�C�������
            streamReader.Close();

        }

        //�u���b�N����
        for (int y = 0; y < m_dungeonSizeY;y++)
        {
            for(int x = 0; x < m_dungeonSizeX;x++)
            {
                int random = Random.Range(0, m_dungeonPath.Count);
                Make10_10Block(mapListManager[random], x * 10, y * 10);

            }
        }




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
                if((int)m_playerPos.x == x + originX && (int)m_playerPos.y == y + originY)
                {
                    continue;
                }

                //�R�A�𐶐�
                if(m_corePosX == x + originX && m_corePosY == y + originY)
                {
                    MakeCore();

                    continue;
                }
                // 0 �̏ꍇ�͉����������Ȃ�
                if (mapList[y][x] == "0" || mapList[y][x] == "")
                    continue;

                // �������W
                Vector3 pos = new(originX + x,originY + y, 0.0f);

                // �u���b�N�̐���
                GameObject block = Instantiate<GameObject>(m_block[LotteryBlock(m_blockOdds)], pos, Quaternion.identity);

                // �u���b�N�X�N���v�g������
                block.AddComponent<Block>();

                // �j��s�\�u���b�N�ɂ���
                if (mapList[y][x] == m_blockBedrockNum)
                {
                    // ������₷���悤�ɂƂ肠�����F��ς���
                    block.GetComponent<SpriteRenderer>().color = Color.gray;
                    // �j��s�\�ɂ���
                    block.GetComponent<Block>().DontBroken = true;
                }

                // �j�ɂ���
                if (mapList[y][x] == m_blockCoreNum)
                {
                    // ������₷���悤�ɂƂ肠�����F��ς���
                    block.GetComponent<SpriteRenderer>().color = Color.red;
                    // �_���W�����X�N���v�g������
                    block.AddComponent<Dungeon>();
                }

                //���邳�̒ǉ�
                if(m_isBrightness)
                    block.AddComponent<ChangeBrightness>();
            }
        }
    }


    int LotteryBlock(List<int> blockOddsList)
    {
        //�m���̒��I
        List<int> oddsList = new List<int>();

        int allOdds = 0;

        for (int i = 0; i < blockOddsList.Count; i++)
        {
            for (int j = 0; j < blockOddsList[i]; j++)
            {
                oddsList.Add(i);
            }
            allOdds += blockOddsList[i];
        }

        return oddsList[Random.Range(0,allOdds)];
    }

    private void MakeCore()
    {
        // �������W
        Vector3 pos = new(m_corePosX, m_corePosY, 0.0f);

        // �u���b�N�̐���
        GameObject block = Instantiate<GameObject>(m_blockCore, pos, Quaternion.identity);

        // �u���b�N�X�N���v�g������
        block.AddComponent<Block>();

        //���邳�̒ǉ�
        if (m_isBrightness)
            block.AddComponent<ChangeBrightness>();

    }
}
