using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ToolDataBase", menuName = "CreateToolDataBase")]
public class ToolDataBase : ScriptableObject
{
    [Header("�c�[���̎�ޏ��Őݒ肵�Ă�")]
    public List<ToolData> tool;

    public List<SerializableKeyPair<ToolData.ToolType, ToolData>> tool2;

    [Header("Type�m�F�p")]
    public ToolData.ToolType type;
}
