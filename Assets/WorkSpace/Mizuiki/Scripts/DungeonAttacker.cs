using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonAttacker : MonoBehaviour
{
	// 攻撃パターンクラス設定用
	[System.Serializable]
	public struct AttackPattern
	{
		public DungeonAttackData.AttackType type;
		public DungeonAttackBase attack;
	}

	[Header("---------- 攻撃状態 ----------")]
	[Header("攻撃状態")]
	[SerializeField] private bool m_active = false;
	[Header("使用する攻撃テーブル")]
	[SerializeField] private DungeonAttackData.AttackTableType m_attackTableType;
	[Header("攻撃ランク")]
	[SerializeField] private int m_attackRank = 0;

	[Header("---------- コアとターゲットの距離 ----------")]
	[Header("コアの位置")]
	[SerializeField] private Vector3 m_corePosition = Vector3.zero;
	[Header("攻撃対象")]
	[SerializeField] private Transform m_target = null;
	[Header("開始時のコアとターゲットの距離")]
	[SerializeField] private float m_startCoreDistance = 0.0f;

	[Header("---------- データ設定するものたち ----------")]
	[Header("攻撃の情報")]
	[SerializeField] private DungeonAttackData m_attackData = null;

	// ランダム攻撃
	private bool m_random = false;
	// 攻撃ランクの上限
	private int m_attackRankLimit = 10;

	// 距離に応じた攻撃段階
	private DungeonAttackData.AttackGrade m_attackGrade;

	// 攻撃停止時間
	private float m_stayTime = 10.0f;
	private float m_stayTimer = 0.0f;
	// 攻撃発生時間
	private float m_attackTime = 10.0f;
	private float m_attackTimer = 0.0f;

	// 攻撃テーブルの判定範囲
	private float m_attackTableRange = 5.0f;

	// 使用攻撃テーブルの閾値(割合)
	//private float m_thresholdValueRate = 0.5f;

	// ダンジョンの攻撃順
	private List<DungeonAttackData.AttackTable> m_attackTableList = new();
	private readonly Dictionary<DungeonAttackData.AttackTableType, DungeonAttackTable> m_attackTables = new();

	// ターン内の攻撃処理
	private readonly DungeonAttackTurn m_turn = new();


	[Header("---------- インスペクターで設定必須 ----------")]
	[Header("ダンジョンの攻撃パターン")]
	[SerializeField] private AttackPattern[] m_attackPattern;
	private readonly Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = new();

	[Header("========== デバッグ用 ==========")]
	[Header("[P] で発動")]
	[SerializeField] private DungeonAttackData.AttackType m_attackerType;


	private void Start()
	{
		// 親を持ってくる
		GameObject parent = transform.parent.gameObject;
		if (parent)
		{
			// 親からプレイシーンマネージャを取得
			if (parent.TryGetComponent(out PlaySceneManager scene))
			{
				// プレイヤーを取得
				m_target = scene.GetPlayer().transform;
				// コアの位置を取得
				m_corePosition = scene.GetCore().transform.position;
			}
			// 親からダンジョンジェネレータを取得
			if (parent.TryGetComponent(out DungeonGenerator generator))
			{
				m_attackData = generator.GetDungeonData().AttackData;
			}
		}

		// データの設定
		SetAttackData();
		// 攻撃パターン初期化
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

		// 攻撃テーブル初期化
		for (int i = 0; i < m_attackTableList.Count; i++)
		{
			// タイプの取得
			DungeonAttackData.AttackTableType type = m_attackTableList[i].type;
			// 辞書の上書き防止
			if (m_attackTables.ContainsKey(type))
			{
				continue;
			}
			// 新たなデータを作成
			DungeonAttackTable data = new()
			{
				PatternIndex = 0,				// 攻撃インデックスの初期化
				Table = m_attackTableList[i]	// 攻撃テーブルの設定
			};
			// 辞書に追加
			m_attackTables[type] = data;
		}

		// ターン処理の初期設定
		m_turn.Attacker = m_attacker;

		// 停止時間を初期化
		m_stayTimer = m_stayTime;

		// 開始時の距離を取得
		if (m_target)
		{
			m_startCoreDistance = Vector3.Distance(m_target.position, m_corePosition);
		}

		// distance の昇順にソート
		m_attackGrade.attackGrade.Sort((lhs, rhs) => lhs.distance.CompareTo(rhs.distance));

	}

	private void Update()
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



		// お試し用
		if (Input.GetKeyDown(KeyCode.P))
		{
			m_attacker[m_attackerType].Attack(m_target, MyFunction.Direction.RANDOM, 5.0f, 0, m_attackRank);
		}
	}

	// 攻撃の状態
	public bool Active
	{
		get { return m_active; }
	}
	// コアの位置
	public Vector3 CorePosition
	{
		set { m_corePosition = value; }
	}
	// ターゲットのトランスフォーム
	public Transform Target
	{
		set { m_target = value; }
	}
	// 攻撃の情報
	public DungeonAttackData AttackData
	{
		set { m_attackData = value; }
	}



	// 情報設定
	private void SetAttackData()
	{
		// データがない
		if (m_attackData == null)
		{
			Debug.Log("攻撃データがないよ");
			return;
		}

		// 停止時間設定
		m_stayTime = m_attackData.StayTime;
		// 攻撃時間設定
		m_attackTime = m_attackData.AttackTime;
		// ランク上限
		m_attackRankLimit = m_attackData.RankLimit;
		// ランダムフラグ設定
		m_random = m_attackData.IsRandom;
		// ダンジョンの攻撃順
		m_attackTableList = new(m_attackData.AttackTableList);
		// テーブル決定の判断をする範囲
		m_attackTableRange = m_attackData.AttackTableRange;
		// 使用テーブルを分つ閾値
		//m_thresholdValueRate = m_attackData.ThresholdValueRate;
		// 攻撃段階の設定
		m_attackGrade = m_attackData.AttackGradeData;
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

	}

	// コアとターゲットの距離に応じた攻撃間隔の取得
	private float GetAttackGrade()
	{
		// コアとターゲットの距離
		float distance = Vector3.Distance(m_target.position, m_corePosition);

		// コアとの距離に応じた攻撃段階(0 ~ 1)
		float attackGradeNormal = Mathf.InverseLerp(m_attackGrade.attackMaxDistance, m_startCoreDistance, distance);

		// 距離に応じた段階バージョン
		float attackGradeRank = 1.0f;
		foreach (DungeonAttackData.AttackPower grade in m_attackGrade.attackGrade)
		{
			// 特定段階よりも距離が近い
			if (distance <= m_startCoreDistance * grade.distance)
			{
				attackGradeRank = 1.0f / grade.magnification;
				break;
			}
		}

		// 最終的な攻撃時間の割合を返す
		if (m_attackGrade.attackGradeStep)
		{
			return Mathf.Lerp(m_attackGrade.attackGradeRange.min, m_attackGrade.attackGradeRange.max, attackGradeRank);
		}
		else
		{
			return Mathf.Lerp(m_attackGrade.attackGradeRange.min, m_attackGrade.attackGradeRange.max, attackGradeNormal);
		}
	}

	// 攻撃開始
	private void BeginAttack()
	{
		// 攻撃を開始する
		m_active = true;
		// 攻撃ターンの時間設定
		m_attackTimer = m_attackTime;
		// 攻撃テーブルの決定
		DetermineAttackTable();
		// 攻撃テーブル取得
		DungeonAttackTable data = m_attackTables[m_attackTableType];
		// 攻撃パターンを設定する
		m_turn.AttackPattern = data.Table.pattern[data.PatternIndex];

		Debug.Log("攻撃開始 : " + m_attackTableType);
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
		// 次の攻撃タイプ決定
		NextType();

		Debug.Log("攻撃停止");
	}

	// 次の攻撃タイプ決定
	private void NextType()
	{
		// ランダム攻撃
		if (m_random)
		{
			// 次の攻撃のインデックスをランダムで取得
			m_attackTables[m_attackTableType].PatternIndex = Random.Range(0, m_attackTables[m_attackTableType].Table.pattern.Count);
			return;
		}
		else
		{
			// インデックスのインクリメント
			m_attackTables[m_attackTableType].PatternIndex++;
			// 範囲外になった
			if (m_attackTables[m_attackTableType].PatternIndex >= m_attackTables[m_attackTableType].Table.pattern.Count)
			{
				// 0 に戻す
				m_attackTables[m_attackTableType].PatternIndex = 0;
			}
		}
	}

	// 攻撃テーブルを決定する
	private void DetermineAttackTable()
	{
		// ターゲット周辺のブロックを取得
		Collider2D[] blocks = Physics2D.OverlapCircleAll(m_target.position, m_attackTableRange, LayerMask.NameToLayer("Block"));

		// 判定範囲のブロックの割合を取得
		float blockRate = blocks.Length / (int)Mathf.Round(Mathf.PI * m_attackTableRange * m_attackTableRange);

		foreach (DungeonAttackData.AttackTable attackTable in m_attackTableList)
		{
			// ブロックが決められた割合以上
			if (attackTable.overNum < blockRate)
			{
				m_attackTableType = attackTable.type;
				return;
			}
		}

		m_attackTableType = DungeonAttackData.AttackTableType.CAVITY;

		//// 判定範囲から閾値となるブロックの個数を計算する
		//int thresholdValue = (int)(Mathf.Round(Mathf.PI * m_attackTableRange * m_attackTableRange) * m_thresholdValueRate);

		//// 攻撃テーブルを設定する
		//if (blocks.Length > thresholdValue)
		//{
		//	// 周りがブロックで埋まってるときの攻撃テーブルを使用
		//	m_attackTableType = DungeonAttackData.AttackTableType.FILL;
		//}
		//else
		//{
		//	// 周りにブロックがない時の攻撃テーブルを使用
		//	m_attackTableType = DungeonAttackData.AttackTableType.CAVITY;
		//}
	}


}