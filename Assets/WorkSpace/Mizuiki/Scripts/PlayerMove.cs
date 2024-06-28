using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Rigidbody2D m_rigidbody;

    [Header("速さ")]
    [SerializeField] private float m_speed = 1.0f;

    //private Animator m_animator;


    // Start is called before the first frame update
    void Start()
    {
        // リジットボディ取得
        m_rigidbody = GetComponent<Rigidbody2D>();

        //// アニメーター取得
        //m_animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MovePlayer(Vector2 velocity)
    {
        if (velocity.magnitude <= 0.0f)
        {
            //m_animator.SetBool("Move", false);
            // 移動量の設定のみで処理を終了する
            m_rigidbody.velocity = velocity;
            return;
        }

        //m_animator.SetBool("Move", true);

		// 移動量設定
		m_rigidbody.velocity = velocity * m_speed;

		//// アニメーターに方向を設定
		//m_animator.SetFloat("X", velocity.x);
		//m_animator.SetFloat("Y", velocity.y);

	}
}
