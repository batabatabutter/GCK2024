using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dri : MonoBehaviour
{
    //大きさ
    float scale = 0.0f;
    //宿り先
    GameObject m_dwellBlock;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.9f,0,1);

        transform.localPosition  = new Vector3(0,0.5f,0);

        // 親オブジェクトを取得する
        GameObject parentObject = transform.parent.gameObject;

        // 親オブジェクトからスクリプトを取得する
        m_dwellBlock = parentObject.GetComponent<EnemyDwell>().DwellBlock;
    }

    // Update is called once per frame
    void Update()
    {
        if(scale > 2.0f)
        {
            Destroy(gameObject);

        }
        else
        {
            scale += Time.deltaTime;


            transform.localScale = new Vector3(0.9f, scale, 1);

            transform.localPosition = new Vector3(0, scale / 4 + 0.5f , 0);

            // 子オブジェクトのローカル回転を無効にする
            transform.localRotation = Quaternion.identity;


        }
    }
private void OnCollisionEnter2D(Collision2D collision)
    {
        // ブロックに当たった場合
        if (collision.gameObject.CompareTag("Block") && collision.gameObject != m_dwellBlock)
        {
            // オブジェクトを破壊する
            Destroy(gameObject);
        }

        // プレイヤーに当たった場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える
            collision.gameObject.GetComponent<Player>().AddDamage(1);

            // オブジェクトを破壊する
            Destroy(gameObject);
        }
    }
}
