using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    void Start()
    {
        //�q���̏�����
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //�q���̍X�V
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

}
