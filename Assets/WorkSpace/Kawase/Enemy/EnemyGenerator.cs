using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveManager;

public class EnemyGenerator : MonoBehaviour
{
    [Header("ダンジョンデータベース")]
    [SerializeField] DungeonDataBase m_dungeonDataBase;

    [Header("エネミーデータベース")]
    [SerializeField] EnemyDataBase m_enemyDataBase;

    [Header("プレイシーンマネージャー")]
    [SerializeField] PlaySceneManager m_playSceneManager;

    [Header("プレイヤー中心で敵が沸く範囲"),Min(0)]
    [SerializeField] float m_spawnradius;

    [Header("検知するレイヤー")]
    [SerializeField] LayerMask detectionLayer;  

    //出現する敵のリスト
    List<Enemy.Type> m_spawnList = new List<Enemy.Type>();

    //プレイヤー
    private GameObject m_player;

    //時間
    private float m_spawnTime;
    //親
    private GameObject m_parent;

    //ウェーブ
    int m_wave;

    //状態
    WaveManager.WaveState m_waveState;

    //出現数
    int m_spawnEnemyNum;

    //スポーンタイマー
    float m_timer;

    //ウェーブマネージャー
    WaveManager m_waveManager;

    // Start is called before the first frame update
    void Start()
    {
        if(m_waveManager == null)
        {
            m_waveManager = GetComponent<WaveManager>();
        }

        //ステージ番号
        int stageNum = m_playSceneManager.StageNum;
        //現在のウェーブ数の取得
        m_wave = m_waveManager.WaveNum;
        //ウェーブごとの情報
        DungeonData.DungeonWave dungeonData = m_dungeonDataBase.dungeonDatas[stageNum].DungeonWaves[m_wave];
        //出現数のせってい
        m_spawnEnemyNum = dungeonData.generateEnemyNum;

        //スポーン間隔
        m_spawnTime = dungeonData.dungeonATKCoolTime;

        //タイマーの設定
        m_timer = m_spawnTime;

        //初期化
        m_spawnList.Clear();

        //ステージの出現敵配列
        List<Enemy.Type> enemyTypeList = dungeonData.generateEnemyType;


        for (int i = 0; i < enemyTypeList.Count; i++)
        {
            //データベースにないものはいったん例外
            if((int)enemyTypeList[i] >= m_enemyDataBase.enemyDatas.Count)
            {
                continue;
            }
            //番号に合わせたプレハブを取得
            Enemy.Type type = enemyTypeList[i];
            // リスト内に同じ要素がないか確認
            if (!m_spawnList.Contains(type))
            {
                // 同じ要素がない場合に要素を追加
                m_spawnList.Add(type);
            }
        }

        if(m_player == null)
        {
            //プレイヤーよこせ
            m_player = m_playSceneManager.GetPlayer();
        }
        //if(m_player == null)
        {
            //親
            m_parent = new GameObject("Enemies");
        }

    }

    // Update is called once per frame
    void Update()
    {



        if (m_timer < 0)
        {
            //スポーン
            Spawn(m_spawnList[Random.Range(0, m_spawnList.Count)]);

            m_spawnEnemyNum--;

            m_timer = m_spawnTime;
        }
        else
        {
            if(m_waveManager.waveState == WaveState.Attack)
            {
                m_timer -= Time.deltaTime;

            }
        }

        if(m_spawnEnemyNum <= 0)
        {
            m_waveManager.waveState = WaveState.Break;
            //ウェーブ上限じゃない場合かさん
            if ( m_waveManager.WaveNum < m_waveManager.WAVE_MAX_NUM - 1)
            {

                m_waveManager.WaveNum++;

            }
            Start();

        }

    }

    public void Spawn(Enemy.Type type)
    {
        switch (m_enemyDataBase.enemyDatas[(int)type].system)
        {
            case Enemy.System.Dwell:

                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_player.transform.position, m_spawnradius, detectionLayer);

                List<Collider2D> collidersList = new List<Collider2D>(colliders);

                // 条件に合わない要素を削除
                collidersList.RemoveAll(collider => !ShouldKeepCollider(collider));

                if (collidersList.Count > 0)
                {
                    // 検知したオブジェクトからランダムに一つ選ぶ
                    Collider2D randomCollider = collidersList[Random.Range(0, collidersList.Count)];

                    // 選択されたオブジェクトに対する処理を行う

                    Vector3 spawnPos = randomCollider.transform.position;

                    GameObject enemy = Instantiate(m_enemyDataBase.enemyDatas[(int)type].prefab, spawnPos, Quaternion.identity, m_parent.transform);

                    enemy.GetComponent<EnemyDwell>().DwellBlock = randomCollider.gameObject;

                }
                else
                {
                    Debug.Log("宿り先ブロックがない");
                }

                    break;
            default:
                break;
        }

    }

    // コライダーを保持するかどうかを判定するメソッド
    bool ShouldKeepCollider(Collider2D collider)
    {
        // 指定された条件に基づいてコライダーを保持するかどうかを判定
        Block block = collider.gameObject.GetComponent<Block>();
        if (block != null && block.BlockData != null) // BlockDataがnullでないことを確認
        {
            return block.BlockData.Type < BlockData.BlockType.SPECIAL;
        }
        // Blockスクリプトがアタッチされていない場合は保持しない
        return false;
    }
}
