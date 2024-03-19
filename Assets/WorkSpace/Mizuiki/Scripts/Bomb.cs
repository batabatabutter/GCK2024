using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("爆破ダメージ")]
    [SerializeField] private float m_explosionPower = 1.0f;

    [Header("爆破範囲(半径)")]
    [SerializeField] private float m_explosionRange = 2.0f;

    [Header("爆発までの時間")]
    [SerializeField] private float m_timeToExplosion = 3.0f;

    // 爆発しているか
    private bool m_isExplosion = false;


    // Start is called before the first frame update
    void Start()
    {
        // リジッドボディがついてなければつける
        if (!GetComponent<Rigidbody2D>())
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            // 重力は存在しない
            rigidbody.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // カウントダウン
        if (m_timeToExplosion > 0.0f)
        {
            m_timeToExplosion -= Time.deltaTime;
        }
        // 時間が0以下 で爆発前
        else if (!m_isExplosion)
        {
            // BoxCollider2D を削除
            Destroy(GetComponent<BoxCollider2D>());

            // CircleCollider をつける
            CircleCollider2D circle = gameObject.AddComponent<CircleCollider2D>();
            // 爆破範囲の設定
            circle.radius = m_explosionRange;
            // トリガーにする
            circle.isTrigger = true;

            // レイヤーを Block 以外にする
            gameObject.layer = 0;

            // 爆発状態にする
            m_isExplosion = true;

        }
        // 爆発後
        else if (m_isExplosion)
        {
            // 自分自身を破壊する
            Destroy(gameObject);
        }

    }


	// 爆破ダメージを与える
	private void OnTriggerStay2D(Collider2D collision)
	{
        // ブロックタグが付いていない
        if (!collision.CompareTag("Block"))
            return;

        // ブロックスクリプト取得
        if (collision.TryGetComponent(out Block block))
        {
            block.AddMiningDamage(m_explosionPower);
        }

		
	}

}
