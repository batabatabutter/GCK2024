using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Bomb : Tool
{
    [Header("ˆÈ‰ºq‹Ÿ‚Ìİ’è«")]
    [Header("ˆÓ–¡‚í‚©‚ç‚ñ")]
    [SerializeField]SpriteRenderer spriteRenderer;
    [Header("”š”­‚Ü‚Å‚ÌŠÔ")]
    [SerializeField] float bombReadyTime;
    [Header("”š”­ŠÔ")]
    [SerializeField] float bombTime;
    [Header("”š”­”¼Œa")]
    [SerializeField] float bombRadius = 3.0f;

    public override void Initialize()
    {
    }
    public override void ToolUpdate()
    {
        float t = (Mathf.Sin(Time.time * 10) + 1) / 2;

        spriteRenderer.color = Color.Lerp(Color.black,Color.red,t);

    }
}
