using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "ToolDataBase", menuName = "CreateToolDataBase")]
public class ToolDataBase : ScriptableObject
{
    [Header("ツールの種類順で設定してね")]
    public List<ToolData> tool;

    public List<SerializableKeyPairCustomEnum<ToolData.ToolType, ToolData>> toolData;
    public Dictionary<ToolData.ToolType, ToolData> toolDic;

    [Header("Type確認用")]
    public ToolData.ToolType type;
    [CustomEnum(typeof(ToolData.ToolType))] public string type2;
    public ToolData.ToolType type2e => SerializeUtil.Restore<ToolData.ToolType>(type2.ToString());

    //  データ起動時
    private void OnEnable()
    {
        toolDic = SerializableKeyPairCustomEnum<ToolData.ToolType, ToolData>.ConvertToDictionaly(toolData);
    }
}
