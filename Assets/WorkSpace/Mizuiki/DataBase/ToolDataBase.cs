using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "ToolDataBase", menuName = "CreateToolDataBase")]
public class ToolDataBase : ScriptableObject
{
    [Header("�c�[���̎�ޏ��Őݒ肵�Ă�")]
    public List<ToolData> tool;

    public List<SerializableKeyPairCustomEnum<ToolData.ToolType, ToolData>> toolData;
    public Dictionary<ToolData.ToolType, ToolData> toolDic;
    public List<SerializableKeyPairCustomEnum<ToolData.ToolType, ToolData>> toolNormalData;
    public Dictionary<ToolData.ToolType, ToolData> toolNormalDic;
    public List<SerializableKeyPairCustomEnum<ToolData.ToolType, ToolData>> toolRareData;
    public Dictionary<ToolData.ToolType, ToolData> toolRareDic;

    [Header("Type�m�F�p")]
    public ToolData.ToolType type;

    //  �f�[�^�N����
    private void OnEnable()
    {
        toolDic = SerializableKeyPairCustomEnum<ToolData.ToolType, ToolData>.ConvertToDictionaly(toolData);
    }
}
