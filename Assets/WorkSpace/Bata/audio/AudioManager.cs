using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("効果音再生")]
    [SerializeField] private AudioSource m_seSource;

    [Header("効果音再生用空箱")]
    [SerializeField] private GameObject m_seObj;

    [Header("効果音音量")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_seVol = 0.5f;

    //  効果音
    public float SEVol
    {
        set { m_seVol = value; }
        get { return m_seVol; }
    }

    /// <summary>
    /// 効果音再生
    /// </summary>
    /// <param name="clip">クリップ</param>
    public void PlaySE(AudioClip clip)
    {
        m_seSource.PlayOneShot(clip, m_seVol);
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
}
