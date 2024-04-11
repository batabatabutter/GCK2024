using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dri : MonoBehaviour
{
    //宿り先
    GameObject m_dwellBlock;

    // Start is called before the first frame update
    void Start()
    {
        // 親オブジェクトを取得する
        GameObject parentObject = transform.parent.gameObject;

        // 親オブジェクトからスクリプトを取得する
        m_dwellBlock = parentObject.GetComponent<EnemyDwell>().DwellBlock;

    }

    // Update is called once per frame
    void Update()
    {
         // 子オブジェクトのローカル回転を無効にする
         transform.localRotation = Quaternion.identity;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ブロックに当たった場合
        if (collision.gameObject.CompareTag("Block") && collision.gameObject != m_dwellBlock)
        {
            // オブジェクトを破壊する
            DestroyDri();
            Debug.Log(collision.gameObject.transform.position);
        }

        // プレイヤーに当たった場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える
            collision.gameObject.GetComponent<Player>().AddDamage(1);

            // オブジェクトを破壊する
            DestroyDri();
        }

    }


    public void DestroyDri()
    {
        Destroy(gameObject);
    }
}
