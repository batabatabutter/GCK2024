using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollRock : MonoBehaviour
{

    [Header("転がるまでの時間")]
    [SerializeField] float m_rollTime = 3.0f;
    [Header("消滅までの時間")]
    [SerializeField] float m_destroyTime = 10.0f;
    [Header("速度（m/s）")]
    [SerializeField] float m_speed = 5.0f;
    [Header("最終スケール")]
    [SerializeField] private float m_finalScale = 1.0f;
    [Header("攻撃力")]
    public int m_damage = 1;

    int m_rotate;

    float m_scale = 0.0f;

    Rigidbody2D m_rb;


    // Start is called before the first frame update
    void Start()
    {
        m_rotate = (int)transform.localEulerAngles.z;
        m_rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        m_rollTime -= Time.deltaTime;
        m_destroyTime -= Time.deltaTime;

        if(m_destroyTime < 0 )
        {
            Destroy(gameObject);
        }

        if (transform.lossyScale.x < m_finalScale)
        {
            m_scale += Time.deltaTime * 0.5f;
        }

        if (m_rollTime < 0)
        {
            if(m_rotate == 0)
            {
                //上に行く
                m_rb.velocity = new Vector3(0, m_speed, 0);
            }
            else if(m_rotate == 90)
            {
                //左
                m_rb.velocity = new Vector3(-m_speed, 0, 0);


            }
            else if(m_rotate == 180)
            {
                //下
                m_rb.velocity = new Vector3(0, -m_speed, 0);

            }
            else
            {
                //右
                m_rb.velocity = new Vector3(m_speed, 0, 0);

            }
        }
        else
        {
            transform.localScale = new Vector3(m_scale, m_scale, m_scale);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // トリガーは処理しない
        if (collision.isTrigger == true)
            return;

        // プレイヤーに当たった
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().AddDamage(m_damage);

            Destroy(gameObject);
            return;
        }

        // ブロックに当たった
        if (collision.CompareTag("Block"))
        {
            Destroy(gameObject);
            return;
        }

    }


}
