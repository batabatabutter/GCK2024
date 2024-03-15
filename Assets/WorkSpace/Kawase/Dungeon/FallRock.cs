using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FallRock : MonoBehaviour
{
    [Header("óéâ∫Ç‹Ç≈ÇÃéûä‘")]
    public float fallTime = 3.0f;
    [Header("çUåÇóÕ")]
    public int damage = 1;

    Rigidbody2D rd;

    float scale = 0.0f;

    float keepX;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();

        rd.gravityScale = 0.0f;

        transform.localScale = Vector3.zero;

        keepX = transform.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        fallTime -= Time.deltaTime;

        if(transform.lossyScale.x < 1.0f)
        {
            scale += Time.deltaTime * 0.5f;
        }

        if(fallTime < 0)
        {
            rd.gravityScale = 1.0f;

            transform.position = new Vector3(keepX,transform.position.y,transform.position.z);
        }
        else
        {
            transform.localScale = new Vector3(scale,scale,scale);

            transform.position = new Vector3(keepX + (Mathf.Sin(Time.time * 20) / 4), transform.position.y, transform.position.z);
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().AddDamage(damage);

            Destroy(gameObject);

        }
        if (collision.gameObject.GetComponent<Highlight>())
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);

        }
    }



}
