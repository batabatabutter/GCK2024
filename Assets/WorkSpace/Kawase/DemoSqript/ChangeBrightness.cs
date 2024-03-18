using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //とりあえず色があるもの
    SpriteRenderer spriteRenderer;
    //松明までの距離
    float toachLength;

    //明るさの最小値
    [Header("明るさの最小値")]
    [SerializeField] int MinBrightness;
    //明るさの最大値
    [Header("明るさの最大値(０はかんべん)")]
    [SerializeField] int MaxBrightness = 7;

    //松明格納List
    List<GameObject> colList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


        //HSVのカラーの吐き出し用
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgbをhsvに変換
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        //０にすると色が消える
        v = 0.001f;

        //hsvをrgbに変換
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

            //当たった松明の保存
            colList.Add(collision.gameObject);


            ChangeColor();
        }
    }


    private void ChangeColor()
    {
        //例外処理
        if (colList[0] == null)
        {
            return;
        }

        //一番近い松明の距離
        //松明から自身の距離(初期値として最初のやつ)
        float nearToachLength = Vector3.Distance(colList[0].transform.position, this.transform.position);

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

            //HSVのカラーの吐き出し用
            float h = 0.0f;
            float s = 0.0f;
            float v = 0.0f;
            //rgbをhsvに変換
            Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

            //(vの明度が０～１００なので)１００を基準にした一メモリを算出
            int rate = 100 / MaxBrightness;

            //多分０．０～１．０の間なのかな？
            v = (100 - rate * toachLength) / 100;

            //hsvをrgbに変換
            spriteRenderer.color = Color.HSVToRGB(h, s, v);
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

}
