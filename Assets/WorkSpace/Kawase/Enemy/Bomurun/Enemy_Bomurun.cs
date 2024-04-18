using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bomurun : EnemyDwell
{
    [Header("爆発")]
    [SerializeField] GameObject m_blast;

    //爆発したかどうか
    bool m_isExplosion = false;

    Animator m_anim;

    protected override void Start()
    {
        base.Start();

        m_anim = GetComponent<Animator>();

    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    //  石田参上・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・・

    public override void Attack()
    {
        m_anim.Play("Bomb");
        // 子供のオブジェクトを取得する
        Instantiate(m_blast, gameObject.transform.position, Quaternion.identity, gameObject.transform);


    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
