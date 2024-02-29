using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //‚Æ‚è‚ ‚¦‚¸F‚ª‚ ‚é‚à‚Ì
    SpriteRenderer spriteRenderer;
    //¼–¾‚Ü‚Å‚Ì‹——£
    float toachLength;

    //–¾‚é‚³‚ÌÅ¬’l
    [Header("–¾‚é‚³‚ÌÅ¬’l")]
    [SerializeField] int MinBrightness;
    //–¾‚é‚³‚ÌÅ‘å’l
    [Header("–¾‚é‚³‚ÌÅ‘å’l(‚O‚Í‚©‚ñ‚×‚ñ)")]
    [SerializeField] int MaxBrightness = 7;


    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


        //HSV‚ÌƒJƒ‰[‚Ì“f‚«o‚µ—p
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgb‚ğhsv‚É•ÏŠ·
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        v = 0.001f;

        //hsv‚ğrgb‚É•ÏŠ·
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
            //¼–¾‚Æ‚Ì‹——£
            toachLength = Vector3.Distance(collision.transform.position, this.transform.position);
            //¬”“_Ø‚èã‚°
            Mathf.Ceil(toachLength);


            //HSV‚ÌƒJƒ‰[‚Ì“f‚«o‚µ—p
            float h = 0.0f;
            float s = 0.0f;
            float v = 0.0f;
            //rgb‚ğhsv‚É•ÏŠ·
            Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);



            //(v‚Ì–¾“x‚ª‚O`‚P‚O‚O‚È‚Ì‚Å)‚P‚O‚O‚ğŠî€‚É‚µ‚½ˆêƒƒ‚ƒŠ‚ğZo
            int rate = 100 / MaxBrightness;

            //‘½•ª‚OD‚O`‚PD‚O‚ÌŠÔ‚È‚Ì‚©‚ÈH
            v = (100 - rate * toachLength) / 100;



            //hsv‚ğrgb‚É•ÏŠ·
            spriteRenderer.color = Color.HSVToRGB(h, s, v);
        }

    }

}
