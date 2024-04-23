using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iwa : Enemy_AttackBall
{

    [Header("速度")] 
    [SerializeField] float m_speed = 3f;

    private bool m_isMove = false;

    // Rigidbody2Dコンポーネントをアタッチするオブジェクト
    private Rigidbody2D rb;

    private Quaternion m_parentRotation;

    Vector2 m_velocityDirection;

    protected override void Start()
    {
        base.Start();

        // Rigidbody2Dコンポーネントを取得する
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (m_isMove)
        {
            // 速度を設定する
            rb.velocity = m_velocityDirection * m_speed;

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void MoveStart()
    {
        // 親オブジェクトの回転を取得する
        m_parentRotation = transform.parent.transform.rotation;

        Vector2 targetPos = transform.parent.GetComponent<Enemy_Iwarun>().Player.transform.position;
        Vector2 startLocation = transform.parent.position;

        // 親オブジェクトの正面方向を取得する
        m_velocityDirection = (Vector2)(targetPos - startLocation).normalized;

        transform.parent = null;
        m_isMove = true;
    }
}
