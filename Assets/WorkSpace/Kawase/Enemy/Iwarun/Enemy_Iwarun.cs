using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Iwarun : EnemyDwell
{
    [Header("��")]
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
        // �q���̃I�u�W�F�N�g���擾����
        Instantiate(m_stone, gameObject.transform.position, Quaternion.identity, gameObject.transform);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }

}
