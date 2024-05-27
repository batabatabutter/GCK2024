using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataBase", menuName = "CreateDataBase/Audio/AudioDataBase")]
public class AudioDataBase : ScriptableObject
{
    [Header("音声一覧")]
    [SerializeField] private List<AudioData> audioDatas;
    //  高速化用ハッシュセット
    private HashSet<AudioData> audioDataSet;

    //  なんちゃら時
    private void OnEnable()
    {
        audioDataSet = new HashSet<AudioData>(audioDatas);
    }

    //  オーディオデータ取得
    public AudioData GetAudioData(AudioDataID id)
    {
        AudioData aD = null;

        //  IDが一致しているものを返す
        foreach (AudioData audioData in audioDataSet.Where(x => x.AudioDataID == id))
        {
            aD = audioData;
        }

        //  音源なければエラー表示
        if (aD == null) Debug.Log("Error: AudioDataID[" + id + "]に対応する音源が存在しません。");

        return aD;
    }
}
