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


    // Start is called before the first frame update
    void Start()
    {
        // リジットボディ取得
        m_rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MovePlayer(Vector2 velocity)
    {
		// 移動量設定
		m_rigidbody.velocity = velocity * m_speed;

	}
}
