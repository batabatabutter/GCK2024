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
    // 距離に応じた攻撃段階
    [System.Serializable, Tooltip("distance 昇順にソート")]
    public struct AttackGrade
    {
        [Tooltip("開始地点の distance 倍のとき")]
        public float distance;
        [Tooltip("攻撃の発生倍率")]
        public float grade;
    }

    [Header("攻撃状態")]
    [SerializeField] private bool m_active = false;
    [Header("攻撃ランク")]
    [SerializeField] private int m_attackRank = 0;
    [Header("攻撃ランクの上限")]
    [SerializeField] private int m_attackRankLimit = 10;

    [Header("コアの位置")]
    [SerializeField] private Vector3 m_corePosition = Vector3.zero;
    [Header("攻撃対象")]
    [SerializeField] private Transform m_target = null;

    [Header("---------- コアとターゲットの距離 ----------")]
    [Header("攻撃段階の範囲")]
    [SerializeField] private MyFunction.MinMaxFloat m_attackGradeRange;
    [Header("攻撃が最大になる距離")]
    [SerializeField] private float m_attackMaxDistance = 0.0f;
    // 開始時のターゲットとコアの距離
    private float m_coreDistance = 0.0f;
    [Header("コアとの距離の取り方の種類")]
    [SerializeField] private bool m_attackGradeStep = false;
    [Header("距離に応じた攻撃段階")]
    [SerializeField] private List<AttackGrade> m_attackGrade;

    [Header("現在の攻撃のグレード(確認用)")]
    [SerializeField] private float m_nowAttackGrade = 1.0f;

    [Header("---------- 攻撃自体の情報 ----------")]
    [Header("攻撃のクールタイム(確認用)")]
    [SerializeField] private float m_attackCoolTime = 0.0f;

    [Header("攻撃の情報")]
    [SerializeField] private DungeonAttackData m_attackData = null;
    [SerializeField] private bool m_useData = true;

    [Header("攻撃停止時間")]
    [SerializeField] private float m_stayTime = 10.0f;
    private float m_stayTimer = 0.0f;
    [Header("攻撃発生時間")]
    [SerializeField] private float m_attackTime = 10.0f;
    private float m_attackTimer = 0.0f;

    [Header("ダンジョンの攻撃パターン")]
    [SerializeField] private AttackPattern[] m_attackPattern;
    private readonly Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = new();

    // ターン内の攻撃処理
    private DungeonAttackTurn m_turn = new();

    [Header("---------- 攻撃状態 ----------")]
    [Header("ランダム攻撃")]
    [SerializeField] private bool m_random = false;

    [Header("使用する攻撃テーブル")]
    [SerializeField] private DungeonAttackData.AttackTableType m_attackTablePattern;
    [Header("攻撃パターンのインデックス")]
    [SerializeField] private readonly Dictionary<DungeonAttackData.AttackTableType, int> m_attackPatternIndex = new();

    [Header("---------- 攻撃テーブル ----------")]
    [Header("攻撃テーブルの判定範囲")]
    [SerializeField] private float m_attackTableRange = 5.0f;

    [Header("使用攻撃テーブルの閾値(割合)"), Range(0.0f, 1.0f), Tooltip("この数値より大きい場合は[FillTable]、小さい場合は[CavityTable]")]
    [SerializeField] private float m_thresholdValueRate = 0.5f;

    [Header("ダンジョンの攻撃順")]
	[SerializeField] private List<DungeonAttackData.AttackTable> m_attackTableList = new();

	// 使用する攻撃テーブル
	private DungeonAttackData.AttackTable m_attackTable = new();

    // 攻撃の種類
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

        // データの設定
        SetAttackData();

        // ターン処理の初期設定
        m_turn.Attacker = m_attacker;

        // クールタイムの設定
        //SetCoolTime();

        // 停止時間を初期化
        m_stayTimer = m_stayTime;

        // 仮でFillを使用攻撃テーブルとする
        m_attackTable = m_attackTableList[0];
        // 攻撃タイプの初期化
        m_type = m_attackTable.pattern[0].AttackList[0].type;

        // 攻撃パターンのインデックス初期化
        for (DungeonAttackData.AttackTableType i = 0; i < DungeonAttackData.AttackTableType.OVER; i++)
        {
            m_attackPatternIndex.Add(i, 0);
        }

		// 開始時の距離を取得
		if (m_target)
        {
            m_coreDistance = Vector3.Distance(m_target.position, m_corePosition);
        }

        // distance の昇順にソート
        m_attackGrade.Sort((lhs, rhs) => lhs.distance.CompareTo(rhs.distance));

    }

    // Update is called once per frame
    void Update()
    {
        // 攻撃活動中
        if (m_active)
        {
            // 攻撃処理
            Attack();
            // 攻撃ターンの時間経過
            m_attackTimer -= Time.deltaTime;
			// 攻撃ターン終了
			if (m_attackTimer <= 0.0f)
			{
                // 攻撃終了
                EndAttack();
			}
		}
		else
        {
            // 停止時間の経過
            m_stayTimer -= Time.deltaTime;
			// 停止時間が過ぎた
			if (m_stayTimer <= 0.0f)
			{
                // 攻撃開始
                BeginAttack();
			}
		}
    }


    // コアの位置
    public Vector3 CorePosition
    {
        set { m_corePosition = value; }
    }




    // 攻撃
    private void Attack()
    {
		// 攻撃パターンがなければ処理しない
		if (m_attacker.Count == 0)
		{
			return;
		}
        m_turn.Attack(m_target, m_attackRank, GetAttackGrade());
		//// 時間経過
		//m_attackCoolTime -= Time.deltaTime;
		//// 攻撃する
		//if (m_attackCoolTime <= 0.0f)
		//{
  //          // 攻撃パターンの情報取得
  //          DungeonAttackData.AttackList pattern = m_attackTable[m_attackPatternIndex[m_attackTablePattern]].attackList;
		//	// 指定タイプの攻撃発生
		//	m_attacker[m_type].Attack(m_target, pattern., m_attackRank);
  //          // クールタイム計算
		//	m_attackCoolTime = m_attacker[m_type].AttackTime * GetAttackGrade();
		//}

	}

    // 攻撃開始
    private void BeginAttack()
    {
		// 攻撃を開始する
		m_active = true;
		// 攻撃ターンの時間設定
		m_attackTimer = m_attackTime;
		// 次の攻撃タイプ決定
		NextType();
		// 攻撃タイプが設定されていなければ処理しない
		if (!m_attacker.ContainsKey(m_type))
		{
			Debug.Log(m_type.ToString() + " : 攻撃パターンが設定されてないよ");
			m_active = false;
			return;
		}
		Debug.Log("攻撃開始 : " + m_type.ToString());
	}

	// 攻撃終了
	private void EndAttack()
    {
		// 攻撃を停止する
		m_active = false;
		// 停止時間の設定
		m_stayTimer = m_stayTime;
		// ランクアップ
		m_attackRank++;
		// 攻撃ランクが上限を超えないようにクランプ
		m_attackRank = Mathf.Clamp(m_attackRank, 0, m_attackRankLimit);
		Debug.Log("攻撃停止");
	}

	// コアとターゲットの距離に応じた攻撃間隔の取得
	private float GetAttackGrade()
    {
		// コアとターゲットの距離
		float distance = Vector3.Distance(m_target.position, m_corePosition);

		// コアとの距離に応じた攻撃段階(0 ~ 1)
		float attackGradeNormal = Mathf.InverseLerp(m_attackMaxDistance, m_coreDistance, distance);

        // 距離に応じた段階バージョン
        float attackGradeRank = 1.0f;
        foreach (AttackGrade grade in m_attackGrade)
        {
            // 特定段階よりも距離が近い
            if (distance <= m_coreDistance * grade.distance)
            {
                attackGradeRank = grade.grade;
                break;
            }
        }

        // 最終的な攻撃時間の割合を返す
        if (m_attackGradeStep)
        {
            m_nowAttackGrade = attackGradeRank;
            return Mathf.Lerp(m_attackGradeRange.min, m_attackGradeRange.max, attackGradeRank);
        }
        else
        {
            m_nowAttackGrade = attackGradeNormal;
            return Mathf.Lerp(m_attackGradeRange.min, m_attackGradeRange.max, attackGradeNormal);
        }
	}

	// 次の攻撃タイプ決定
	private void NextType()
    {
        // 攻撃テーブルの決定
        DetermineAttackTable();

        // ランダム攻撃
        if (m_random)
        {
            // 次の攻撃のインデックスをランダムで取得
            m_attackPatternIndex[m_attackTablePattern] = Random.Range(0, m_attackTable.pattern.Count);
            return;
        }
        else
        {
            // インデックスのインクリメント
            m_attackPatternIndex[m_attackTablePattern]++;
            // 範囲外になった
            if (m_attackPatternIndex[m_attackTablePattern] >= m_attackTable.pattern.Count)
            {
                // 0 に戻す
                m_attackPatternIndex[m_attackTablePattern] = 0;
            }
        }

        // タイプの設定
		//m_type = m_attackTable[m_attackPatternIndex[m_attackTablePattern]].attackList[0].type;
	}

    // 攻撃テーブルを決定する
    private void DetermineAttackTable()
    {
        // ターゲット周辺のブロックを取得
        Collider2D[] blocks = Physics2D.OverlapCircleAll(m_target.position, m_attackTableRange, LayerMask.NameToLayer("Block"));

        // 判定範囲から閾値となるブロックの個数を計算する
        int thresholdValue = (int)(Mathf.Round(Mathf.PI * m_attackTableRange * m_attackTableRange) * m_thresholdValueRate);

        // 攻撃テーブルを設定する
        if (blocks.Length > thresholdValue)
        {
            // 周りがブロックで埋まってるときの攻撃テーブルを使用
            m_attackTable = m_attackTableList[0];
        }
        else
        {
            // 周りにブロックがない時の攻撃テーブルを使用
            m_attackTable = m_attackTableList[1];
        }
    }

	// 情報設定
	private void SetAttackData()
    {
        // データがない
        if (m_attackData == null)
            return;

        // データを使わない
        if (!m_useData)
            return;

        // 停止時間設定
        m_stayTime = m_attackData.StayTime;
        // 攻撃時間設定
        m_attackTime = m_attackData.AttackTime;
        // ランダムフラグ設定
        m_random = m_attackData.IsRandom;
        // ダンジョンの攻撃順
        m_attackTableList = new (m_attackData.AttackTableList);
    }

    //private void SetCoolTime()
    //{
    //    foreach (DungeonAttackData.AttackPattern data in m_attackTable)
    //    {
    //        // 攻撃タイプ
    //        DungeonAttackData.AttackType type = data.type;

    //        // キーが設定してある
    //        if (m_attacker.ContainsKey(type))
    //        {
    //            // 攻撃時間の設定
    //            m_attacker[type].AttackTime = data.[0].time;
    //            // 攻撃範囲の設定
    //            m_attacker[type].SetAttackRange(data.attackList[0].range);
    //            // ランク増加値の設定
    //            m_attacker[type].SetRankValue(data.rankValue);
    //        }
    //    }
    //}

}
