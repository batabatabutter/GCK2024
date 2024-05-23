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
    [SerializeField] float m_spawnRadius;

    [Header("宿り先オブジェクトのレイヤー")]
    [SerializeField] LayerMask m_detectionLayer;  

    [Header("プレイヤー")]
    [SerializeField] private GameObject m_player;

	//出現する敵のリスト
	readonly List<Enemy.Type> m_spawnList = new();

    //時間
    private float m_spawnTime;
    //親
    private GameObject m_parent;

    //ウェーブ
    int m_wave = 0;

    //状態
    //WaveState m_waveState;

    //出現数
    int m_spawnEnemyNum;

    //スポーンタイマー
    float m_timer;

    //ウェーブマネージャー
    //WaveManager m_waveManager;

    [Header("ダンジョンアタッカー")]
    [SerializeField] private DungeonAttacker m_dungeonAttacker = null;


    // Start is called before the first frame update
    void Start()
    {
        //if(m_waveManager == null)
        //{
        //    m_waveManager = GetComponent<WaveManager>();
        //}

        int stageNum = 0;
		if (m_playSceneManager)
        {
			// ステージ番号
			stageNum = m_playSceneManager.StageNum;
			// プレイヤー取得
			if (m_player == null)
			{
				m_player = m_playSceneManager.GetPlayer();
			}

		}

        // ダンジョンアタッカー取得
        if (m_dungeonAttacker == null)
        {
            m_dungeonAttacker = GetComponent<DungeonAttacker>();
        }

		//現在のウェーブ数の取得
		//m_wave = m_waveManager.WaveNum;
        //ウェーブごとの情報
        DungeonData.DungeonWave dungeonData = m_dungeonDataBase.dungeonDatas[stageNum].DungeonWaves[m_wave];
        //出現数のせってい
        m_spawnEnemyNum = dungeonData.generateEnemyNum;

        //スポーン間隔
        m_spawnTime = dungeonData.geterateEnemyInterval;

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

        //if(m_player == null)
        {
            //親
            m_parent = new GameObject("Enemies");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // アタッカーがない場合はスポーンさせない
        if (m_dungeonAttacker == null)
        {
            return;
        }

        if (m_timer < 0)
        {
            //スポーン
            Spawn(m_spawnList[Random.Range(0, m_spawnList.Count)]);

            m_spawnEnemyNum--;

            m_timer = m_spawnTime;
        }
        else
        {
            // 攻撃状態の場合はタイマー減算
            //if(m_waveManager.waveState == WaveState.Attack)
            if (m_dungeonAttacker.Active)
            {
                m_timer -= Time.deltaTime;

            }
        }

        if(m_spawnEnemyNum <= 0)
        {
            //m_waveManager.waveState = WaveState.Break;
            ////ウェーブ上限じゃない場合かさん
            //if ( m_waveManager.WaveNum < m_waveManager.WAVE_MAX_NUM - 1)
            //{

            //    m_waveManager.WaveNum++;

            //}
            Start();

        }

    }

    public void Spawn()
    {
        // 念のため例外を生まないかチェック
        if (!CheckEnableEnemy())
        {
            Debug.Log("敵が生成できないよ。生成範囲を見直してね");
            return;
        }
        // タイプを指定してスポーン
        Spawn(m_spawnList[Random.Range(0, m_spawnList.Count)]);
    }
	public void Spawn(Enemy.Type type)
	{
        Spawn(type, m_spawnRadius);
	}
	public void Spawn(Enemy.Type type, float radius)
    {
        // 敵の種類が範囲外だった場合は生成しなおす
        if (type == Enemy.Type.OverID)
        {
            Spawn();
            return;
        }

        switch (m_enemyDataBase.enemyDatas[(int)type].system)
        {
            case Enemy.System.Dwell:
                // ブロック憑依型

                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_player.transform.position, radius, m_detectionLayer);

                List<Collider2D> collidersList = new(colliders);

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

                    // ブロックを憑依済みにする == 憑依不可能状態にする
                    randomCollider.GetComponent<Block>().CanPossess = false;
                }
                else
                {
                    Debug.Log("宿り先ブロックがない");
                }
                break;

            case Enemy.System.Mob:
                // 自立型(後で消すと思う)



            default:
                break;
        }
    }





    // リストの中に生成範囲の敵がいるか確認
    private bool CheckEnableEnemy()
    {
        foreach (Enemy.Type type in m_spawnList)
        {
            if (type != Enemy.Type.OverID)
            {
                // 範囲内
                return true;
            }
        }
        // 範囲内の敵はいなかった
        return false;
    }

    // コライダーを保持するかどうかを判定するメソッド
    private bool ShouldKeepCollider(Collider2D collider)
    {
        // 指定された条件に基づいてコライダーを保持するかどうかを判定
        Block block = collider.gameObject.GetComponent<Block>();

        // Blockスクリプトがアタッチされていない場合は保持しない
        if (block == null)
            return false;

		// 憑依済みの場合は保持しない
		if (block.CanPossess == false)
			return false;

		// BlockDataがnullでないことを確認
		if (block.BlockData != null)
        {
            return block.BlockData.Type < BlockData.BlockType.SPECIAL;
        }
        // ブロックデータがなかったら保持しない
        return false;
    }
}
