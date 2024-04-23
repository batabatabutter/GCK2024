using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageNumScriptableObject", menuName = "StageNumScriptableObject", order = 0)]
public class StageNumScriptableObject : UnityEngine.ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int initStageNum = default;
    [NonSerialized] public int stageNum;

    public void OnAfterDeserialize()
    {
        // Editor��ł͍Đ����ɕύX����ScriptableObject���̒l�����s�I�����ɏ����Ȃ��B
        // ���̂��߁A�����l�Ǝ��s���Ɏg���ϐ��͕����Ă����A����������K�v������B
        stageNum = initStageNum;
    }

    public void OnBeforeSerialize() { /* do nothing */ }
}
