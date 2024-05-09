using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static TestDungeonGenerator;
/// <summary>
/// enemyを親にした自立型の敵
/// </summary>

public class EnemyMob : Enemy
{
    //マップ状況の読み込み
    const float RELOAD_TIME = 5.0f;

    protected List<Vector2Int> m_roadRoute;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //プレイヤーを見つけていない場合は処理をしない
        //if (base.Player == null)
        //    return;



    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && base.Player == null)
        {
            base.Player = collision.gameObject;

            // ゲームオブジェクトから Collider コンポーネントを削除する
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                Destroy(collider);
            }

            // ゲームオブジェクトにボックスコライダーを追加する
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

            // ボックスコライダーのサイズや位置を調整する
            boxCollider.size = new Vector2(1, 1); // サイズの設定
        }
    }

    //ブロックジェネレーターに書きたかったけど重かったやつ

    //m_roadRoute = m_generator.GetRoute();



    //public List<Vector2Int> GetRoute()
    //{
    //    List<Vector2Int> route = new List<Vector2Int>();

    //    for (int i = 0; i < m_blocks.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < m_blocks.GetLength(1); j++)
    //        {
    //            // ブロックが壊された場合やコアの場合にのみ追加する
    //            if (m_blocks[i, j] == null || m_blocks[i, j].tag != "Block")
    //            {
    //                Vector2Int newVector = new Vector2Int(j, i);

    //                // 新しい要素がリストに含まれていない場合のみ追加
    //                if (!route.Contains(newVector))
    //                {
    //                    route.Add(newVector);
    //                }
    //            }
    //        }
    //    }

    //    return route;
    //}
}
