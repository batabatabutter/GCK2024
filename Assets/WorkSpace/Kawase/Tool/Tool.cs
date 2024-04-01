using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    void Start()
    {
        //子供の初期化
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //子供の更新
        ToolUpdate();
    }
    public void Plant(Vector3 plantPos)
    {
        Instantiate(this, plantPos, Quaternion.identity);
    }

    public virtual void Initialize()
    {
    }

    public virtual void ToolUpdate()
    {
    }

    // ツールを使用する
    public virtual void UseTool(GameObject obj)
    {
        Debug.Log("ツール使用");
    }

}
