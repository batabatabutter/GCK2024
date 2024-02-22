using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("ブロックの耐久")]
    [SerializeField] private float m_blockEndurance;

    [Header("破壊不可")]
    [SerializeField] private bool m_dontBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 採掘ダメージ
    public void AddMiningDamage(float power)
    {
        // 破壊不可能ブロックの場合は処理しない
        if (m_dontBroken)
            return;

        // 採掘ダメージ加算
        m_blockEndurance -= power;

        // 耐久が0になった
        if (m_blockEndurance <= 0.0f)
        {
            BrokenBlock();
        }
    }

    // ブロックが壊れた
    private void BrokenBlock()
    {
        // 破壊不可能ブロックの場合は処理しない
        if (m_dontBroken)
            return;

        // アイテムドロップ


        // 自身を削除
        Destroy(gameObject);

    }

    
    public bool DontBroken
    {
        get { return m_dontBroken; }
        set { m_dontBroken = value; }
    }

}
