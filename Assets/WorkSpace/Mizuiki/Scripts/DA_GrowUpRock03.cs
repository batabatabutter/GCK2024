using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GrowUpRock03 : DA_GrowUpRock
{
    [Header("---------- 迫りくる岩 ----------")]
    [Header("岩を生やすクラス")]
    [SerializeField] private DA_GrowUpRock02 m_growUpRock02;

    [Header("攻撃フラグ")]
    [SerializeField] private bool m_attack = false;

    [Header("迫りくる時間")]
    [SerializeField] private float m_loomingTime = 0.5f;
    private float m_loomingTimer = 0.0f;

    [Header("並ぶ数")]
    [SerializeField] private int m_range = 5;

    [Header("ランク")]
    [SerializeField] private int m_rank = 0;
    [Header("ランクに応じた増加量")]
    [SerializeField] private float m_rankValue = 1.0f;

    [Header("迫りくる方向")]
    [SerializeField] private MyFunction.Direction m_direction;

    [Header("ターゲットの位置")]
    [SerializeField] private Vector3 m_targetPosition = Vector3.zero;
    [Header("ターゲットからの距離")]
    [SerializeField] private float m_distance = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (m_growUpRock02 == null)
        {
			// 岩を生やそう
			m_growUpRock02 = GetComponent<DA_GrowUpRock02>();
		}
	}

    // Update is called once per frame
    void Update()
    {
        // 攻撃しない
        if (!m_attack)
            return;

        // ターゲットに十分近づいた
        if (m_distance < 0.0f)
        {
            // 攻撃フラグオフ
            m_attack = false;
            return;
        }

        // タイマー稼働中
        if (m_loomingTimer > 0.0f)
        {
			// 時間経過
			m_loomingTimer -= Time.deltaTime;

            return;
		}

        // タイマーゼロ
        // 攻撃発生
        m_growUpRock02.AttackLump(m_targetPosition, m_direction, m_range, m_distance, m_rankValue, m_rank);
        // タイマー設定
        m_loomingTimer = m_loomingTime;
        // 距離を近づける
        m_distance--;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
        // ターゲットの位置取得
        m_targetPosition = target.position;
        // 迫りくる方向取得
        m_direction = MyFunction.GetDirection(direction);
        // 並ぶ数取得
        m_range = (int)range;
        // ターゲットからの距離取得
        m_distance = distance;
        // ランク補正値
        m_rankValue = rankValue;
        // ランク
        m_rank = attackRank;

        // 一発目の攻撃
        m_growUpRock02.Attack(target, m_direction, m_range, m_distance, rankValue, attackRank);

        // 攻撃フラグオン
        m_attack = true;
        // タイマー設定
        m_loomingTimer = m_loomingTime;
        // 距離を近づける
        m_distance--;
	}
}
