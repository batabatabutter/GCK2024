using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    //�ݒu�N�[���^�C��
    [Header("�ݒu�N�[���^�C��")][SerializeField] float plantCooltime;
    //�ݒu�N�[���^�C���̍ő�l
    [NonSerialized] public float maxCoolTime;
    void Start()
    {
        //�q���̏�����
        Initialize();
        //�N�[���^�C���̍ő�l��ۑ�
        maxCoolTime = plantCooltime;
    }

    // Update is called once per frame
    void Update()
    {
        //�v�����g���̑傫���ω�����
        ChangePlantScale();
        //�q���̍X�V
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
