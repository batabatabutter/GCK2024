using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "ToolData", menuName = "CreateTool")]
public class ToolData : ScriptableObject
{
	public enum ToolType
	{
		TOACH,      // ����
		BOMB,       // ���e

		OVER
	}

	public string toolName = "";                 // ���O
	public ToolType toolType = ToolType.TOACH;     // ���

	public List<ItemData> itemMaterials = new List<ItemData>();

	//public List<Item.Type>	craftMaterialType  = new();		// �K�v�A�C�e����
	//public List<int>		craftMaterialCount = new();		// �K�v�A�C�e����

	public ToolData(ToolData tool)
	{
		this.toolName = tool.toolName;
		this.toolType = tool.toolType;
		this.itemMaterials = tool.itemMaterials;
	}

}


[CreateAssetMenu(fileName = "ToolDataBase", menuName = "CreateToolDataBase")]
public class ToolDataBase : ScriptableObject
{
    public List<ToolData> tool;
}
