using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBank : MonoBehaviour
{

    [Header("���Ŏ���")]
    [SerializeField] float destroyTime = 3.0f;
    [Header("�_�ő��x")]
    [SerializeField] int flashSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }

        //�O�`�P
        float alpha = (Mathf.Sin(Time.time * flashSpeed) + 1 ) / 2;


        //�e�܂߂Ďq���ƃ��[�[�v
        foreach (SpriteRenderer child in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            child.color = new Color(child.color.r, child.color.g, child.color.b, alpha);
        }

    }
}
