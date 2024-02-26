using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Bomb : Tool
{
    [SerializeField]SpriteRenderer spriteRenderer;

    [SerializeField] float BombTime;

    public override void Initialize()
    {
        Debug.Log("îöíeê›íu");
    }
    public override void ToolUpdate()
    {
        float t = (Mathf.Sin(Time.time * 10) + 1) / 2;

        spriteRenderer.color = Color.Lerp(Color.black,Color.red,t);

        BombTime -= Time.deltaTime;

        if(BombTime < 0)
        {
            Destroy(gameObject);
        }

    }
}
