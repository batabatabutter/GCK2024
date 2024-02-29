using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Bomb : Tool
{
    [Header("以下子供の設定↓")]
    [Header("意味わからん")]
    [SerializeField]SpriteRenderer spriteRenderer;
    [Header("爆発時間")]
    [SerializeField] float BombTime;

    public override void Initialize()
    {
        Debug.Log("爆弾設置");
    }
    public override void ToolUpdate()
    {
        float t = (Mathf.Sin(Time.time * 10) + 1) / 2;

        spriteRenderer.color = Color.Lerp(Color.black,Color.red,t);
        if(transform.lossyScale.x >= 1.0f)
        {
            BombTime -= Time.deltaTime;
        }

        if (BombTime < 0)
        {
            Destroy(gameObject);
        }

    }
}
