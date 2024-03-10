using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "ToolData", menuName = "CreateTool")]
public class ToolData : ScriptableObject
{
	public enum ToolType
	{
		TOACH,      // 松明
		BOMB,       // 爆弾

		OVER
	}

	public string toolName = "";                 // 名前
	public ToolType toolType = ToolType.TOACH;     // 種類

	public List<ItemData> itemMaterials = new List<ItemData>();

	//public List<Item.Type>	craftMaterialType  = new();		// 必要アイテム種
	//public List<int>		craftMaterialCount = new();		// 必要アイテム数

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
