using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : MonoBehaviour
{
    //  画像
    [SerializeField] private UnityEngine.UI.Image m_image;

    //  画像
    [SerializeField] private Text m_num;

    //  画像設定
    public void SetImage(Sprite image) { m_image.sprite = image;}
    //  画像取得
    public Sprite GetImage() { return m_image.sprite;}

    //  所持数設定
    public void SetNum(int num) { m_num.text = num.ToString(); }
    //  所持数取得
    public int GetNum() { return int.Parse(m_num.text);}
}
