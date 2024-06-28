using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Rigidbody2D m_rigidbody;

    [Header("����")]
    [SerializeField] private float m_speed = 1.0f;

    //private Animator m_animator;


    // Start is called before the first frame update
    void Start()
    {
        // ���W�b�g�{�f�B�擾
        m_rigidbody = GetComponent<Rigidbody2D>();

        //// �A�j���[�^�[�擾
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
            // �ړ��ʂ̐ݒ�݂̂ŏ������I������
            m_rigidbody.velocity = velocity;
            return;
        }

        //m_animator.SetBool("Move", true);

		// �ړ��ʐݒ�
		m_rigidbody.velocity = velocity * m_speed;

		//// �A�j���[�^�[�ɕ�����ݒ�
		//m_animator.SetFloat("X", velocity.x);
		//m_animator.SetFloat("Y", velocity.y);

	}
}
