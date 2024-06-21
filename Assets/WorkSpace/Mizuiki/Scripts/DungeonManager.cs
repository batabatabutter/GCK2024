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



    // �_���W��������
    public void GenerateDungeon(int stageNum)
    {
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
        m_attacker.enabled = false;
        m_enemyGenerator.enabled = false;
    }

}
