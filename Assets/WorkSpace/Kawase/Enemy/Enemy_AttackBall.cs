using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackBall : MonoBehaviour
{
    //宿り先
    GameObject m_dwellBlock;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // 親オブジェクトを取得する
        GameObject parentObject = transform.parent.gameObject;

        // 親オブジェクトからスクリプトを取得する
        m_dwellBlock = parentObject.GetComponent<EnemyDwell>().DwellBlock;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // 子オブジェクトのローカル回転を無効にする
        transform.localRotation = Quaternion.identity;

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // ブロックに当たった場合
        if (collision.gameObject.CompareTag("Block") && collision.gameObject != m_dwellBlock)
        {
            // オブジェクトを破壊する
            DestroyThis();
        }

        // プレイヤーに当たった場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える
            collision.gameObject.GetComponent<Player>().AddDamage(1);

            // オブジェクトを破壊する
            DestroyThis();
        }

    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

}
