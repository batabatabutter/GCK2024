using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iwa : Enemy_AttackBall
{

    [Header("���x")] 
    [SerializeField] float m_speed = 3f;

    private bool m_isMove = false;

    // Rigidbody2D�R���|�[�l���g���A�^�b�`����I�u�W�F�N�g
    private Rigidbody2D rb;

    private Quaternion m_parentRotation;

    Vector2 m_velocityDirection;

    protected override void Start()
    {
        base.Start();

        // Rigidbody2D�R���|�[�l���g���擾����
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (m_isMove)
        {
            // ���x��ݒ肷��
            rb.velocity = m_velocityDirection * m_speed;

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void MoveStart()
    {
        // �e�I�u�W�F�N�g�̉�]���擾����
        m_parentRotation = transform.parent.transform.rotation;

        Vector2 targetPos = transform.parent.GetComponent<Enemy_Iwarun>().Player.transform.position;
        Vector2 startLocation = transform.parent.position;

        // �e�I�u�W�F�N�g�̐��ʕ������擾����
        m_velocityDirection = (Vector2)(targetPos - startLocation).normalized;

        transform.parent = null;
        m_isMove = true;
    }
}
