using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hotarun : EnemyDwell
{
    [Header("蛍火")]
    [SerializeField] GameObject m_firefly;

    float m_deleteFireTime;

    public float DeleteTime
    {
        get { return m_deleteFireTime; }
    }

    protected override void Start()
    {
        base.Start();

        m_deleteFireTime = m_enemyData.coolTime;

    }
    // Update is called once per frame
    protected override void Update()
    {
        //特殊な処理
        base.Update();

        //プレイヤーの方向を向く。いたら
        if (base.Player != null)
        {
            RotationToPlayer();
        }

        //宿り先が死んだら死ぬ
        if (!base.DwellBlock)
        {
            base.Dead();
        }

        //攻撃
        if (m_attackCoolTime < 0)
        {
            //攻撃
            Attack();
            //リセット
            m_attackCoolTime = m_enemyData.coolTime;
        }
        else
        {
            if(Player)
            {
                if(Vector3.Distance(Player.transform.position,transform.position) < m_enemyData.radius)
                m_attackCoolTime -= Time.deltaTime;
            }
        }



    }

    public override void Attack()
    {
        // 子供のオブジェクトを取得する
        Instantiate(m_firefly, gameObject.transform.position, Quaternion.identity, gameObject.transform);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
