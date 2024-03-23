using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolFrame : MonoBehaviour
{
    //  �摜
    [SerializeField] private UnityEngine.UI.Image m_image;
    //  �t���[���F
    [SerializeField] private Image m_frameColor;
    //  �w�i�F
    [SerializeField] private Image m_backColor;
    //  �g�p�\��
    [SerializeField] private Text m_num;
    //  �g�p�\���w�i
    [SerializeField] private Image m_numBack;

    //  �F
    [Header("�F�w��")]
    [SerializeField] private Color m_isNoneSelectedFrameColor;
    [SerializeField] private Color m_isSelectedFrameColor;
    [SerializeField] private Color m_canUsedBackColor;
    [SerializeField] private Color m_cantUsedBackColor;
    [SerializeField] private Color m_canUsedNumBackColor;
    [SerializeField] private Color m_cantUsedNumBackColor;

    //  ��Ԑݒ�
    private bool m_isSelected = false;

    //  �J�n
    private void Start()
    {
        m_backColor.color = m_cantUsedBackColor;
        m_numBack.color = m_cantUsedNumBackColor;
        m_frameColor.color = m_isNoneSelectedFrameColor;
    }

    //  �摜�ݒ�
    public void SetImage(Sprite image) { m_image.sprite = image; }
    //  �摜�擾
    public Sprite GetImage() { return m_image.sprite; }

    //  �g�p�\���ݒ�
    public void SetNum(int num)
    {
        m_num.text = num.ToString();
        //  �g�p�\�����ɂ���Ĕw�i�F�ύX
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
    //  �g�p�\���擾
    public int GetNum() { return int.Parse(m_num.text); }

    //  �I����Ԑݒ�
    public void SetIsSelected(bool flag) 
    { 
        m_isSelected = flag;
        //  �I�΂�Ă邩�Ńt���[���F�ύX
        if (flag) m_frameColor.color = m_isSelectedFrameColor;
        else m_frameColor.color = m_isNoneSelectedFrameColor;
    }
}
