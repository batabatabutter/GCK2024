using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [Header("演出用画像")]
    [SerializeField] private Image m_img = null;

    [Header("肉ダメージ色")]
    [SerializeField] private Color m_dmgColor = Color.white;
    [Header("鎧ダメージ色")]
    [SerializeField] private Color m_armorDmgColor = Color.white;

    [Header("フェードアウト時間")]
    [SerializeField] private float m_fadeOutTime = 0.0f;

    //  シーンマネージャー
    private PlaySceneManager m_playSceneManager;
    //  プレイヤー
    private Player m_player;

    //  時間カウント用
    private float m_timeCount = 0.0f;

    /// <summary>
    /// 開始時
    /// </summary>
    private void Start()
    {
        //  プレイシーンマネージャー設定
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        m_player.DmgEvents.AddListener(GenerateEffect);
    }

    // Update is called once per frame
    private void Update()
    {
        //  時間減算
        if(m_timeCount > 0) m_timeCount = Mathf.Max(m_timeCount - Time.deltaTime, 0.0f);

        //  エフェクト色
        Color col = m_img.color;
        col.a = m_timeCount / m_fadeOutTime;
        m_img.color = col;

        if(Input.GetKey(KeyCode.P))
        {
            GenerateEffect();        
        }
    }

    /// <summary>
    /// ダメージを食らった時のエフェクト再生
    /// </summary>
    private void GenerateEffect()
    {
        //if (isArmor) m_img.color = m_armorDmgColor;
        //else
        m_img.color = m_dmgColor;
        m_timeCount = m_fadeOutTime;
    }

    //  プレイシーン設定
    public void SetPlaySceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

        //  プレイシーンマネージャーが無かったら格納しない
        if (m_playSceneManager == null)
            UnityEngine.Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:ToolUI");
        else
        {
            //  プレイヤー格納
            m_player = m_playSceneManager.Player;
        }
    }
}
