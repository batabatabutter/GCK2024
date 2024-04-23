using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    //  状態
    public enum GameState
    {
        Play,
        Clear,
        Failed
    }

    //  UIキャンバス
    [Header("UIキャンバス")]
    [SerializeField] private GameObject m_playUIObj;
    [SerializeField] private GameObject m_resultUIObj;

    //  ステージ番号を取得
    [Header("ステージ番号取得用オブジェクト")]
    [SerializeField] private StageNumScriptableObject m_stageNumScriptableObj;

    //  プレイヤー
    private GameObject m_player;
    //  UIキャンバス
    private GameObject m_playUI;
    private GameObject m_resultUI;
    //  コア
    private GameObject m_core;
    private List<GameObject> m_cores;
    //  ゲーム状態
    private GameState m_gameState = GameState.Play;

    //  ステージ番号
    private int m_stageNum = 0;

    // ゲッター
    public int StageNum
    {
        get { return m_stageNum; }
    }

    //  デバッグ用
    [Header("デバッグ用")]
    [SerializeField] private bool m_debugFlag;
    [SerializeField] private int m_debugStageNum;
    [SerializeField] private bool m_uiFlag;

    // Start is called before the first frame update
    void Awake()
    {
        //  ステージ番号設定
        if (m_debugFlag) m_stageNum = m_debugStageNum;
        else m_stageNum = m_stageNumScriptableObj.stageNum;

        //  ステージ作成
        GetComponent<DungeonGenerator>().SetStageNum(m_stageNum);
        GetComponent<DungeonGenerator>().CreateStage();

        //  UI生成
        m_playUI = Instantiate(m_playUIObj);
        m_playUI.GetComponent<PlaySceneUICanvas>().SetPlayScenemManager(this);
        m_resultUI = Instantiate(m_resultUIObj);
        m_resultUI.GetComponent<ResultUI>().SetPlayScenemManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        //  ゲーム遷移確認
        if (m_gameState == GameState.Play)
        {
            //  ゲームクリア
            if (m_core == null || m_core.IsDestroyed() || !m_core.activeInHierarchy)
                m_gameState = GameState.Clear;
            //  ゲームオーバー
            else if (m_player.GetComponent<Player>().HitPoint <= 0)
                m_gameState = GameState.Failed;
        }

        //  デバッグ用
        if (m_uiFlag) m_playUI.SetActive(true);
        else m_playUI.SetActive(false);
    }

    //  プレイヤーの設定
    public void SetPlayer(GameObject player) { m_player = player; }
    //  プレイヤーの取得
    public GameObject GetPlayer() { return m_player; }

    //  コアの設定
    public void SetCore(GameObject core) { m_core = core; }
    //  コア達の取得
    public GameObject GetCore() { return m_core; }
    //  コアの追加
    public void AddCore(GameObject core) { m_cores.Add(core); }
    //  コア達の取得
    public List<GameObject> GetCores() { return m_cores; }

    //  ゲーム状態取得
    public GameState GetGameState() { return m_gameState; }
}
