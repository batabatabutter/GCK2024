using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blast : Enemy_AttackBall
{
    [Header("ブロックへのダメージ")]
    [SerializeField] int m_addBlockDamege = 500;
    [Header("プレイヤーへのダメージ")]
    [SerializeField] int m_playerDamege = 1;

    // Rigidbody2Dコンポーネントをアタッチするオブジェクト
    private Rigidbody2D rb;


    protected override void Start()
    {
        base.Start();

        transform.parent = null;

        // Rigidbody2Dコンポーネントを取得する
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // タグかレイヤーがブロック
        if (collision.CompareTag("Block") || collision.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            // 当たったオブジェクトから Block スクリプトを取得
            Block[] blocks = collision.GetComponents<Block>();

            // 取得したスクリプトをループで処理
            foreach (Block block in blocks)
            {
                // スクリプトが存在する場合の処理
                if (block != null && !block.IsDestroyed() && block)
                {
                    block.AddMiningDamage(m_addBlockDamege);
                }
            }
        }
        //プレイヤーにダメージ
        if(collision.CompareTag("Player"))
        {
            // プレイヤースクリプト取得
            if (collision.TryGetComponent(out Player player))
            {
                player.AddDamage(m_playerDamege);
            }
        }
        //宿り先は自爆
        if (m_dwellBlock.TryGetComponent(out Block dwellblock))
        {
            dwellblock.BrokenBlock();
        }

        //消える
        DestroyThis();

    }

    public override void MoveStart()
    {

    }
}
