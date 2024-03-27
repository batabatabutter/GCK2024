using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //‚Æ‚è‚ ‚¦‚¸F‚ª‚ ‚é‚à‚Ì
    SpriteRenderer spriteRenderer;
    //–¾‚é‚³‚ÌÅ‘å’l
    const int MAX_BRIGHTNESS = 7;
    //LightList
    public List<GameObject> m_lightList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //•‚­‚·‚é
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
        //ŒõŒ¹‚Í‚±‚Ìˆ—‚ğ‚µ‚È‚¢
        if (GetComponent<Block>())
        {
            if (GetComponent<Block>().LightLevel > 0)
            {
                return;
            }
        }

        //HSV‚ÌƒJƒ‰[‚Ì“f‚«o‚µ—p
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;


        //—áŠOˆ—
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
            //‚È‚ñ‚Å‚©‚í‚©‚ç‚ñ‚¯‚Ç2‰ñ‚â‚ñ‚È‚¢‚ÆƒoƒO‚é
            for (int i = 0; i < 2; i++)
            {
                //(v‚Ì–¾“x‚ª‚O`‚P‚O‚O‚È‚Ì‚Å)‚P‚O‚O‚ğŠî€‚É‚µ‚½ˆêƒƒ‚ƒŠ‚ğZo
                int rate = 100 / MAX_BRIGHTNESS;
                //‘½•ª‚OD‚O`‚PD‚O‚ÌŠÔ‚È‚Ì‚©‚ÈH
                v = (rate * lightListV.Max()) * 0.01f;
                //hsv‚ğrgb‚É•ÏŠ·
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }

        }
    }

    private void ChangeBlack()
    {
        //HSV‚ÌƒJƒ‰[‚Ì“f‚«o‚µ—p
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgb‚ğhsv‚É•ÏŠ·
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        v = 0.0001f;

        //hsv‚ğrgb‚É•ÏŠ·
        spriteRenderer.color = Color.HSVToRGB(h, s, v);


    }
    private void FlashLight()
    {
        //ƒuƒƒbƒNƒXƒNƒŠƒvƒg–İ‚È‚ç
        if (GetComponent<Block>())
        {
            int lightLevel = GetComponent<Block>().LightLevel;
            //–¾‚é‚³ƒŒƒxƒ‹‚ğ‚Á‚Ä‚é‚©
            if (lightLevel > 0)
            {
                //ˆêuƒRƒ‰ƒCƒ_[“WŠJ
                StartCoroutine(AddCricleColToDelete());

                //HSV‚ÌƒJƒ‰[‚Ì“f‚«o‚µ—p
                float h = 0.0f;
                float s = 0.0f;
                float v = 0.0f;
                //rgb‚ğhsv‚É•ÏŠ·
                Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

                //(v‚Ì–¾“x‚ª‚O`‚P‚O‚O‚È‚Ì‚Å)‚P‚O‚O‚ğŠî€‚É‚µ‚½ˆêƒƒ‚ƒŠ‚ğZo
                int rate = 100 / MAX_BRIGHTNESS;


                //‘½•ª‚OD‚O`‚PD‚O‚ÌŠÔ‚È‚Ì‚©‚ÈH
                v = (rate * lightLevel) * 0.01f;

                //hsv‚ğrgb‚É•ÏŠ·
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }
        }
    }


    private IEnumerator AddCricleColToDelete()
    {
        bool isDeleteRb = false;

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        //‚à‚µ‚È‚©‚Á‚½‚ç
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
