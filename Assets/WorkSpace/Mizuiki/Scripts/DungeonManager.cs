using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [Header("�v���C�V�[���}�l�[�W��")]
    [SerializeField] private PlaySceneManager m_playSceneManager = null;

    [Header("�W�F�l���[�^")]
    [SerializeField] private DungeonGenerator m_generator = null;

    [Header("�A�^�b�J�[")]
    [SerializeField] private DungeonAttacker m_attacker = null;

    [Header("�G�l�~�[")]
    [SerializeField] private EnemyGenerator m_enemyGenerator = null;

    [Header("�_���W�����ԍ�")]
    [SerializeField] private int m_stageNum = 0;


    // �_���W��������
    public void GenerateDungeon(int stageNum)
    {
        // �X�e�[�W�ԍ��ݒ�
        m_stageNum = stageNum;

        m_generator.CreateStage(stageNum);
    }

    // �V�[���}�l�[�W���ݒ�
    public void SetSceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

        // �q�̐ݒ�
        m_generator.PlaySceneManager = playSceneManager;
    }

    // �N���A���̌Ăяo��
    public void Clear()
    {
        // �A�^�b�J�[���~�߂�
        m_attacker.enabled = false;
        // �G�l�~�[�̐������~�߂�
        m_enemyGenerator.enabled = false;

        // �f�[�^�ݒ�
        SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;
        // �N���A�ݒ�
        saveData.SetClear(m_stageNum);
        // �u���b�N�z�u
        saveData.SetBlocks(m_generator.Blocks, m_stageNum);
        saveData.SaveBlocks(m_stageNum);

    }

}
