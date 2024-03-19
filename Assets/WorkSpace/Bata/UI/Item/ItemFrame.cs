using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : MonoBehaviour
{
    //  �摜
    [SerializeField] private UnityEngine.UI.Image m_image;

    //  �摜
    [SerializeField] private Text m_num;

    //  �摜�ݒ�
    public void SetImage(Sprite image) { m_image.sprite = image;}
    //  �摜�擾
    public Sprite GetImage() { return m_image.sprite;}

    //  �������ݒ�
    public void SetNum(int num) { m_num.text = num.ToString(); }
    //  �������擾
    public int GetNum() { return int.Parse(m_num.text);}
}
