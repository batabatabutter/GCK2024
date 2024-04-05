using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class PlayerMining : MonoBehaviour
{
    // 採掘に使用する値
    [System.Serializable]
    public struct MiningValue
    {
		[Header("採掘範囲(半径)")]
		public float range;
		[Header("採掘力")]
		public float power;
		[Header("採掘速度(/s)")]
		public float speed;
		[Header("クリティカル率(%)")]
		public float critical;
        [Header("クリティカルダメージ")]
        public float criticalDamage;
		[Header("アイテムドロップ率")]
		public float itemDrop;

        public static MiningValue operator+ (MiningValue left, MiningValue right)
        {
            MiningValue val = new()
            {
                range = left.range + right.range,
                power = left.power + right.power,
                speed = left.speed + right.speed,
                critical = left.critical + right.criticalDamage,
                criticalDamage = left.criticalDamage + right.criticalDamage,
                itemDrop = left.itemDrop + right.itemDrop,
            };
            return val;
        }
        public static MiningValue operator* (MiningValue left, MiningValue right)
        {
			MiningValue val = new()
			{
				range = left.range * right.range,
				power = left.power * right.power,
				speed = left.speed * right.speed,
				critical = left.critical * right.critical,
                criticalDamage = left.criticalDamage * right.criticalDamage,
				itemDrop = left.itemDrop * right.itemDrop
			};
			return val;
        }
    }

    [Header("レイヤーマスク")]
    [SerializeField] private LayerMask m_layerMask;

    [Header("基礎値")]
    [SerializeField] private MiningValue m_miningValueBase;
    [Header("倍率")]
    [SerializeField] private MiningValue m_miningValueRate;
    [Header("強化値")]
    [SerializeField] private MiningValue m_miningValueBoost;

    // 最終的な採掘値
    private MiningValue m_miningValue;

    //[Header("採掘範囲(半径)")]
    //[SerializeField] private float m_miningRange = 2.0f;
    //[Header("採掘範囲倍率")]
    //[SerializeField] private float m_miningRangeRate = 1.0f;

    //[Header("採掘力")]
    //[SerializeField] private float m_miningPower = 1.0f;
    //[Header("採掘力倍率")]
    //[SerializeField] private float m_miningPowerRate = 1.0f;

    //[Header("採掘速度(/s)")]
    //[SerializeField] private float m_miningSpeed = 1.0f;
    private float m_miningCoolTime = 0.0f;
    //[Header("採掘速度倍率")]
    //[SerializeField] private float m_miningSpeedRate = 1.0f;

    //[Header("クリティカル率(%)")]
    //[SerializeField] private float m_criticalRate = 0.0f;
    //[Header("クリティカルダメージ(%)")]
    //[SerializeField] private float m_criticalDamageRate = 2.0f;

    //[Header("アイテムドロップ数")]
    //[SerializeField] private int m_itemDropCount = 1;
    //[Header("アイテムドロップ率")]
    //[SerializeField] private float m_itemDropRate = 1.0f;

    // 採掘回数
    private int m_miningCount = 0;
    // ブロックの破壊数
    private int m_brokenCount = 0;
    // 与えたダメージ
    private float m_takenDamage = 0.0f;


    [Header("デバッグ表示")]
    [SerializeField] private GameObject m_debugMiningRange;
    [SerializeField] private GameObject m_debugMiningPoint;

    // Start is called before the first frame update
    void Start()
    {
        // 採掘値の計算
        m_miningValue = m_miningValueBase * m_miningValueRate * m_miningValueBoost;

        // 採掘範囲の設定
        m_debugMiningRange.transform.localScale = new Vector3(m_miningValue.range * 2.0f, m_miningValue.range * 2.0f, m_miningValue.range * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
		// 採掘値の計算
		m_miningValue = m_miningValueBase * m_miningValueRate * m_miningValueBoost;

		// 採掘クールタイム
		if (m_miningCoolTime > 0.0f)
        {
            m_miningCoolTime -= Time.deltaTime;
        }

        // プレイヤーの位置
        Vector2 playerPos = transform.position;

        // マウスの位置を取得
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // プレイヤーの位置からマウスの位置へのベクトル
        Vector2 playerToMouse = mousePos - playerPos;
        // ベクトル正規化
        playerToMouse.Normalize();

        // プレイヤーから採掘方向へのRayCast
        RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_miningValue.range, m_layerMask);
        // 当たったものがあれば当たった位置が採掘ポイント
        if (rayCast)
        {
            m_debugMiningPoint.transform.position = rayCast.point;
        }
        // 当たったものがなければ
        else
        {
            // 採掘ポイント
            m_debugMiningPoint.transform.position = playerPos + (playerToMouse * m_miningValue.range);
        }



	}

    // 採掘する
    public void Mining()
    {
        // 採掘クールタイム中
        if (m_miningCoolTime > 0.0f)
            return;

		// プレイヤーの位置
		Vector2 playerPos = transform.position;

		// マウスの位置を取得
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// プレイヤーの位置からマウスの位置へのベクトル
		Vector2 playerToMouse = mousePos - playerPos;
		// ベクトル正規化
		playerToMouse.Normalize();

        // プレイヤーから採掘方向へのRayCast
        RaycastHit2D[] rayCasts = Physics2D.RaycastAll(playerPos, playerToMouse, m_miningValue.range, m_layerMask);
        foreach (RaycastHit2D rayCast in rayCasts)
        {
            // タグが Block
            if (rayCast.transform.CompareTag("Block"))
            {
                // ブロックにダメージを与える
                if (CauseDamageToBlock(rayCast.transform))
                {
                    // 与ダメージに加算
                    m_takenDamage += m_miningValue.power;

                    // 一番手前のブロックにダメージを与えた
                    break;
                }

                continue;
            }

            // タグが Tool
            if (rayCast.transform.CompareTag("Tool"))
            {
                // マウスカーソルと同じグリッド
                if (MyFunction.CheckSameGrid(rayCast.transform.position, mousePos))
				{
                    // ツールにダメージを与える
                    CauseDamageToBlock(rayCast.transform);
                    break;
				}

			}
		}

		// クールタイム設定
		m_miningCoolTime = 1.0f / (m_miningValue.speed);

	}



	/// <summary>
	/// ブロックにダメージを与える
	/// </summary>
	/// <param name="transform">ダメージを与えるブロック</param>
	/// <returns>ダメージが通ったか</returns>
	private bool CauseDamageToBlock(Transform transform)
    {
		// [Block] の取得を試みる
		if (transform.TryGetComponent(out Block block))
		{
			// 採掘ダメージ加算
			if (block.AddMiningDamage(GetPower(), (int)(m_miningValue.itemDrop)))
			{
				// 破壊回数加算
				m_brokenCount++;
			}

			// 採掘回数加算
			m_miningCount++;

			// ブロックに当たったらダメージ処理を抜ける
			return true;

		}

        return false;
	}


	// 採掘力算出
	private float GetPower()
    {
        // 採掘力
        float power = m_miningValue.power;

        // 0 ~ 100%
        float rand = Random.Range(0, 100);

        // 出目がクリティカル率より小さい
        if (rand <= m_miningValue.critical)
        {
            // ダメージ倍率をかける
            power *= m_miningValue.criticalDamage;
        }

        // 採掘力を返す
        return power;
    }


    // 基礎値
    public MiningValue MiningValueBase
    {
        get { return m_miningValueBase; }
        set { m_miningValueBase = value; }
    }
    // 倍率
    public MiningValue MiningValueRate
    {
        get { return m_miningValueRate; }
        set { m_miningValueRate = value; }
    }
    // 強化値
    public MiningValue MiningValueBoost
    {
        get { return m_miningValueBoost; }
        set { m_miningValueBoost = value; }
    }

    // 採掘力
    public float MiningPower
    {
        get { return m_miningValue.power; }
    }

    //// 採掘速度
    //public float MiningSpeed
    //{
    //    get { return m_miningValue.speed; }
    //    set { m_miningValue.speed = value; }
    //}
    //// 採掘速度の倍率
    //public float MiningSpeedRate
    //{
    //    get { return m_miningSpeedRate; }
    //    set { m_miningSpeedRate = value; }
    //}

    //// クリティカル率
    //public float CriticalRate
    //{
    //    get { return m_criticalRate; }
    //    set { m_criticalRate = value; }
    //}

    //// アイテムドロップ率
    //public float ItemDropRate
    //{
    //    set { m_itemDropRate = value; }
    //}

    // 採掘回数
    public int MiningCount
    {
        get { return m_miningCount; }
        set { m_miningCount = value; }
    }
    // 破壊数
    public int BrokenCount
    {
        get { return m_brokenCount; }
    }
    // 与えたダメージ
    public float TakenDamage
    {
        get { return m_takenDamage; }
    }

}
