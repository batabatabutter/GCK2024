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
        // Editor上では再生中に変更したScriptableObject内の値が実行終了時に消えない。
        // そのため、初期値と実行時に使う変数は分けておき、初期化する必要がある。
        stageNum = initStageNum;
    }

    public void OnBeforeSerialize() { /* do nothing */ }
}
