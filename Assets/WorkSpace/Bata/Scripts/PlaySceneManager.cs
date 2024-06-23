using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    //  ���
    public enum GameState
    {
        Play,
        Clear,
        Failed
    }

    //  UI�L�����o�X
    [Header("UI�L�����o�X")]
    [SerializeField] private GameObject m_playUIObj;
    [SerializeField] private GameObject m_resultUIObj;

    //  �X�e�[�W�ԍ����擾
    [Header("�X�e�[�W�ԍ��擾�p�I�u�W�F�N�g")]
    [SerializeField] private StageNumScriptableObject m_stageNumScriptableObj;

    [Header("�_���W�����}�l�[�W��")]
    [SerializeField] private DungeonManager m_dungeonManager = null;

    [Header("�v���C���[")]
    [SerializeField] private Player m_player;
    //  UI�L�����o�X
    private GameObject m_playUI;
    private GameObject m_resultUI;
    //  �R�A
    private Block m_core;
    //  �Q�[�����
    private GameState m_gameState = GameState.Play;

    //  �X�e�[�W�ԍ�
    private int m_stageNum = 0;

    // �Q�b�^�[
    public int StageNum
    {
        get { return m_stageNum; }
    }

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    [SerializeField] private bool m_debugFlag;
    [SerializeField] private int m_debugStageNum;
    [SerializeField] private bool m_uiFlag;

    // Start is called before the first frame update
    void Awake()
    {
        //  �X�e�[�W�ԍ��ݒ�
        if (m_debugFlag)
        {
            m_stageNum = m_debugStageNum;
        }
        else
        {
            m_stageNum = m_stageNumScriptableObj.stageNum;
        }

        // �_���W�����Ƀv���C���[�̃g�����X�t�H�[����ݒ�
        m_dungeonManager.SetTransform(m_player.transform);

        //  �X�e�[�W�쐬
        //GetComponent<DungeonGenerator>().SetStageNum(m_stageNum);
        //GetComponent<DungeonGenerator>().CreateStage();
        m_dungeonManager.SetSceneManager(this);
        m_dungeonManager.GenerateDungeon(m_stageNum);

        // �v���C���[�̍��W�ݒ�
        m_player.transform.position = m_dungeonManager.PlayerPosition;
        // �R�A�̎擾
        m_core = m_dungeonManager.DungeonCore;

        //  UI����
        m_playUI = Instantiate(m_playUIObj);
        m_playUI.GetComponent<PlaySceneUICanvas>().SetPlayScenemManager(this);
        m_resultUI = Instantiate(m_resultUIObj);
        m_resultUI.GetComponent<ResultUI>().SetPlayScenemManager(this);

		// �T�[�`�̐ݒ�
		if (m_player.TryGetComponent(out SearchBlock search))
		{
			// �T�[�`�Ƀu���b�N��ݒ肷��
			search.SetSearchBlocks(m_dungeonManager.GetBlocks());
			// �T�[�`�͈͂�ݒ�
			Vector2Int size = m_dungeonManager.GetDungeonSize();
			search.MarkerMaxScale = Mathf.Max(size.x, size.y) / 2.0f;
		}

	}

	// Update is called once per frame
	void Update()
    {
        //  �Q�[���J�ڊm�F
        if (m_gameState == GameState.Play)
        {
            //  �Q�[���N���A
            if (m_core == null || m_core.IsDestroyed())
            {
                m_gameState = GameState.Clear;
                // �_���W�����̃N���A�ݒ�
                m_dungeonManager.Clear();
                // �A�C�e���ݒ�
                SaveDataReadWrite.m_instance.Items = m_player.transform.Find("Item").gameObject.GetComponent<PlayerItem>().Items;
                // �f�[�^�ۑ�
                SaveDataReadWrite.m_instance.Save();
            }
            //  �Q�[���I�[�o�[
            else if (m_player.GetComponent<Player>().HitPoint <= 0)
                m_gameState = GameState.Failed;
        }

        //  �f�o�b�O�p
        if (m_uiFlag) m_playUI.SetActive(true);
        else m_playUI.SetActive(false);
    }


    // �v���C���[
    public Player Player
    {
        get { return m_player; }
    }

    // �R�A
    public Block Core
    {
        get { return m_core; }
    }

    //  �Q�[����Ԏ擾
    public GameState GetGameState() { return m_gameState; }
}
