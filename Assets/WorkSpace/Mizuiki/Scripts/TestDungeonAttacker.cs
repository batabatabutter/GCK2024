using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeonAttacker : MonoBehaviour
{
    // 攻撃パターンクラス設定用
    [System.Serializable]
    public struct AttackPattern
    {
        public DungeonAttackData.AttackType type;
        public DungeonAttackBase attack;
	}

    [Header("攻撃状態")]
    [SerializeField] private bool m_active = false;
    [Header("攻撃ランク")]
    [SerializeField] private int m_attackRank = 0;

    [Header("攻撃対象")]
    [SerializeField] private Transform m_target = null;

    [Header("攻撃のクールタイム(確認用)")]
    [SerializeField] private float m_attackCoolTime = 0.0f;

    [Header("攻撃の情報")]
    [SerializeField] private DungeonAttackData m_attackData = null;

    [Header("攻撃停止時間")]
    [SerializeField] private float m_stayTime = 10.0f;
    private float m_stayTimer = 0.0f;
    [Header("攻撃発生時間")]
    [SerializeField] private float m_attackTime = 10.0f;
    private float m_attackTimer = 0.0f;

    [Header("ランダム攻撃")]
    [SerializeField] private bool m_random = false;

    [Header("攻撃パターンのインデックス")]
    [SerializeField] private int m_attackPatternIndex = 0;

    [Header("ダンジョンの攻撃パターン")]
    [SerializeField] private AttackPattern[] m_attackPattern;
    private Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = new();

	private DungeonAttackData.AttackType m_type;


	// Start is called before the first frame update
	void Start()
    {
        // 攻撃パターンの初期化
        for (int i = 0; i < m_attackPattern.Length; i++)
        {
            // タイプの取得
			DungeonAttackData.AttackType type = m_attackPattern[i].type;
            // 上書き防止
            if (m_attacker.ContainsKey(type))
            {
                continue;
            }
            // 辞書に追加
            m_attacker[type] = m_attackPattern[i].attack;
		}

        SetAttackData();

        // 停止時間を初期化
        m_stayTimer = m_stayTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_active)
        {
            Attack();
            // 攻撃時間の経過
            m_attackTimer -= Time.deltaTime;
			// 攻撃時間が過ぎた
			if (m_attackTimer <= 0.0f)
			{
				// 攻撃を停止する
				m_active = false;
				m_stayTimer = m_stayTime;
                // ランクアップ
                m_attackRank++;
				Debug.Log("攻撃停止");
			}
		}
		else
        {
            // 停止時間の経過
            m_stayTimer -= Time.deltaTime;
			// 停止時間が過ぎた
			if (m_stayTimer <= 0.0f)
			{
				// 攻撃を開始する
				m_active = true;
				m_attackTimer = m_attackTime;
				Debug.Log("攻撃開始");
			}
		}
	}


    private void Attack()
    {
		// 攻撃パターンがなければ処理しない
		if (m_attacker.Count == 0)
		{
			return;
		}
		// 時間経過
		m_attackCoolTime -= Time.deltaTime;
		// 攻撃する
		if (m_attackCoolTime <= 0.0f)
		{
			m_attacker[m_type].Attack(m_target, m_attackRank);
			m_attackCoolTime = m_attacker[m_type].AttackTime;
		}

	}

    // 次の攻撃タイプ決定
    private void NextType()
    {
        // ランダム攻撃
        if (m_random)
        {
            // 次の攻撃のインデックスをランダムで取得
            m_attackPatternIndex = Random.Range(0, m_attackData.AttackPattern.Length);
            return;
        }
        else
        {
            // インデックスのインクリメント
            m_attackPatternIndex++;
            // 範囲外になった
            if (m_attackPatternIndex >= m_attackData.AttackPattern.Length)
            {
                // 0 に戻す
                m_attackPatternIndex = 0;
            }
        }
        // タイプの設定
		m_type = m_attackData.AttackPattern[m_attackPatternIndex].attackType;
	}

	// 情報設定
	private void SetAttackData()
    {
        if (m_attackData == null)
            return;

        // 停止時間設定
        m_stayTime = m_attackData.StayTime;
        // 攻撃時間設定
        m_attackTime = m_attackData.AttackTime;
        // ランダムフラグ設定
        m_random = m_attackData.IsRandom;

    }

}
