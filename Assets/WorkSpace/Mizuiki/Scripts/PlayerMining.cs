using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class PlayerMining : MonoBehaviour
{
    [Header("レイヤーマスク")]
    [SerializeField] private LayerMask m_layerMask;

    [Header("採掘範囲(半径)")]
    [SerializeField] private float m_miningRange = 2.0f;
    [Header("採掘範囲倍率")]
    [SerializeField] private float m_miningRangeRate = 1.0f;

    [Header("採掘力")]
    [SerializeField] private float m_miningPower = 1.0f;
    [Header("採掘力倍率")]
    [SerializeField] private float m_miningPowerRate = 1.0f;

    [Header("採掘速度(/s)")]
    [SerializeField] private float m_miningSpeed = 1.0f;
    private float m_miningCoolTime = 0.0f;
    [Header("採掘速度倍率")]
    [SerializeField] private float m_miningSpeedRate = 1.0f;

    [Header("クリティカル率(%)")]
    [SerializeField] private float m_criticalRate = 0.0f;
    [Header("クリティカルダメージ(%)")]
    [SerializeField] private float m_criticalDamageRate = 2.0f;

    // 採掘回数
    private int m_miningCount = 0;
    // ブロックの破壊数
    private int m_brokenCount = 0;


    [Header("デバッグ表示")]
    [SerializeField] private GameObject m_debugMiningRange;
    [SerializeField] private GameObject m_debugMiningPoint;

    // Start is called before the first frame update
    void Start()
    {
        // 採掘範囲の設定
        m_debugMiningRange.transform.localScale = new Vector3(m_miningRange * 2.0f, m_miningRange * 2.0f, m_miningRange * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
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
        RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_miningRange * m_miningRangeRate, m_layerMask);
        // 当たったものがあれば当たった位置が採掘ポイント
        if (rayCast)
        {
            m_debugMiningPoint.transform.position = rayCast.point;
        }
        // 当たったものがなければ
        else
        {
            // 採掘ポイント
            m_debugMiningPoint.transform.position = playerPos + (playerToMouse * m_miningRange * m_miningRangeRate);
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
        RaycastHit2D[] rayCasts = Physics2D.RaycastAll(playerPos, playerToMouse, m_miningRange * m_miningRangeRate, m_layerMask);
        foreach (RaycastHit2D rayCast in rayCasts)
        {
            // タグが Block
            if (rayCast.transform.CompareTag("Block"))
            {
                // ブロックにダメージを与える
                if (CauseDamageToBlock(rayCast.transform))
                {
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
                    CauseDamageToBlock(rayCast.transform);
                    break;
				}

			}
		}

		// クールタイム設定
		m_miningCoolTime = 1.0f / (m_miningSpeed * m_miningSpeedRate);

	}



    // ブロックにダメージを与える
    private bool CauseDamageToBlock(Transform transform)
    {
		// [Block] の取得を試みる
		if (transform.TryGetComponent(out Block block))
		{
			// 採掘ダメージ加算
			if (block.AddMiningDamage(GetPower()))
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
        float power = m_miningPower * m_miningPowerRate;

        // 0 ~ 100%
        float rand = Random.Range(0, 100);

        // 出目がクリティカル率より小さい
        if (rand <= m_criticalRate)
        {
            // ダメージ倍率をかける
            power *= m_criticalDamageRate;
        }

        // 採掘力を返す
        return power;
    }


    // 採掘速度
    public float MiningSpeed
    {
        get { return m_miningSpeed; }
        set { m_miningSpeed = value; }
    }
    // 採掘速度の倍率
    public float MiningSpeedRate
    {
        get { return m_miningSpeedRate; }
        set { m_miningSpeedRate = value; }
    }

    // 採掘力
    public float MiningPower
    {
        get { return m_miningPower; }
        set { m_miningPower = value; }
    }

    // クリティカル率
    public float CriticalRate
    {
        get { return m_criticalRate; }
        set { m_criticalRate = value; }
    }

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

}
