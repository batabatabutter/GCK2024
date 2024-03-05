using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMining : MonoBehaviour
{
    [Header("採掘範囲(半径)")]
    [SerializeField] private float m_miningRange = 2.0f;

    [Header("採掘力")]
    [SerializeField] private float m_miningPower = 1.0f;

    [Header("採掘速度(/s)")]
    [SerializeField] private float m_miningSpeed = 1.0f;
    private float m_miningCoolTime = 0.0f;

    [Header("レイヤーマスク")]
    [SerializeField] private LayerMask m_layerMask;

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
        RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_miningRange, m_layerMask);
        // 当たったものがあれば当たった位置が採掘ポイント
        if (rayCast)
        {
            m_debugMiningPoint.transform.position = rayCast.point;
        }
        // 当たったものがなければ
        else
        {
            // 採掘ポイント
            m_debugMiningPoint.transform.position = playerPos + (playerToMouse * m_miningRange);
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
        RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_miningRange, m_layerMask);
		// 当たったものがあれば採掘
		if (rayCast)
		{
            // [Block] の取得を試みる
            if (rayCast.transform.TryGetComponent(out Block block))
            {
                // 採掘ダメージ加算
                block.AddMiningDamage(m_miningPower);
            }
            //Debug.Log("MINING");
		}

        // クールタイム設定
        m_miningCoolTime = 1.0f / m_miningSpeed;

	}
}
