using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Bomb : Tool
{
    [Header("ˆÈ‰ºŽq‹Ÿ‚ÌÝ’è«")]
    [Header("ˆÓ–¡‚í‚©‚ç‚ñ")]
    [SerializeField]SpriteRenderer spriteRenderer;
    [Header("”š”­ŽžŠÔ")]
    [SerializeField] float BombTime;

    public override void Initialize()
    {
        Debug.Log("”š’eÝ’u");
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
