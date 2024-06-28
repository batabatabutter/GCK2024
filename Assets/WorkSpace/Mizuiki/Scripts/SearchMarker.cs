using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SearchMarker : MonoBehaviour
{
	[Header("マーカーの表示時間")]
    [SerializeField] private float m_lifeTime = 1.0f;
    private float m_time = 0.0f;

    [Header("マーカーの表示範囲(直径)")]
    [SerializeField] private float m_maxScale = 100.0f;

    [Header("パーティクル")]
    [SerializeField] private ParticleSystem m_particleSystem = null;

    [Header("プレイヤー")]
    [SerializeField] private Transform m_player = null;

    // マーカーの強制フェード
    private bool m_fade = false;



    // Start is called before the first frame update
    void Start()
    {
        // オブジェクト自体の生存時間
        m_time = m_lifeTime * 2.0f;
        // パーティクルの生存時間設定
        ParticleSystem.MainModule main = m_particleSystem.main;
        main.startLifetimeMultiplier = m_lifeTime;
        // 最大サイズ設定
        ParticleSystem.SizeOverLifetimeModule size = m_particleSystem.sizeOverLifetime;
        size.xMultiplier = m_maxScale;
        // パーティクル再生開始
        m_particleSystem.Play();

    }

    // Update is called once per frame
    void Update()
    {
        // 時間経過
        m_time -= Time.deltaTime;
        // 表示時間を過ぎたら自分自身を破棄する
        if (m_time < 0.0f)
        {
            Destroy(gameObject);
        }

        // 強制フェード発生済み
        if (m_fade)
        {
            return;
        }

        // マーカーとプレイヤーの距離
        float distance = Vector3.Distance(transform.position, m_player.position);

        // 時間
        float t = m_particleSystem.time / m_lifeTime;

		// パーティクルがプレイヤーに近づいたら最大サイズを変える
		if (distance < (m_maxScale / 2.0f) * t)
        {
            // 透明度の設定
            ParticleSystem.ColorOverLifetimeModule color = m_particleSystem.colorOverLifetime;
            Gradient gradient = color.color.gradient;

            Gradient grad = new();
            grad.SetKeys(
             new GradientColorKey[] {
                  gradient.colorKeys[0], gradient.colorKeys[1] },
             new GradientAlphaKey[] {
                  new (gradient.alphaKeys[0].alpha, t), new(gradient.alphaKeys[1].alpha, t + 0.1f) });
            color.color = grad;

            // 強制フェード発生
            m_fade = true;

		}
	}


    public float LifeTime
    {
        set { m_lifeTime = value; }
    }

    public float MaxScale
    {
        set { m_maxScale = value; }
    }

    public Transform Player
    {
        set { m_player = value; }
    }
}
