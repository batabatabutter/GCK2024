using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("ライフ")]
    [SerializeField] private int m_life = 5;

    [Header("最大ライフ")]
    [SerializeField] private int m_maxLife = 10;

    [Header("アーマーの数")]
    [SerializeField] private int m_armor = 0;

    [Header("無敵時間")]
    [SerializeField] private float m_invincibleTime = 1.0f;
    private float m_invincible = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WorkSpace/Kawase/Tool/Tool_Prefab/Tool_Toach.prefab"), transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_invincible > 0.0f)
        {
			// 無敵時間の経過
			m_invincible -= Time.time;
        }
        
    }

    // ダメージ
    public void AddDamage(int damage)
    {
        // 無敵時間中
        if (m_invincible > 0.0f)
            return;

        // アーマーがある
        if (m_armor > 0)
        {
            // アーマーを1つ削る
            m_armor--;
            if (m_armor <= 0) AudioManager.Instance.PlaySE(AudioDataID.BreakArmor);
            else AudioManager.Instance.PlaySE(AudioDataID.GetDamageArmor);
            return;
        }

        // 体力減少
        m_life -= damage;
        if (m_life <= 0) AudioManager.Instance.PlaySE(AudioDataID.DeadPlayer);
        else AudioManager.Instance.PlaySE(AudioDataID.GetDamage);

        // 無敵時間の設定
        m_invincible = m_invincibleTime;

    }

    // 回復
    public void Healing(int val)
    {
        // 体力加算
        m_life += val;

        // 最大ライフを超えた
        if (m_life > m_maxLife)
        {
            m_life = m_maxLife;
        }
    }


    // ライフ
    public int HitPoint
    {
        get { return m_life; }
    }
    // 最大ライフ
    public int MaxLife
    {
        get { return m_maxLife; }
    }
    // アーマーの枚数
    public int Armor
    {
        get { return m_armor; }
        set { m_armor = value; }
    }

}
