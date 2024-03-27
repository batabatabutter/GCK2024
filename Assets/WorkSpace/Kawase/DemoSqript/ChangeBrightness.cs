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
    //���邳�̍ő�l
    const int MAX_BRIGHTNESS = 7;
    //LightList
    public List<GameObject> m_lightList = new List<GameObject>();

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

        for (int i = 0; i < m_lightList.Count; i++)
        {
            if (m_lightList[i] == null)
            {

                m_lightList.RemoveAt(i);

                if (m_lightList.Count == 0)
                {
                    ChangeBlack();

                }
                else
                {
                    ChangeColor();

                }
            }
        }

        ChangeColor();

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Block>())
        {
            if ((collision.gameObject.layer == 3 || collision.CompareTag("Block")) && GetComponent<Block>().LightLevel < 1)
            {
                m_lightList.Add(collision.gameObject);
                ChangeColor();
            }
        }
        else if(GetComponent<ChangeBrightness>() && (collision.gameObject.layer == 3 || collision.CompareTag("Block")))
        {
            m_lightList.Add(collision.gameObject);
            ChangeColor();

        }



    }


    private void ChangeColor()
    {
        //�����͂��̏��������Ȃ�
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
        float v = 0.0f;


        //��O����
        if ((m_lightList.Count == 0 || !m_lightList.Any() || m_lightList[0] == null || gameObject == null))
        {
            return;
        }
        else
        {
            List<float> lightListV = new List<float>();

            for (int i = 0; i < m_lightList.Count; i++)
            {
                if (m_lightList[i] == null)
                    continue;

                float lightLength = Mathf.Ceil(Vector3.Distance(m_lightList[i].transform.position, this.transform.position));

                lightListV.Add(m_lightList[i].GetComponent<Block>().LightLevel - lightLength);
            }
            //�Ȃ�ł��킩��񂯂�2����Ȃ��ƃo�O��
            for (int i = 0; i < 2; i++)
            {
                //(v�̖��x���O�`�P�O�O�Ȃ̂�)�P�O�O����ɂ����ꃁ�������Z�o
                int rate = 100 / MAX_BRIGHTNESS;
                //�����O�D�O�`�P�D�O�̊ԂȂ̂��ȁH
                v = (rate * lightListV.Max()) * 0.01f;
                //hsv��rgb�ɕϊ�
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }

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
        bool isDeleteRb = false;

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        //�����Ȃ�������
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            isDeleteRb = true;
        }

        rb.isKinematic = true;


        CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();

        circleCol.radius = GetComponent<Block>().LightLevel;
        circleCol.isTrigger = true;

        yield return new WaitForSeconds(0.1f);

        Destroy(circleCol);
        if(isDeleteRb)
        {
            Destroy(rb);
        }

    }

}
