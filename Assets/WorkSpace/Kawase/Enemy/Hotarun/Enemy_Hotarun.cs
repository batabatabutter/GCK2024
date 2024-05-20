using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hotarun : EnemyDwell
{
    [Header("�u��")]
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
        //����ȏ���
        base.Update();

        //�v���C���[�̕����������B������
        if (base.Player != null)
        {
            RotationToPlayer();
        }

        //�h��悪���񂾂玀��
        if (!base.DwellBlock)
        {
            base.Dead();
        }

        //�U��
        if (m_attackCoolTime < 0)
        {
            //�U��
            Attack();
            //���Z�b�g
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
        // �q���̃I�u�W�F�N�g���擾����
        Instantiate(m_firefly, gameObject.transform.position, Quaternion.identity, gameObject.transform);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
