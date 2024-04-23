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

    //  �v���C���[
    private GameObject m_player;
    //  UI�L�����o�X
    private GameObject m_playUI;
    private GameObject m_resultUI;
    //  �R�A
    private GameObject m_core;
    private List<GameObject> m_cores;
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
        if (m_debugFlag) m_stageNum = m_debugStageNum;
        else m_stageNum = m_stageNumScriptableObj.stageNum;

        //  �X�e�[�W�쐬
        GetComponent<DungeonGenerator>().SetStageNum(m_stageNum);
        GetComponent<DungeonGenerator>().CreateStage();

        //  UI����
        m_playUI = Instantiate(m_playUIObj);
        m_playUI.GetComponent<PlaySceneUICanvas>().SetPlayScenemManager(this);
        m_resultUI = Instantiate(m_resultUIObj);
        m_resultUI.GetComponent<ResultUI>().SetPlayScenemManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        //  �Q�[���J�ڊm�F
        if (m_gameState == GameState.Play)
        {
            //  �Q�[���N���A
            if (m_core == null || m_core.IsDestroyed() || !m_core.activeInHierarchy)
                m_gameState = GameState.Clear;
            //  �Q�[���I�[�o�[
            else if (m_player.GetComponent<Player>().HitPoint <= 0)
                m_gameState = GameState.Failed;
        }

        //  �f�o�b�O�p
        if (m_uiFlag) m_playUI.SetActive(true);
        else m_playUI.SetActive(false);
    }

    //  �v���C���[�̐ݒ�
    public void SetPlayer(GameObject player) { m_player = player; }
    //  �v���C���[�̎擾
    public GameObject GetPlayer() { return m_player; }

    //  �R�A�̐ݒ�
    public void SetCore(GameObject core) { m_core = core; }
    //  �R�A�B�̎擾
    public GameObject GetCore() { return m_core; }
    //  �R�A�̒ǉ�
    public void AddCore(GameObject core) { m_cores.Add(core); }
    //  �R�A�B�̎擾
    public List<GameObject> GetCores() { return m_cores; }

    //  �Q�[����Ԏ擾
    public GameState GetGameState() { return m_gameState; }
}
