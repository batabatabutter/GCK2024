using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMarker : MonoBehaviour
{
    [Header("マーカーの表示時間")]
    [SerializeField] private float m_lifeTime = 1.0f;
    private float m_time = 0.0f;

    [Header("パーティクル")]
    [SerializeField] private ParticleSystem m_particleSystem = null;


    // Start is called before the first frame update
    void Start()
    {
        m_time = m_lifeTime;
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
    }


    public float LifeTime
    {
        set { m_lifeTime = value; }
    }
}
