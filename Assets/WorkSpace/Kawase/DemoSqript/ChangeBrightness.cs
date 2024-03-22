using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //とりあえず色があるもの
    SpriteRenderer spriteRenderer;
    //松明までの距離
    float toachLength;
    //明るさの最大値
    const int MAX_BRIGHTNESS = 7;

    //松明格納List
    List<GameObject> colList = new List<GameObject>();
    //LightList
    List<GameObject> lightList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //黒くする
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
            //当たった松明の保存
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
        //HSVのカラーの吐き出し用
        float h = 0.0f;
        float s = 0.0f;
        float toachv = 0.0f;

        //例外処理
        if (colList.Count == 0 || !colList.Any() || colList[0] == null)
        {
            
        }
        else
        {
            //一番近い松明の距離
            //松明から自身の距離(初期値として最初のやつ)
            float nearToachLength = Vector3.Distance(colList[0].transform.position, transform.position);

            //一番近い松明の算出
            for (int i = 0; i < colList.Count; i++)
            {
                if (colList[i] == null)
                {
                    continue;
                }
                //より近いなら
                if (nearToachLength > Vector3.Distance(colList[i].transform.position, this.transform.position))
                {
                    nearToachLength = Vector3.Distance(colList[i].transform.position, this.transform.position);
                }
            }


            //なんでかわからんけど2回やんないとバグる？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？
            for (int i = 0; i < 2; i++)
            {

                //松明との距離
                toachLength = nearToachLength;
                //小数点切り上げ
                Mathf.Ceil(toachLength);

                //rgbをhsvに変換
                Color.RGBToHSV(spriteRenderer.color, out h, out s, out toachv);

                //(vの明度が０～１００なので)１００を基準にした一メモリを算出
                int rate = 100 / MAX_BRIGHTNESS;

                //多分０．０～１．０の間なのかな？
                toachv = (100 - rate * toachLength) * 0.01f;



            }

        }


        float lightv = 0.0f;


        //例外処理
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


            //なんでかわからんけど2回やんないとバグる？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？
            for (int i = 0; i < 2; i++)
            {
                //(vの明度が０～１００なので)１００を基準にした一メモリを算出
                int rate = 100 / MAX_BRIGHTNESS;
                //多分０．０～１．０の間なのかな？
                lightv = (rate * lightListV.Max()) * 0.01f;

            }

        }

        if (lightv > toachv)
        {
            //hsvをrgbに変換
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
        //HSVのカラーの吐き出し用
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgbをhsvに変換
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        v = 0.0001f;

        //hsvをrgbに変換
        spriteRenderer.color = Color.HSVToRGB(h, s, v);


    }
    private void FlashLight()
    {
        //ブロックスクリプト餅なら
        if (GetComponent<Block>())
        {
            int lightLevel = GetComponent<Block>().LightLevel;
            //明るさレベルを持ってるか
            if (lightLevel > 0)
            {
                //一瞬コライダー展開
                StartCoroutine(AddCricleColToDelete());

                //HSVのカラーの吐き出し用
                float h = 0.0f;
                float s = 0.0f;
                float v = 0.0f;
                //rgbをhsvに変換
                Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

                //(vの明度が０～１００なので)１００を基準にした一メモリを算出
                int rate = 100 / MAX_BRIGHTNESS;


                //多分０．０～１．０の間なのかな？
                v = (rate * lightLevel) * 0.01f;

                //hsvをrgbに変換
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
