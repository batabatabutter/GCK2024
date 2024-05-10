using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMarker : MonoBehaviour
{
    [Header("�}�[�J�[�̕\������")]
    [SerializeField] private float m_lifeTime = 1.0f;
    private float m_time = 0.0f;

    [Header("�p�[�e�B�N��")]
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
        // ���Ԍo��
        m_time -= Time.deltaTime;
        // �\�����Ԃ��߂����玩�����g��j������
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
