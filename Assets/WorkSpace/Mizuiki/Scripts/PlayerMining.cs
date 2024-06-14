using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class PlayerMining : MonoBehaviour
{
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

    // 採掘回数
    private int m_miningCount = 0;
    // 与えたダメージ
    private float m_takenDamage = 0.0f;


    [Header("デバッグ表示")]
    [SerializeField] private bool m_debug = true;
    [SerializeField] private GameObject m_debugMiningRange;
    [SerializeField] private GameObject m_debugMiningPoint;

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
        SetCircularSawScale();
    }

    // Update is called once per frame
    void Update()
    {
        // 採掘値の計算
        m_miningValue = m_circularSaw.GetMiningValue() * m_miningValueRate * m_miningValueBoost;

        // 採掘クールタイム
        if (m_miningCoolTime > 0.0f)
        {
            m_miningCoolTime -= Time.deltaTime;
        }

        // マウスの位置を取得
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 採掘位置
        Vector2 miningPoint = Vector2.zero;

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
                miningPoint = rayCast.point;
                hit = true;
                break;
            }

            // タグが Tool
            if (rayCast.transform.CompareTag("Tool"))
            {
                // マウスカーソルと同じグリッド
                if (MyFunction.CheckSameGrid(rayCast.transform.position, mousePos))
                {
                    miningPoint = rayCast.transform.position;
                    hit = true;
                    break;
                }
            }
        }
        // 当たったものがない
        if (!hit)
        {
            // 採掘ポイント
            miningPoint = miningRay.origin + (miningRay.direction * miningRay.length);
        }

        // 丸のこのサイズ設定
        SetCircularSawScale();

        // 採掘ポイント設定
        m_circularSaw.SetPosition(miningPoint);

        if (m_debug)
        {
            m_debugMiningPoint.transform.position = miningPoint;
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
    private void MiningOfRange(Vector2 center)
    {
        // ブロックの取得
        Collider2D[] blocks = Physics2D.OverlapCircleAll(center, m_miningValue.size / 2.0f, LayerMask.GetMask("Block"));

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

    // 丸のこのサイズ設定
    private void SetCircularSawScale()
    {
        // スケール
        float scale = Mathf.Max(m_miningValue.size, 1.0f);

        // 丸のこのサイズ設定
        m_circularSaw.transform.localScale = Vector3.one * scale;

    }

}
