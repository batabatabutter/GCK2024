using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolFrame : MonoBehaviour
{
    //  画像
    [SerializeField] private UnityEngine.UI.Image m_image;
    //  フレーム色
    [SerializeField] private Image m_frameColor;
    //  背景色
    [SerializeField] private Image m_backColor;
    //  使用可能数
    [SerializeField] private Text m_num;
    //  使用可能数背景
    [SerializeField] private Image m_numBack;

    //  色
    [Header("色指定")]
    [SerializeField] private Color m_isNoneSelectedFrameColor;
    [SerializeField] private Color m_isSelectedFrameColor;
    [SerializeField] private Color m_canUsedBackColor;
    [SerializeField] private Color m_cantUsedBackColor;
    [SerializeField] private Color m_canUsedNumBackColor;
    [SerializeField] private Color m_cantUsedNumBackColor;

    //  状態設定
    private bool m_isSelected = false;

    //  開始
    private void Start()
    {
        m_backColor.color = m_cantUsedBackColor;
        m_numBack.color = m_cantUsedNumBackColor;
        m_frameColor.color = m_isNoneSelectedFrameColor;
    }

    //  画像設定
    public void SetImage(Sprite image) { m_image.sprite = image; }
    //  画像取得
    public Sprite GetImage() { return m_image.sprite; }

    //  使用可能数設定
    public void SetNum(int num)
    {
        m_num.text = num.ToString();
        //  使用可能数がによって背景色変更
        if (num > 0)
        {
            m_backColor.color = m_canUsedBackColor;
            m_numBack.color = m_canUsedNumBackColor;
        }
        else
        {
            m_backColor.color = m_cantUsedBackColor;
            m_numBack.color = m_cantUsedNumBackColor;
        }
    }
    //  使用可能数取得
    public int GetNum() { return int.Parse(m_num.text); }

    //  選択状態設定
    public void SetIsSelected(bool flag) 
    { 
        m_isSelected = flag;
        //  選ばれてるかでフレーム色変更
        if (flag) m_frameColor.color = m_isSelectedFrameColor;
        else m_frameColor.color = m_isNoneSelectedFrameColor;
    }
}
