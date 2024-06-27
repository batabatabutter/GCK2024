using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBomb : Block
{
    [System.Serializable]
    public enum BombState
    {
        STAY,       // 待機
        DETONATE,   // 起爆
        EXPLOSION,  // 爆破
        DESTROY,    // 破壊

        OVER,
    }

    [Header("---------- 爆弾 ----------")]

    [Header("ブロックへの爆破ダメージ")]
    [SerializeField] private float m_blockExplosionPower = 1.0f;
    [Header("プレイヤーへの爆破ダメージ")]
    [SerializeField] private int m_playerExplosionDamage = 1;

    [Header("爆破範囲(半径)")]
    [SerializeField] private float m_explosionRange = 2.0f;

    [Header("爆発までの時間")]
    [SerializeField] private float m_timeToExplosion = 3.0f;

    [Header("破壊の方式(ダメージ方式/確定破壊)")]
    [SerializeField] private bool m_damage = true;
    [Header("即時爆破ダメージ")]
    [SerializeField] private float m_immediateDamage = 1000.0f;

    [Header("ブロック破壊時のアイテムドロップ倍率")]
    [SerializeField] private int m_itemDropRate = 0;

    [Header("爆破状態")]
    [SerializeField] private BombState m_state = BombState.STAY;
    //[SerializeField] private bool m_detonate = false;

    [Header("爆発させる用のブロック")]
    [SerializeField] private GameObject m_detonateBlock = null;

    [Header("爆発音声")]
    [SerializeField] private AudioClip m_bombSE = null;

    //// 爆破可能か
    //private bool m_canExplosion = false;
    //// 爆発しているか
    //private bool m_isExplosion = false;


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
        // 爆破状態
        switch (m_state)
        {
            case BombState.STAY:    // 待機中

                break;

            case BombState.DETONATE:    // 起爆後
                // カウントダウン
                m_timeToExplosion -= Time.deltaTime;
                // 時間経過で爆破状態に遷移
                if (m_timeToExplosion < 0)
                {
                    m_state = BombState.EXPLOSION;
                }

                break;

            case BombState.EXPLOSION:   // 爆破
				// 爆発に必要なブロックの生成
				if (m_detonateBlock)
				{
					// 起爆ブロックの生成
					Instantiate(m_detonateBlock, transform.position, Quaternion.identity);

                    //  爆発音
                    AudioManager.Instance.PlaySE(m_bombSE, transform.position);

					// 一回生成したら null にする
					m_detonateBlock = null;
				}
                else
                {
                    break;
                }

                // BoxCollider2D を削除
				Destroy(GetComponent<BoxCollider2D>());

				// CircleCollider をつける
				CircleCollider2D circle = gameObject.AddComponent<CircleCollider2D>();
				// 爆破範囲の設定
				circle.radius = m_explosionRange;
				// トリガーにする
				circle.isTrigger = true;

				// レイヤーを Bomb にする
				gameObject.layer = LayerMask.NameToLayer("Bomb");

				break;

            case BombState.DESTROY:     // 破壊
				// 自分自身を破壊する
				Destroy(gameObject);

				break;
        }

    }

    // 起爆する
    public void Detonate()
    {
        // 起爆前
        if (m_state == BombState.STAY)
        {
			// 爆破状態にする
			m_state = BombState.DETONATE;

			// スプライトの色を変える
			GetComponent<SpriteRenderer>().color = Color.red;
		}
	}


	// 爆破ダメージを与える
	private void OnTriggerEnter2D(Collider2D collision)
	{
        // 爆破状態ではない
        if (m_state < BombState.EXPLOSION)
            return;

        // タグかレイヤーがブロック
        if (collision.CompareTag("Block") || collision.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            // ブロックスクリプト取得
            if (collision.TryGetComponent(out Block block))
            {
                // ダメージ方式
                if (m_damage)
                {
                    block.AddMiningDamage(m_blockExplosionPower, m_itemDropRate);
                }
                // 破壊方式
                else
                {
                    block.BrokenBlock();
                }
            }

            // 破壊状態にする
            m_state = BombState.DESTROY;
        }
        // タグがプレイヤー
        else if (collision.CompareTag("Player"))
        {
            // プレイヤースクリプト取得
            if (collision.TryGetComponent(out Player player))
            {
                player.AddDamage(m_playerExplosionDamage);
            }

			// 破壊状態にする
			m_state = BombState.DESTROY;
		}

	}

    // ダメージを受けた
    public override bool AddMiningDamage(float power, int dropCount = 1)
    {
        // 起爆
        Detonate();

        return true;
    }
	// 破壊しようとされている
	public override bool BrokenBlock(int dropCount = 1)
	{
        // 起爆
        Detonate();

        return true;
	}

}
