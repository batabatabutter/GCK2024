using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ToolDataBase", menuName = "CreateToolDataBase")]
public class ToolDataBase : ScriptableObject
{
    public List<ToolData> tool;
}
