using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("プレイヤーの目")]
    [SerializeField] private SpriteRenderer m_playerEye = null;

    [Header("目のスプライト"), Tooltip("0->開いてる, 1->閉じてる")]
    [SerializeField] private List<Sprite> m_eyeSprites = new();

    [Header("瞬きの時間")]
    [SerializeField] private float m_blinkTime = 0.1f;
    private float m_blinkTimer = 0.0f;

    [Header("平均的な瞬き間隔(秒)")]
    [SerializeField] private float m_blinkInterval = 1.0f;

    [Header("プレイヤーの採掘情報")]
    [SerializeField] private PlayerMining m_playerMining = null;

    [Header("位置の振れ幅")]
    [SerializeField] private Vector2 m_amplitude = Vector2.one;



    // Start is called before the first frame update
    void Start()
    {
        // 採掘の取得
        if (m_playerMining == null)
        {
            m_playerMining = GetComponent<PlayerMining>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 採掘がなければ処理しない
        if (m_playerMining == null)
            return;

        // 採掘方向
        Vector2 miningVec = m_playerMining.GetMiningVector();
        // 採掘半径
        float miningRange = m_playerMining.MiningRange;
        // 移動割合
        float rate = miningVec.magnitude / miningRange;

        // 座標設定
        m_playerEye.transform.position = m_playerMining.transform.position + (Vector3)(m_amplitude * miningVec.normalized * rate);

        // 瞬き
        Blink();

    }



    // 瞬き
    private void Blink()
    {
        // 目を閉じてる
        if (m_blinkTimer > 0.0f)
        {
            // 時間経過
            m_blinkTimer -= Time.deltaTime;
            return;
        }

        float rand = Random.Range(0.0f, 1.0f);

        // 目を開けておこう
        if (rand >= 1.0f / m_blinkInterval * Time.deltaTime)
        {
            m_playerEye.sprite = m_eyeSprites[0];
        }
        // 閉じなきゃ
        else
        {
            m_playerEye.sprite = m_eyeSprites[1];
            m_blinkTimer = m_blinkTime;
        }
    }
}
