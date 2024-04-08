using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    const int MAX_RECHANGE = 100;

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


    // Start is called before the first frame update
    void Start()
    {
        //ステージ番号
        int stageNum = m_playSceneManager.StageNum;
        //ステージの出現敵配列
        List<Enemy.Type> enemyTypeList = m_dungeonDataBase.dungeonDatas[stageNum].enemy;

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

        //プレイヤーよこせ
        m_player = m_playSceneManager.GetPlayer();
        //スポーン間隔よこせ
        m_spawnTime = m_dungeonDataBase.dungeonDatas[stageNum].enemySpawnTime;

        //親
        m_parent = new GameObject("Enemies");

    }

    // Update is called once per frame
    void Update()
    {
        //ステージ番号
        int stageNum = m_playSceneManager.StageNum;

        if (m_spawnTime < 0)
        {
            //スポーン
            Spawn(m_spawnList[Random.Range(0, m_spawnList.Count)]);


            m_spawnTime = m_dungeonDataBase.dungeonDatas[stageNum].enemySpawnTime;
        }
        else
        {
            m_spawnTime -= Time.deltaTime;
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
