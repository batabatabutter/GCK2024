using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataBase", menuName = "CreateDataBase/Audio/AudioDataBase")]
public class AudioDataBase : ScriptableObject
{
    [Header("�����ꗗ")]
    [SerializeField] private List<AudioData> audioDatas;
    //  �������p�n�b�V���Z�b�g
    private HashSet<AudioData> audioDataSet;

    //  �Ȃ񂿂�玞
    private void OnEnable()
    {
        audioDataSet = new HashSet<AudioData>(audioDatas);
    }

    //  �I�[�f�B�I�f�[�^�擾
    public AudioData GetAudioData(AudioDataID id)
    {
        AudioData aD = null;

        //  ID����v���Ă�����̂�Ԃ�
        foreach (AudioData audioData in audioDataSet.Where(x => x.AudioDataID == id))
        {
            aD = audioData;
        }

        //  �����Ȃ���΃G���[�\��
        if (aD == null) Debug.Log("Error: AudioDataID[" + id + "]�ɑΉ����鉹�������݂��܂���B");

        return aD;
    }
}
