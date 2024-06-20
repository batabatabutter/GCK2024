using UnityEngine;
using UnityEngine.UI;

public class PlayerMining : MonoBehaviour
{
    //// 採掘方向のRay
    //struct MiningRay
    //{
    //    public Vector2 direction;
    //    public Vector2 origin;
    //    public float length;

    //    public readonly Vector2 MiningPos()
    //    {
    //        return origin + (direction * length);
    //    }
    //}

    [Header("丸のこ")]
    [SerializeField] private CircularSaw m_circularSaw = null;

    [Header("レイヤーマスク")]
    [SerializeField] private LayerMask m_layerMask;

    [Header("倍率")]
    [SerializeField] private MiningData.MiningValue m_miningValueRate;
    [Header("強化値")]
    [SerializeField] private MiningData.MiningValue m_miningValueBoost;

    [Header("採掘パーティクル")]
    [SerializeField] private ParticleSystem m_miningParticle = null;

    // 最終的な採掘値
    private MiningData.MiningValue m_miningValue;

    // 採掘のクールタイム
    private float m_miningCoolTime = 0.0f;

    // 採掘対象ブロック
    private Transform m_miningBlock = null;

    // 採掘回数
    private int m_miningCount = 0;
    // 与えたダメージ
    private float m_takenDamage = 0.0f;


    [Header("デバッグ表示")]
    [SerializeField] private bool m_debug = true;
    [SerializeField] private GameObject m_debugMiningRange;
    [SerializeField] private GameObject m_debugMiningPoint;
    [SerializeField] private Text       m_debugText = null;

    // Start is called before the first frame update
    void Start()
    {
        // 採掘値の計算
        m_miningValue = m_circularSaw.GetMiningValue() * m_miningValueRate * m_miningValueBoost;

        if (m_debug)
        {
            m_debugMiningRange.SetActive(true);
            m_debugMiningPoint.SetActive(true);
            // 採掘範囲の設定
            m_debugMiningRange.transform.localScale = new Vector3(m_miningValue.range * 2.0f, m_miningValue.range * 2.0f, m_miningValue.range * 2.0f);
        }
        else
        {
            m_debugMiningRange.SetActive(false);
            m_debugMiningPoint.SetActive(false);
        }

        // 丸のこのサイズ設定
        m_circularSaw.SetRange(m_miningValue.range, m_miningValue.size);
    }

    // Update is called once per frame
    void Update()
    {
        // 採掘値の計算
        m_miningValue = m_circularSaw.GetMiningValue() * m_miningValueRate * m_miningValueBoost;

        // 丸のこのサイズ設定
        m_circularSaw.SetRange(m_miningValue.range, m_miningValue.size);

		// 採掘クールタイム
		if (m_miningCoolTime > 0.0f)
        {
            m_miningCoolTime -= Time.deltaTime;
        }

		// 採掘対象ブロックのリセット
		m_miningBlock = null;

        // 採掘位置
        Vector2 miningPoint = Vector2.zero;

        // 採掘用のRay取得
        CircularSaw.MiningRay miningRay = m_circularSaw.GetMiningRay(transform);

        // プレイヤーから採掘方向へのRayCast
        RaycastHit2D[] rayCasts = Physics2D.RaycastAll(miningRay.origin, miningRay.direction, miningRay.length, m_layerMask);
        // ブロックに当たったフラグ
        bool hitBlock = false;
        foreach (RaycastHit2D rayCast in rayCasts)
        {
            // タグが Block
            if (rayCast.transform.CompareTag("Block"))
            {
                // 当たった位置を採掘ポイントにする
                miningPoint = rayCast.point;
                // 採掘ブロックの設定
                m_miningBlock = rayCast.transform;
                // 当たった
                hitBlock = true;
                break;
            }

            // タグが Tool
            if (rayCast.transform.CompareTag("Tool"))
            {
                // マウスカーソルと同じグリッド
                if (MyFunction.CheckSameGrid(rayCast.transform.position, m_circularSaw.transform.position))
                {
                    miningPoint = rayCast.transform.position;
                    break;
                }
            }
        }

        // ブロックに当たった
        if (hitBlock)
        {
            // 採掘ポイント設定
            m_circularSaw.SetPosition(miningPoint);
            Debug.Log("HIT");
		}

        if (m_debug)
        {
            m_debugText.text = "";
            foreach (RaycastHit2D hit2D in rayCasts)
            {
                m_debugText.text += hit2D + "\n";
            }
        }

    }

    // 採掘する
    public void Mining()
    {
        // 回転させる
        m_circularSaw.Rotate(m_miningValue.speed);

        // 採掘クールタイム中
        if (m_miningCoolTime > 0.0f)
            return;

        // 範囲採掘
        MiningOfRange(m_circularSaw.transform.position);

        // クールタイム設定
        m_miningCoolTime = 1.0f / m_miningValue.speed;

    }

    // 採掘ベクトルの取得
    public Vector2 GetMiningVector()
    {
        return m_circularSaw.transform.position - transform.position;
    }


    // 倍率
    public MiningData.MiningValue MiningValueRate
    {
        get { return m_miningValueRate; }
        set { m_miningValueRate = value; }
    }
    // 強化値
    public MiningData.MiningValue MiningValueBoost
    {
        get { return m_miningValueBoost; }
        set { m_miningValueBoost = value; }
    }

    // 採掘範囲
    public float MiningRange
    {
        get { return m_miningValue.range; }
    }
    // 採掘力
    public float MiningPower
    {
        get { return m_miningValue.power; }
    }

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
    private void MiningOfRange(Vector2 center)
    {
        // ブロックの取得
        Collider2D[] blocks = Physics2D.OverlapCircleAll(center, m_miningValue.size / 2.0f, LayerMask.GetMask("Block"));

        // 
        if (blocks.Length == 0 &&
            m_miningBlock)
        {
            CauseDamageToBlock(m_miningBlock);
            return;
        }

        foreach (Collider2D block in blocks)
        {
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

}
