using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AudioData_", menuName = "CreateDataBase/Audio/AudioData")]
public class AudioData : ScriptableObject
{
    [Header("�����t�@�C��")]
    [SerializeField] private AudioClip audioClip;
    public AudioClip AudioClip => audioClip;

    [Header("����ID")]
    [SerializeField][CustomEnum(typeof(AudioClipID))] private string audioClipID;
    public AudioClipID AudioClipID => SerializeUtil.Restore<AudioClipID>(audioClipID);

}
