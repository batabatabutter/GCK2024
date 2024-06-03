using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SearchMarker : MonoBehaviour
{
	[Header("�}�[�J�[�̕\������")]
    [SerializeField] private float m_lifeTime = 1.0f;
    private float m_time = 0.0f;

    [Header("�}�[�J�[�̕\���͈�(���a)")]
    [SerializeField] private float m_maxScale = 100.0f;

    [Header("�p�[�e�B�N��")]
    [SerializeField] private ParticleSystem m_particleSystem = null;

    [Header("�v���C���[")]
    [SerializeField] private Transform m_player = null;

    // �}�[�J�[�̋����t�F�[�h
    private bool m_fade = false;



    // Start is called before the first frame update
    void Start()
    {
        // �I�u�W�F�N�g���̂̐�������
        m_time = m_lifeTime * 2.0f;
        // �p�[�e�B�N���̐������Ԑݒ�
        ParticleSystem.MainModule main = m_particleSystem.main;
        main.startLifetimeMultiplier = m_lifeTime;
        // �ő�T�C�Y�ݒ�
        ParticleSystem.SizeOverLifetimeModule size = m_particleSystem.sizeOverLifetime;
        size.xMultiplier = m_maxScale;
        // �p�[�e�B�N���Đ��J�n
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

        // �����t�F�[�h�����ς�
        if (m_fade)
        {
            return;
        }

        // �}�[�J�[�ƃv���C���[�̋���
        float distance = Vector3.Distance(transform.position, m_player.position);

        // ����
        float t = m_particleSystem.time / m_lifeTime;

		// �p�[�e�B�N�����v���C���[�ɋ߂Â�����ő�T�C�Y��ς���
		if (distance < (m_maxScale / 2.0f) * t)
        {
            // �����x�̐ݒ�
            ParticleSystem.ColorOverLifetimeModule color = m_particleSystem.colorOverLifetime;
            Gradient gradient = color.color.gradient;

            Gradient grad = new();
            grad.SetKeys(
             new GradientColorKey[] {
                  gradient.colorKeys[0], gradient.colorKeys[1] },
             new GradientAlphaKey[] {
                  new (gradient.alphaKeys[0].alpha, t), new(gradient.alphaKeys[1].alpha, t + 0.1f) });
            color.color = grad;

            // �����t�F�[�h����
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
