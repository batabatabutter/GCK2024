using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    //設置クールタイム
    [Header("設置クールタイム")][SerializeField] float plantCooltime;
    //設置クールタイムの最大値
    [NonSerialized] public float maxCoolTime;
    void Start()
    {
        //子供の初期化
        Initialize();
        //クールタイムの最大値を保存
        maxCoolTime = plantCooltime;
    }

    // Update is called once per frame
    void Update()
    {
        //プラント中の大きさ変化処理
        ChangePlantScale();
        //子供の更新
        ToolUpdate();
    }
    public void Plant(Vector3 plantPos)
    {
        Instantiate(this, plantPos, Quaternion.identity);
    }

    private void ChangePlantScale()
    {
        if (plantCooltime > 0.0f)
        {
            plantCooltime -= Time.deltaTime;
            float scale = 1 - plantCooltime / maxCoolTime;
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    public virtual void Initialize()
    {
    }

    public virtual void ToolUpdate()
    {
    }

}
