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

    [Header("ダンジョンマネージャ")]
    [SerializeField] private DungeonManager m_dungeonManager = null;

    [Header("プレイヤー")]
    [SerializeField] private Player m_player;
    //  UIキャンバス
    private GameObject m_playUI;
    private GameObject m_resultUI;
    //  コア
    private Block m_core;
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
        if (m_debugFlag)
        {
            m_stageNum = m_debugStageNum;
        }
        else
        {
            m_stageNum = m_stageNumScriptableObj.stageNum;
        }

        // ダンジョンにプレイヤーのトランスフォームを設定
        m_dungeonManager.SetTransform(m_player.transform);

        //  ステージ作成
        //GetComponent<DungeonGenerator>().SetStageNum(m_stageNum);
        //GetComponent<DungeonGenerator>().CreateStage();
        m_dungeonManager.SetSceneManager(this);
        m_dungeonManager.GenerateDungeon(m_stageNum);

        // プレイヤーの座標設定
        m_player.transform.position = m_dungeonManager.PlayerPosition;
        // コアの取得
        m_core = m_dungeonManager.DungeonCore;

        //  UI生成
        m_playUI = Instantiate(m_playUIObj);
        m_playUI.GetComponent<PlaySceneUICanvas>().SetPlayScenemManager(this);
        m_resultUI = Instantiate(m_resultUIObj);
        m_resultUI.GetComponent<ResultUI>().SetPlayScenemManager(this);

		// サーチの設定
		if (m_player.TryGetComponent(out SearchBlock search))
		{
			// サーチにブロックを設定する
			search.SetSearchBlocks(m_dungeonManager.GetBlocks());
			// サーチ範囲を設定
			Vector2Int size = m_dungeonManager.GetDungeonSize();
			search.MarkerMaxScale = Mathf.Max(size.x, size.y) / 2.0f;
		}

	}

	// Update is called once per frame
	void Update()
    {
        //  ゲーム遷移確認
        if (m_gameState == GameState.Play)
        {
            //  ゲームクリア
            if (m_core == null || m_core.IsDestroyed())
            {
                m_gameState = GameState.Clear;
                // ダンジョンのクリア設定
                m_dungeonManager.Clear();
                // アイテム設定
                SaveDataReadWrite.m_instance.Items = m_player.transform.Find("Item").gameObject.GetComponent<PlayerItem>().Items;
                // データ保存
                SaveDataReadWrite.m_instance.Save();
            }
            //  ゲームオーバー
            else if (m_player.GetComponent<Player>().HitPoint <= 0)
                m_gameState = GameState.Failed;
        }

        //  デバッグ用
        if (m_uiFlag) m_playUI.SetActive(true);
        else m_playUI.SetActive(false);
    }


    // プレイヤー
    public Player Player
    {
        get { return m_player; }
    }

    // コア
    public Block Core
    {
        get { return m_core; }
    }

    //  ゲーム状態取得
    public GameState GetGameState() { return m_gameState; }
}
