using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightArrow : MonoBehaviour
{

    Vector3 fastPos;

    [Header("êUÇÍïù")]
    [SerializeField] float range = 1.0f;
    [Header("è¡ñ≈éûä‘")]
    [SerializeField] float destroyTime = 3.0f;


    float rota;

    // Start is called before the first frame update
    void Start()
    {
        fastPos = transform.position;

        rota = Mathf.Floor(transform.rotation.z * 10) / 10;

    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if(destroyTime < 0)
        {
            Destroy(gameObject);
        }

        if((rota > 0.6 && rota < 1))
            transform.position = new Vector3(fastPos.x + Mathf.Sin(Time.time * 5) * range, fastPos.y,fastPos.z);
        else
            transform.position = new Vector3(fastPos.x, fastPos.y + Mathf.Sin(Time.time * 5) * range, fastPos.z);


    }
}
