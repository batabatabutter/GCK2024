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
    public class MiningValue
    {
		[Tooltip("採掘範囲(半径)")]
		public float range = 1.0f;
        [Tooltip("採掘サイズ(半径)")]
        public float size = 0.0f;
		[Tooltip("採掘力")]
		public float power = 1.0f;
		[Tooltip("採掘速度(/s)")]
		public float speed = 1.0f;
		[Tooltip("クリティカル率(%)")]
		public float critical = 1.0f;
        [Tooltip("クリティカルダメージ(%)")]
        public float criticalDamage = 200.0f;
		[Tooltip("アイテムドロップ率(%)")]
		public float itemDrop = 100.0f;

        public static MiningValue operator+ (MiningValue left, MiningValue right)   // 和
        {
            MiningValue val = new()
            {
                range = left.range + right.range,
                size = left.size + right.size,
                power = left.power + right.power,
                speed = left.speed + right.speed,
                critical = left.critical + right.critical,
                criticalDamage = left.criticalDamage + right.criticalDamage,
                itemDrop = left.itemDrop + right.itemDrop,
            };
            return val;
        }
        public static MiningValue operator- (MiningValue left, MiningValue right)   // 差
        {
            MiningValue val = new()
            {
                range = left.range - right.range,
                size = left.size - right.size,
                power = left.power - right.power,
                speed = left.speed - right.speed,
                critical = left.critical - right.critical,
                criticalDamage = left.criticalDamage - right.criticalDamage,
                itemDrop = left.itemDrop - right.itemDrop,
            };
            return val;
        }
        public static MiningValue operator* (MiningValue left, MiningValue right)   // 積
        {
			MiningValue val = new()
			{
				range = left.range * right.range,
                size = left.size * right.size,
				power = left.power * right.power,
				speed = left.speed * right.speed,
				critical = left.critical * right.critical,
                criticalDamage = left.criticalDamage * right.criticalDamage,
				itemDrop = left.itemDrop * right.itemDrop
			};
			return val;
        }
        public static MiningValue operator *(MiningValue value, float product)
        {
			MiningValue val = new()
			{
				range = value.range * product,
				size = value.size * product,
				power = value.power * product,
				speed = value.speed * product,
				critical = value.critical * product,
				criticalDamage = value.criticalDamage * product,
				itemDrop = value.itemDrop * product
			};
			return val;
		}
	}

    // 採掘方向のRay
    struct MiningRay
    {
        public Vector2 direction;
        public Vector2 origin;
        public float length;

        public readonly Vector2 MiningPos()
        {
            return origin + (direction * length);
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

    [Header("採掘パーティクル")]
    [SerializeField] private ParticleSystem m_miningParticle = null;

    // 最終的な採掘値
    private MiningValue m_miningValue;

    // 採掘のクールタイム
    private float m_miningCoolTime = 0.0f;

    // 採掘回数
    private int m_miningCount = 0;
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


		// マウスの位置を取得
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// 採掘用のRay取得
		MiningRay miningRay = GetMiningPoint(mousePos);

		// プレイヤーから採掘方向へのRayCast
		RaycastHit2D[] rayCasts = Physics2D.RaycastAll(miningRay.origin, miningRay.direction, miningRay.length, m_layerMask);
        bool hit = false;
		foreach (RaycastHit2D rayCast in rayCasts)
		{
			// タグが Block
			if (rayCast.transform.CompareTag("Block"))
			{
                // 当たった位置を採掘ポイントにする
                m_debugMiningPoint.transform.position = rayCast.point;
                hit = true;
                break;
			}

			// タグが Tool
			if (rayCast.transform.CompareTag("Tool"))
			{
				// マウスカーソルと同じグリッド
				if (MyFunction.CheckSameGrid(rayCast.transform.position, mousePos))
				{
                    m_debugMiningPoint.transform.position = rayCast.transform.position;
                    hit = true;
					break;
				}
			}
		}
        // 当たったものがない
        if (!hit)
        {
            // 採掘ポイント
            m_debugMiningPoint.transform.position = miningRay.origin + (miningRay.direction * miningRay.length);
        }

    }

	// 採掘する
	public void Mining()
    {
        // 採掘クールタイム中
        if (m_miningCoolTime > 0.0f)
            return;

		// マウスの位置を取得
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// 採掘用のRay取得
		MiningRay miningRay = GetMiningPoint(mousePos);

        // プレイヤーから採掘方向へのRayCast
        RaycastHit2D[] rayCasts = Physics2D.RaycastAll(miningRay.origin, miningRay.direction, miningRay.length, m_layerMask);
        // ダメージを与えたブロックの位置
        Transform blockTransform = null;
        foreach (RaycastHit2D rayCast in rayCasts)
        {
            // タグが Block
            if (rayCast.transform.CompareTag("Block"))
            {
                // ブロックにダメージを与える
                if (CauseDamageToBlock(rayCast.transform))
                {
                    // ダメージを与えたブロック
                    blockTransform = rayCast.transform;

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

					// ダメージを与えたブロック
					blockTransform = rayCast.transform;

					break;
				}
			}
		}
        // 範囲採掘
        MiningOfRange(blockTransform, miningRay.MiningPos());

		// クールタイム設定
		m_miningCoolTime = 1.0f / m_miningValue.speed;

	}



    private MiningRay GetMiningPoint(Vector2 mousePos)
    {
		// プレイヤーの位置
		Vector2 playerPos = transform.position;

		// プレイヤーの位置からマウスの位置へのベクトル
		Vector2 playerToMouse = mousePos - playerPos;
		// プレイヤーからマウスまでの距離
		float length = playerToMouse.magnitude;
		if (length > m_miningValue.range)
		{
			length = m_miningValue.range;
		}
		// ベクトル正規化
		playerToMouse.Normalize();

        MiningRay miningRay = new()
        {
            direction = playerToMouse,
            origin = playerPos,
            length = length,
        };

        return miningRay;
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
            if (!block.AddMiningDamage(GetPower(), (int)(m_miningValue.itemDrop / 100.0f)))
                return false;

			// 採掘回数加算
			m_miningCount++;
			// 与ダメージ加算
			m_takenDamage += m_miningValue.power;

			// 採掘エフェクト
			if (m_miningParticle)
            {
                Instantiate(m_miningParticle.gameObject, transform.position, Quaternion.identity);
            }

			// ブロックに当たったらダメージ処理を抜ける
			return true;

		}

        return false;
	}

    // 広範囲の採掘
    private void MiningOfRange(Transform hit, Vector2 center)
    {
        // トランスフォームがある
        if (hit)
        {
            center = hit.position;
        }

        // 採掘サイズが 1 以下
        if (m_miningValue.size <= 0.5f)
            return;

        // ブロックの取得
        Collider2D[] blocks = Physics2D.OverlapCircleAll(center, m_miningValue.size, LayerMask.GetMask("Block"));

        foreach(Collider2D block in blocks)
        {
            // 中心のブロックは除外
            if (hit == block.transform)
                continue;

			// ダメージ
			// タグが Block
			if (block.transform.CompareTag("Block"))
			{
                // ブロックにダメージを与える
                CauseDamageToBlock(block.transform);
				continue;
			}
			// タグが Tool
			if (block.transform.CompareTag("Tool"))
			{
				// マウスカーソルと同じグリッド
				if (MyFunction.CheckSameGrid(block.transform.position, center))
				{
					// ツールにダメージを与える
					CauseDamageToBlock(block.transform);
					break;
				}
			}
		}

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
            power *= m_miningValue.criticalDamage / 100.0f;
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
    // 与えたダメージ
    public float TakenDamage
    {
        get { return m_takenDamage; }
    }

}
