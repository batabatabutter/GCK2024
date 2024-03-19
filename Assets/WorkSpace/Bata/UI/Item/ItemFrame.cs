using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : MonoBehaviour
{
    //  ‰æ‘œ
    [SerializeField] private UnityEngine.UI.Image m_image;

    //  ‰æ‘œ
    [SerializeField] private Text m_num;

    //  ‰æ‘œİ’è
    public void SetImage(Sprite image) { m_image.sprite = image;}
    //  ‰æ‘œæ“¾
    public Sprite GetImage() { return m_image.sprite;}

    //  Š”İ’è
    public void SetNum(int num) { m_num.text = num.ToString(); }
    //  Š”æ“¾
    public int GetNum() { return int.Parse(m_num.text);}
}
