using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int WAVE_MAX_NUM;

    public enum WaveState
    {
        Attack,//�U��
        Break,//�x�e
    }

    //�t�F�[�Y�Ǘ�
    private WaveState m_waveState;

    public WaveState waveState
    {
        get { return m_waveState; }
        set { m_waveState = value; }
    }

    //�t�F�[�Y�̔ԍ�
    private int m_waveNum;

    public int WaveNum
    {
        get { return m_waveNum; }
        set { m_waveNum = value; }
    }


    [Header("�_���W�����f�[�^�x�[�X")]
    [SerializeField] private DungeonDataBase m_DungeonDataBase;

    //�E�F�[�u�̌o�ߎ��ԊǗ�
    private float m_waveTime;
    //�_���W�����f�[�^
    private DungeonData m_dungeonDatas;
    //�X�e�[�W�ԍ�
    private int m_stageNum;


    void Start()
    {
        //�X�e�[�W�ԍ��̎擾
        m_stageNum = GetComponent<PlaySceneManager>().StageNum;
        //�_���W�����̃f�[�^�擾
        m_dungeonDatas = m_DungeonDataBase.dungeonDatas[m_stageNum];
        //�ŏ��͋x�e�t�F�[�Y����
        m_waveState = WaveState.Break;

        //�x�e���Ԃ̎擾
        m_waveTime = m_dungeonDatas.RestWaveTime;
        //�t�F�[�Y�O
        m_waveNum = 0;

        //�E�F�[�u�̏���l
        WAVE_MAX_NUM = m_dungeonDatas.DungeonWaves.Count;
    }

    // Update is called once per frame
    void Update()
    {

        //�x�e����
        if(m_waveTime < 0)
        {
            //�E�F�[�u�؂�ւ�
            // �ŏ��̗v�f���擾������@
            m_waveState = WaveState.Attack;
            //�x�e���Ԃ̎擾
            m_waveTime = m_dungeonDatas.RestWaveTime;

        }
        //���Ԃ�����x�e��Ԃ�������
        else if(m_waveTime > 0 && m_waveState == WaveState.Break)
        {
            //�E�F�[�u���Ԃ̐i�s
            m_waveTime -= Time.deltaTime;
        }


        // �����̏������G�l�~�[�W�F�l���[�^�[�ɑg�ݍ���
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    //�x�e�t�F�[�Y��Ɉڍs
        //    m_waveState = WaveState.Break;
        //    m_waveNum++;
        //}
        //if (Time.frameCount % 60 == 0) // 60FPS
        //{
        //    Debug.Log("��ԁF" + m_waveState);
        //    Debug.Log("�E�F�[�u�ԍ��F" + m_waveNum);
        //}
        //Debug.Log("�E�F�[�u����F" + (WAVE_MAX_NUM - 1));
    }

}
