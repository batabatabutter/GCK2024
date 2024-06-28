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

    //[Header("�_���W�����̉ғ����")]
    //[SerializeField] private bool m_isActive = true;




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
    }

    // �_���W�����̃T�C�Y�擾
    public Vector2Int GetDungeonSize()
    {
        return m_generator.GetDungeonData().Size;
    }

    // �u���b�N�̎擾
    public Block[,] GetBlocks()
    {
        return m_generator.Blocks;
    }

    // �_���W�����̓������~�߂�
    public void Stop()
    {
		// �A�^�b�J�[���~�߂�
		m_attacker.enabled = false;
		// �G�l�~�[�̐������~�߂�
		m_enemyGenerator.enabled = false;

	}

    // �N���A���̌Ăяo��
    public void Clear()
    {
        // �_���W�������~�߂�
        Stop();

        // �f�[�^�ݒ�
        SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;
        // �N���A�ݒ�
        saveData.SetClear(m_stageNum);
        // �_���W�����̃��x�����グ��
        saveData.AddLevel(m_stageNum);
        // �u���b�N�z�u
        saveData.SetBlocks(m_generator.Blocks, m_stageNum);
        saveData.SaveBlocks(m_stageNum);

    }


    // �v���C���[�̃g�����X�t�H�[���ݒ�
    public void SetTransform(Transform player)
    {
        // �W�F�l���[�^�ɐݒ�(�`�����N�p)
        m_generator.SetPlayerTransform(player);
        // �A�^�b�J�[�ɐݒ�(�U���i�K�p)
        m_attacker.Target = player;
    }

	// �_���W�����W�F�l���[�^
	public DungeonGenerator DungeonGenerator
    {
        get { return m_generator; }
    }
    // �R�A
    public Block DungeonCore
    {
        get { return m_generator.DungeonCore; }
    }
    // �v���C���[�̍��W
    public Vector3 PlayerPosition
    {
        get { return m_generator.PlayerPosition; }
    }

}
