using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBank : MonoBehaviour
{

    [Header("消滅時間")]
    [SerializeField] float destroyTime = 3.0f;
    [Header("点滅速度")]
    [SerializeField] int flashSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }

        //０～１
        float alpha = (Mathf.Sin(Time.time * flashSpeed) + 1 ) / 2;


        //親含めて子供とルーープ
        foreach (SpriteRenderer child in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            child.color = new Color(child.color.r, child.color.g, child.color.b, alpha);
        }

    }
}
