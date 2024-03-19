using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollRock : MonoBehaviour
{

    [Header("�]����܂ł̎���")]
    [SerializeField] float rollTime = 3.0f;
    [Header("���ł܂ł̎���")]
    [SerializeField] float destroyTime = 10.0f;
    [Header("���x�im/s�j")]
    [SerializeField] float speed = 5.0f;
    [Header("�U����")]
    public int damage = 1;

    int rota;

    float scale = 0.0f;

    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rota = (int)transform.localEulerAngles.z;
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        rollTime -= Time.deltaTime;
        destroyTime -= Time.deltaTime;

        if( destroyTime < 0 )
        {
            Destroy(gameObject);
        }

        if (transform.lossyScale.x < 1.0f)
        {
            scale += Time.deltaTime * 0.5f;
        }

        if (rollTime < 0)
        {
            if(rota == 0)
            {
                //��ɍs��
                rb.velocity = new Vector3(0, speed, 0);
            }
            else if(rota == 90)
            {
                //��
                rb.velocity = new Vector3(-speed, 0, 0);


            }
            else if(rota == 180)
            {
                //��
                rb.velocity = new Vector3(0, -speed, 0);

            }
            else
            {
                //�E
                rb.velocity = new Vector3(speed, 0, 0);

            }
        }
        else
        {
            transform.localScale = new Vector3(scale, scale, scale);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.isTrigger)
        {
            collision.GetComponent<Player>().AddDamage(damage);

            Destroy(gameObject);
        }


    }


}
