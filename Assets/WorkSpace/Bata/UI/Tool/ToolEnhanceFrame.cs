using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolEnhanceFrame : MonoBehaviour
{
    [Header("UIのオブジェクト")]
    [SerializeField] private Image m_icon;
    [SerializeField] private Image m_bar;
    [SerializeField] private Image m_barFrame;
    [Header("色の変化")]
    [SerializeField] private Color m_startColor = Color.white;
    [SerializeField] private Color m_endColor = Color.white;

    //  ツール情報
    private ToolMining m_toolMining = null;

    // Update is called once per frame
    void Update()
    {
        //  強化されてなかったら処理しない
        if (!m_toolMining.IsEnhance)
        {
            m_bar.transform.localScale = Vector3.zero;
            transform.localScale = Vector3.zero;
            return;
        }

        //  情報から大きさ設定
        transform.localScale = Vector3.one;
        m_bar.transform.localScale = new Vector3(
            1.0f - (m_toolMining.PlayerMining.TakenDamage - m_toolMining.StartTakenDMG) /
            (m_toolMining.AmountValue - m_toolMining.StartTakenDMG),
            1.0f, 1.0f);

        //  大きさによって色変化
        m_bar.color = Color.Lerp(m_startColor, m_endColor, 1 - m_bar.transform.localScale.x);
    }

    public Image ToolIcon
    { 
        get { return m_icon; } 
        set { m_icon = value; }
    }

    public Image Bar
    {
        get { return m_bar; }
        set { m_bar = value; }
    }

    public Image BarFrame
    { 
        get { return m_barFrame; }
        set { m_barFrame = value; }
    }

    public ToolMining ToolMining 
    { 
        get { return m_toolMining; } 
        set { m_toolMining = value; }
    }
}
