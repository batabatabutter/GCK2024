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

        v = 0.001f;

        //hsvをrgbに変換
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
            //松明との距離
            toachLength = Vector3.Distance(collision.transform.position, this.transform.position);
            //小数点切り上げ
            Mathf.Ceil(toachLength);


            //HSVのカラーの吐き出し用
            float h = 0.0f;
            float s = 0.0f;
            float v = 0.0f;
            //rgbをhsvに変換
            Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);



            //(vの明度が０〜１００なので)１００を基準にした一メモリを算出
            int rate = 100 / MaxBrightness;

            //多分０．０〜１．０の間なのかな？
            v = (100 - rate * toachLength) / 100;



            //hsvをrgbに変換
            spriteRenderer.color = Color.HSVToRGB(h, s, v);
        }

    }

}
