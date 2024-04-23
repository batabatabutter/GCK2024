using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAreaHealing : BlockTotem
{
    /*
・設置型
・明るさレベル10
・半径3マスの間にいると体力がだんだん回復する 2秒で1回復
・耐久値500
・1秒当たり50ダメージ自身で食らう
     */

    [Header("回復量")]
    [SerializeField] private int m_healingValue = 2;
    [Header("回復間隔")]
    [SerializeField] private float m_healingInterval = 2.0f;
    // 回復のタイマー
    private float m_healingTimer = 0.0f;


    // Update is called once per frame
    void Update()
    {
        // 時間経過
        if (m_healingTimer > 0.0f)
        {
			m_healingTimer -= Time.deltaTime;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
        // プレイヤーじゃない
        if (!collision.CompareTag("Player"))
            return;

        // インターバル中
        if (m_healingTimer > 0.0f)
            return;

        // プレイヤーの取得
        if (collision.TryGetComponent(out Player player))
        {
            // プレイヤーの体力回復
            player.Healing(m_healingValue);
            // インターバル
            m_healingTimer = m_healingInterval;
            Debug.Log("回復");
        }

	}

}
