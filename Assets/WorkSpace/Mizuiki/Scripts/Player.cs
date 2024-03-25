using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("ライフ")]
    [SerializeField] private int m_life = 5;

    [Header("最大ライフ")]
    [SerializeField] private int m_maxLife = 10;

    [Header("アーマーの数")]
    [SerializeField] private int m_armor = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ダメージ
    public void AddDamage(int damage)
    {
        // アーマーがある
        if (m_armor > 0)
        {
            // アーマーを1つ削る
            m_armor--;
            return;
        }

        m_life -= damage;
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
    public int ARMOR
    {
        get { return m_armor; }
        set { m_armor = value; }
    }

}
