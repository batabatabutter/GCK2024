using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("�����f�[�^�x�[�X")]
    [SerializeField] private AudioDataBase m_audioDataBase;

    [Header("���ʉ��Đ�")]
    [SerializeField] private AudioSource m_seSource;

    [Header("���ʉ��Đ��p��")]
    [SerializeField] private GameObject m_seObj;

    [Header("���ʉ�����")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_seVol = 0.5f;

    //  ���ʉ�
    public float SEVol
    {
        set { m_seVol = value; }
        get { return m_seVol; }
    }

    /// <summary>
    /// ���ʉ��Đ�
    /// </summary>
    /// <param name="clip">�N���b�v</param>
    public void PlaySE(AudioClip clip)
    {
        m_seSource.PlayOneShot(clip, m_seVol);
    }

    /// <summary>
    /// ���ʉ��Đ�
    /// </summary>
    /// <param name="ID">ID</param>
    public void PlaySE(AudioClipID id)
    {
        m_seSource.PlayOneShot(m_audioDataBase.GetAudioData(id).AudioClip, m_seVol);
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
    public void PlaySE(AudioClipID id, Vector3 pos)
    {
        //  �w�肵���ʒu�ɉ����𐶐�
        var se = Instantiate(m_seObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        se.clip = m_audioDataBase.GetAudioData(id).AudioClip;
        se.volume = m_seVol;
        se.Play();
    }
}
