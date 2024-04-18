using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bomurun : EnemyDwell
{
    [Header("����")]
    [SerializeField] GameObject m_blast;

    //�����������ǂ���
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

    //  �Γc�Q��E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E

    public override void Attack()
    {
        m_anim.Play("Bomb");
        // �q���̃I�u�W�F�N�g���擾����
        Instantiate(m_blast, gameObject.transform.position, Quaternion.identity, gameObject.transform);


    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
