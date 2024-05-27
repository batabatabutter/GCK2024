using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("�����f�[�^�x�[�X")]
    [SerializeField] private AudioDataBase m_audioDataBase;

    [Header("�I�[�f�B�I�\�[�X")]
    [SerializeField] private AudioSource m_audioSource;

    [Header("���ʉ��Đ��p��")]
    [SerializeField] private GameObject m_seObj;


    [Header("BGM����")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_audioVol = 0.5f;

    [Header("���ʉ�����")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_seVol = 0.5f;

    //  ���ʉ�
    public float AudioVol
    {
        set { m_audioVol = value; }
        get { return m_audioVol; }
    }

    //  ���ʉ�
    public float SEVol
    {
        set { m_seVol = value; }
        get { return m_seVol; }
    }

    //  �N����
    public override void Awake()
    {
        base.Awake();

        //  �����擾
        if (m_audioDataBase == null)
            m_audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// ���ʉ��Đ�
    /// </summary>
    /// <param name="clip">�N���b�v</param>
    public void PlaySE(AudioClip clip)
    {
        m_audioSource.PlayOneShot(clip, m_seVol);
    }

    /// <summary>
    /// ���ʉ��Đ�
    /// </summary>
    /// <param name="ID">ID</param>
    public void PlaySE(AudioDataID id)
    {
        m_audioSource.PlayOneShot(m_audioDataBase.GetAudioData(id).AudioClip, m_seVol);
    }

    /// <summary>
    /// ���ʉ��Đ�
    /// </summary>
    /// <param name="clip">�N���b�v</param>
    /// <param name="pos">���W</param>
    public void PlaySE(AudioClip clip, Vector3 pos)
    {
        var se = Instantiate(m_seObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        se.clip = clip;
        se.volume = m_seVol;
        se.Play();
    }

    /// <summary>
    /// ���ʉ��Đ�
    /// </summary>
    /// <param name="data">�f�[�^</param>
    /// <param name="ID">ID</param>
    public void PlaySE(AudioDataID id, Vector3 pos)
    {
        //  �w�肵���ʒu�ɉ����𐶐�
        var se = Instantiate(m_seObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        se.clip = m_audioDataBase.GetAudioData(id).AudioClip;
        se.volume = m_seVol;
        se.Play();
    }
}
