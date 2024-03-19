using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Bomb : Tool
{
    [Header("�ȉ��q���̐ݒ聫")]
    [Header("�Ӗ��킩���")]
    [SerializeField]SpriteRenderer spriteRenderer;
    [Header("�����܂ł̎���")]
    [SerializeField] float bombReadyTime;
    [Header("��������")]
    [SerializeField] float bombTime;
    [Header("�������a")]
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
