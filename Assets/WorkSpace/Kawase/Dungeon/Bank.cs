using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [Header("�R�E�Q�L�܂ł̎���")]
    [SerializeField] float attackTime = 3.0f;
    [Header("�R�E�Q�L����ł܂ł̎���")]
    [SerializeField] float destroyTime = 3.0f;

    float y = 0;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1.0f,0.0f,1.0f);
        GetComponent<PolygonCollider2D>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        attackTime -= Time.deltaTime;

        if (attackTime < 0)
        {
            destroyTime -= Time.deltaTime;

            if(y < 1)
                y += Time.deltaTime * 2;

            GetComponent<PolygonCollider2D>().enabled = true;
        }

        transform.localScale = new Vector3(1.0f, y, 1.0f);

        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
