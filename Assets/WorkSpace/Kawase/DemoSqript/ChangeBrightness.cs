using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //�Ƃ肠�����F���������
    SpriteRenderer spriteRenderer;
    //�����܂ł̋���
    float toachLength;
    //���邳�̍ő�l
    const int MAX_BRIGHTNESS = 7;

    //�����i�[List
    List<GameObject> colList = new List<GameObject>();
    //LightList
    List<GameObject> lightList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //��������
        ChangeBlack();


        FlashLight();

        

    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Block>())
        {
            if (GetComponent<Block>().LightLevel > 0)
            {
                return;
            }
        }


        for (int i = 0; i < colList.Count; i++)
        {
            if (colList[i] == null)
            {

                colList.RemoveAt(i);

                if (colList.Count == 0 && lightList.Count == 0)
                {
                    ChangeBlack();

                }
                else
                {
                    ChangeColor();

                }
            }
        }

        for (int i = 0; i < lightList.Count; i++)
        {
            if (lightList[i] == null)
            {

                lightList.RemoveAt(i);

                if (colList.Count == 0 && lightList.Count == 0)
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


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tool_Toach>())
        {
            //�������������̕ۑ�
            colList.Add(collision.gameObject);
            ChangeColor();

        }

        if (GetComponent<Block>())
        {
            if (collision.CompareTag("Block") && GetComponent<Block>().LightLevel < 1)
            {
                lightList.Add(collision.gameObject);
                ChangeColor();

            }
        }
    }


    private void ChangeColor()
    {
        if (GetComponent<Block>())
        {
            if (GetComponent<Block>().LightLevel > 0)
            {
                return;
            }
        }
        //HSV�̃J���[�̓f���o���p
        float h = 0.0f;
        float s = 0.0f;
        float toachv = 0.0f;

        //��O����
        if (colList.Count == 0 || !colList.Any() || colList[0] == null)
        {
            
        }
        else
        {
            //��ԋ߂������̋���
            //�������玩�g�̋���(�����l�Ƃ��čŏ��̂��)
            float nearToachLength = Vector3.Distance(colList[0].transform.position, transform.position);

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

                //rgb��hsv�ɕϊ�
                Color.RGBToHSV(spriteRenderer.color, out h, out s, out toachv);

                //(v�̖��x���O�`�P�O�O�Ȃ̂�)�P�O�O����ɂ����ꃁ�������Z�o
                int rate = 100 / MAX_BRIGHTNESS;

                //�����O�D�O�`�P�D�O�̊ԂȂ̂��ȁH
                toachv = (100 - rate * toachLength) * 0.01f;



            }

        }


        float lightv = 0.0f;


        //��O����
        if ((lightList.Count == 0 || !lightList.Any() || lightList[0] == null))
        {

        }
        else
        {



            List<float> lightListV = new List<float>();

            for (int i = 0; i < lightList.Count; i++)
            {
                float lightLength = Mathf.Ceil(Vector3.Distance(lightList[i].transform.position, this.transform.position));

                lightListV.Add(lightList[i].GetComponent<Block>().LightLevel - lightLength);
            }


            //�Ȃ�ł��킩��񂯂�2����Ȃ��ƃo�O��H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H�H
            for (int i = 0; i < 2; i++)
            {
                //(v�̖��x���O�`�P�O�O�Ȃ̂�)�P�O�O����ɂ����ꃁ�������Z�o
                int rate = 100 / MAX_BRIGHTNESS;
                //�����O�D�O�`�P�D�O�̊ԂȂ̂��ȁH
                lightv = (rate * lightListV.Max()) * 0.01f;

            }

        }

        if (lightv > toachv)
        {
            //hsv��rgb�ɕϊ�
            spriteRenderer.color = Color.HSVToRGB(h, s, lightv);
            spriteRenderer.color = Color.HSVToRGB(h, s, lightv);
        }
        else
        {
            spriteRenderer.color = Color.HSVToRGB(h, s, toachv);
            spriteRenderer.color = Color.HSVToRGB(h, s, toachv);

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
    private void FlashLight()
    {
        //�u���b�N�X�N���v�g�݂Ȃ�
        if (GetComponent<Block>())
        {
            int lightLevel = GetComponent<Block>().LightLevel;
            //���邳���x���������Ă邩
            if (lightLevel > 0)
            {
                //��u�R���C�_�[�W�J
                StartCoroutine(AddCricleColToDelete());

                //HSV�̃J���[�̓f���o���p
                float h = 0.0f;
                float s = 0.0f;
                float v = 0.0f;
                //rgb��hsv�ɕϊ�
                Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

                //(v�̖��x���O�`�P�O�O�Ȃ̂�)�P�O�O����ɂ����ꃁ�������Z�o
                int rate = 100 / MAX_BRIGHTNESS;


                //�����O�D�O�`�P�D�O�̊ԂȂ̂��ȁH
                v = (rate * lightLevel) * 0.01f;

                //hsv��rgb�ɕϊ�
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }
        }
    }


    private IEnumerator AddCricleColToDelete()
    {

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();

        rb.isKinematic = true;


        CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();

        circleCol.radius = GetComponent<Block>().LightLevel;
        circleCol.isTrigger = true;

        yield return new WaitForSeconds(0.1f);

        Destroy(circleCol);
        Destroy(rb);

    }

}
