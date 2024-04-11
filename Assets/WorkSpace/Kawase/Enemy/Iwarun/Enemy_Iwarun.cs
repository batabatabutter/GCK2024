using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Iwarun : EnemyDwell
{
    [Header("岩")]
    [SerializeField] GameObject m_stone;
    protected override void Start()
    {
        base.Start();

    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    public override void Attack()
    {
        // 子供のオブジェクトを取得する
        Instantiate(m_stone, gameObject.transform.position, Quaternion.identity, gameObject.transform);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }

}
