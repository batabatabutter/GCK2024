using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ToolDataBase", menuName = "CreateToolDataBase")]
public class ToolDataBase : ScriptableObject
{
    [Header("ツールの種類順で設定してね")]
    public List<ToolData> tool;

    public List<SerializableKeyPair<ToolData.ToolType, ToolData>> tool2;

    [Header("Type確認用")]
    public ToolData.ToolType type;
}
