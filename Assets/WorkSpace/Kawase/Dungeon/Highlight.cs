using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] FallRock fallRock;

    float scale;

    float alpha;

    float fallTime;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;

        scale = 0.0f;

        alpha = 0.5f;

        fallTime = fallRock.fallTime;

    }

    // Update is called once per frame
    void Update()
    {
        fallTime -= Time.deltaTime;

        if(fallTime > 0)
        {
            scale += Time.deltaTime / 3;


        }
        else
        {
            alpha += Time.deltaTime / 1.5f;
        }

        if(transform.lossyScale.x < 1.0f)
        {
            transform.transform.localScale  = new Vector3(scale, scale / 2, scale);
        }

        GetComponent<SpriteRenderer>().color = new Color(1.0f,0.0f,0.0f,alpha);


        if(alpha > 1.0f)
        {
            Destroy(gameObject);
        }

    }
}
