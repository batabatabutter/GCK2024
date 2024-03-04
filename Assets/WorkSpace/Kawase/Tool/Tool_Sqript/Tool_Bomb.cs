using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Bomb : Tool
{
    [Header("ˆÈ‰ºŽq‹Ÿ‚ÌÝ’è«")]
    [Header("ˆÓ–¡‚í‚©‚ç‚ñ")]
    [SerializeField]SpriteRenderer spriteRenderer;
    [Header("”š”­‚Ü‚Å‚ÌŽžŠÔ")]
    [SerializeField] float bombReadyTime;
    [Header("”š”­ŽžŠÔ")]
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
