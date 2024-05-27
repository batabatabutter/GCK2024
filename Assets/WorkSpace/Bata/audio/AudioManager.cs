using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("音源データベース")]
    [SerializeField] private AudioDataBase m_audioDataBase;

    [Header("オーディオソース")]
    [SerializeField] private AudioSource m_audioSource;

    [Header("効果音再生用空箱")]
    [SerializeField] private GameObject m_seObj;


    [Header("BGM音量")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_audioVol = 0.5f;

    [Header("効果音音量")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_seVol = 0.5f;

    //  効果音
    public float AudioVol
    {
        set { m_audioVol = value; }
        get { return m_audioVol; }
    }

    //  効果音
    public float SEVol
    {
        set { m_seVol = value; }
        get { return m_seVol; }
    }

    //  起動時
    public override void Awake()
    {
        base.Awake();

        //  音源取得
        if (m_audioDataBase == null)
            m_audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// 効果音再生
    /// </summary>
    /// <param name="clip">クリップ</param>
    public void PlaySE(AudioClip clip)
    {
        m_audioSource.PlayOneShot(clip, m_seVol);
    }

    /// <summary>
    /// 効果音再生
    /// </summary>
    /// <param name="ID">ID</param>
    public void PlaySE(AudioDataID id)
    {
        m_audioSource.PlayOneShot(m_audioDataBase.GetAudioData(id).AudioClip, m_seVol);
    }

    /// <summary>
    /// 効果音再生
    /// </summary>
    /// <param name="clip">クリップ</param>
    /// <param name="pos">座標</param>
    public void PlaySE(AudioClip clip, Vector3 pos)
    {
        var se = Instantiate(m_seObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        se.clip = clip;
        se.volume = m_seVol;
        se.Play();
    }

    /// <summary>
    /// 効果音再生
    /// </summary>
    /// <param name="data">データ</param>
    /// <param name="ID">ID</param>
    public void PlaySE(AudioDataID id, Vector3 pos)
    {
        //  指定した位置に音源を生成
        var se = Instantiate(m_seObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        se.clip = m_audioDataBase.GetAudioData(id).AudioClip;
        se.volume = m_seVol;
        se.Play();
    }
}
