using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //�Ƃ肠�����F���������
    SpriteRenderer spriteRenderer;
    //�����܂ł̋���
    float toachLength;

    //���邳�̍ŏ��l
    [Header("���邳�̍ŏ��l")]
    [SerializeField] int MinBrightness;
    //���邳�̍ő�l
    [Header("���邳�̍ő�l(�O�͂���ׂ�)")]
    [SerializeField] int MaxBrightness = 7;


    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


        //HSV�̃J���[�̓f���o���p
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgb��hsv�ɕϊ�
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        v = 0.001f;

        //hsv��rgb�ɕϊ�
        spriteRenderer.color = Color.HSVToRGB(h, s, v);


    }

    // Update is called once per frame
    void Update()
    {


    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Tool_Toach>())
        {
            //�����Ƃ̋���
            toachLength = Vector3.Distance(collision.transform.position, this.transform.position);
            //�����_�؂�グ
            Mathf.Ceil(toachLength);


            //HSV�̃J���[�̓f���o���p
            float h = 0.0f;
            float s = 0.0f;
            float v = 0.0f;
            //rgb��hsv�ɕϊ�
            Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);



            //(v�̖��x���O�`�P�O�O�Ȃ̂�)�P�O�O����ɂ����ꃁ�������Z�o
            int rate = 100 / MaxBrightness;

            //�����O�D�O�`�P�D�O�̊ԂȂ̂��ȁH
            v = (100 - rate * toachLength) / 100;



            //hsv��rgb�ɕϊ�
            spriteRenderer.color = Color.HSVToRGB(h, s, v);
        }

    }

}
