using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
        set { ChangeWave(value); m_waveState = value; }
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

    [Header("�i�K�t���O")]
    [SerializeField] private bool m_dankaiFlag;

    //  �v���C���[�̃g�����X�t�H�[��
    private Transform m_playerTr;
    //  �R�A�̃g�����X�t�H�[��
    private Transform m_coreTr;
    //  �R�A�ƃv���C���[�̏�������
    private float m_distancePlayerCore;

    //�E�F�[�u�̌o�ߎ��ԊǗ�
    private float m_waveTime;
    //�_���W�����f�[�^
    private DungeonData m_dungeonDatas;
    //�X�e�[�W�ԍ�
    private int m_stageNum;


    void Start()
    {
        //  �v���C�V�[���}�l�[�W���[
        var pS = GetComponent<PlaySceneManager>();
        //  �v���C���[���W�擾
        m_playerTr = pS.GetPlayer().transform;
        //  �R�A�̍��W�擾
        m_coreTr = pS.GetCore().transform;
        //  �R�A�ƃv���C���[�̋����擾
        m_distancePlayerCore = Vector2.Distance(m_playerTr.position, m_coreTr.position);
        
        //�X�e�[�W�ԍ��̎擾
        m_stageNum = pS.StageNum;
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
        if(m_waveTime <= 0.0f)
        {
            //�E�F�[�u�؂�ւ�
            // �ŏ��̗v�f���擾������@
            m_waveState = WaveState.Attack;

            m_waveTime = 0.0f;
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

    private void ChangeWave(WaveState state)
    {
        //  �x�e���Ԑݒ�
        if(state == WaveState.Break && m_waveState != WaveState.Break) 
        {
            //  �R�A�ƃv���C���[�̋����v�Z
            var nowDistanceRate = Vector2.Distance(m_playerTr.position, m_coreTr.position) / m_distancePlayerCore;

            //  �x�e���Ԃ̎擾
            if (m_dankaiFlag)
            {
                float restTimeRate = 1.0f;
                foreach (var distance in m_dungeonDatas.RestTimeData)
                {
                    //  ����̋����{���Ȃ�x�e���Ԕ{���Đݒ�
                    if (nowDistanceRate <= distance.distanceRate)
                    {
                        restTimeRate = distance.restTimeRate;
                        break;
                    }
                }
                m_waveTime = m_dungeonDatas.RestWaveTime * restTimeRate;
            }
            else
            {
                m_waveTime = m_dungeonDatas.RestWaveTime;
                float decreaseTime = (1.0f - nowDistanceRate) * (1.0f - m_dungeonDatas.RestWaveMaxRate) * m_dungeonDatas.RestWaveTime;
                m_waveTime -= decreaseTime;
            }
        }
    }

}
