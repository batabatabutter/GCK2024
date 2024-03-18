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

    //�����i�[List
    List<GameObject> colList = new List<GameObject>();


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

        //�O�ɂ���ƐF��������
        v = 0.001f;

        //hsv��rgb�ɕϊ�
        spriteRenderer.color = Color.HSVToRGB(h, s, v);


    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < colList.Count; i++)
        {
            if (colList[i] == null)
            {

                colList.RemoveAt(i);

                if(colList.Count == 0 )
                {
                    ChangeBlack();

                }
                else
                {
                    ChangeColor();
                }
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tool_Toach>())
        {

            //�������������̕ۑ�
            colList.Add(collision.gameObject);


            ChangeColor();
        }
    }


    private void ChangeColor()
    {
        //��O����
        if (colList[0] == null)
        {
            return;
        }

        //��ԋ߂������̋���
        //�������玩�g�̋���(�����l�Ƃ��čŏ��̂��)
        float nearToachLength = Vector3.Distance(colList[0].transform.position, this.transform.position);

        //��ԋ߂������̎Z�o
        for (int i = 0; i < colList.Count; i++)
        {
            if (colList[i] == null)
            {
                continue;

            }
            //���߂��Ȃ�
            if (nearToachLength > Vector3.Distance(colList[i].transform.position, this.transform.position))
            {
                nearToachLength = Vector3.Distance(colList[i].transform.position, this.transform.position);
            }
        }

        //�Ȃ�ł��킩��񂯂�2����Ȃ��ƃo�O��H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H
        for (int i = 0; i < 2; i++)
        {

            //�����Ƃ̋���
            toachLength = nearToachLength;
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

    private void ChangeBlack()
    {
        //HSV�̃J���[�̓f���o���p
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgb��hsv�ɕϊ�
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        v = 0.0001f;

        //hsv��rgb�ɕϊ�
        spriteRenderer.color = Color.HSVToRGB(h, s, v);


    }

}
