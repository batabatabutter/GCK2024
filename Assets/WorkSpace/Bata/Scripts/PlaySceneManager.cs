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
    [SerializeField] private GameObject m_playUIObj;
    [SerializeField] private GameObject m_resultUIObj;

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

    // Start is called before the first frame update
    void Start()
    {
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
