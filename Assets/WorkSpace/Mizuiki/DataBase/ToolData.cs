using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolData", menuName = "CreateTool")]
public class ToolData : ScriptableObject
{
	[System.Serializable]
	public enum ToolType
	{
		TOACH,      // 松明
		BOMB,       // 爆弾

		OVER
	}

	public string toolName = "";                 // 名前
	public ToolType toolType = ToolType.TOACH;   // 種類
	public Sprite sprite = null;				 // 画像

	public List<ItemData> itemMaterials = new List<ItemData>();

	//public List<Item.Type>	craftMaterialType  = new();		// 必要アイテム種
	//public List<int>		craftMaterialCount = new();		// 必要アイテム数

	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		toolType = tool.toolType;
		sprite = tool.sprite;
		itemMaterials = tool.itemMaterials;
	}

}

