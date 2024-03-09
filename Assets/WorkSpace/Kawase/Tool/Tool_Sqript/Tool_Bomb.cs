using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Bomb : Tool
{
    [Header("以下子供の設定↓")]
    [Header("意味わからん")]
    [SerializeField]SpriteRenderer spriteRenderer;
    [Header("爆発までの時間")]
    [SerializeField] float bombReadyTime;
    [Header("爆発時間")]
    [SerializeField] float bombTime;
    [Header("爆発半径")]
    [SerializeField] float bombRadius = 3.0f;

    public override void Initialize()
    {
    }
    public override void ToolUpdate()
    {
        float t = (Mathf.Sin(Time.time * 10) + 1) / 2;

        spriteRenderer.color = Color.Lerp(Color.black,Color.red,t);
        if(transform.lossyScale.x >= 1.0f)
        {
            bombReadyTime -= Time.deltaTime;
        }

        if (bombReadyTime < 0)
        {
            //this.GetComponent<CircleCollider2D>().radius = bombRadius;

            bombTime -= Time.deltaTime;

            if (transform.localScale.x <= 1.5f)
            {
                transform.localScale = Vector3.one * bombRadius;

            }
        }

        if(bombTime < 0)
        {
            Destroy(gameObject);
        }

    }
}
