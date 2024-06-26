using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AudioData_", menuName = "CreateDataBase/Audio/AudioData")]
public class AudioData : ScriptableObject
{
    [Header("音声ファイル")]
    [SerializeField] private AudioClip audioClip;
    public AudioClip AudioClip => audioClip;

    [Header("音声ID")]
    [SerializeField][CustomEnum(typeof(AudioDataID))] private string audioDataID;
    public AudioDataID AudioDataID => SerializeUtil.Restore<AudioDataID>(audioDataID);

}
