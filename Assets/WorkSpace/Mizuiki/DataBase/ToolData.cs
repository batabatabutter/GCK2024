using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolData", menuName = "CreateTool")]
public class ToolData : ScriptableObject
{
	[System.Serializable]
	public enum ToolType
	{
		TOACH,      // ����
		BOMB,       // ���e

		OVER
	}

	public string toolName = "";                 // ���O
	public ToolType toolType = ToolType.TOACH;   // ���
	public Sprite sprite = null;				 // �摜

	public List<ItemData> itemMaterials = new List<ItemData>();

	//public List<Item.Type>	craftMaterialType  = new();		// �K�v�A�C�e����
	//public List<int>		craftMaterialCount = new();		// �K�v�A�C�e����

	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		toolType = tool.toolType;
		sprite = tool.sprite;
		itemMaterials = tool.itemMaterials;
	}

}

