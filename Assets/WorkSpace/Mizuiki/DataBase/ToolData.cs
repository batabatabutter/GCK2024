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
		ARMOR,		// アーマー
		UPGRADE,	// 採掘アップグレード

		OVER
	}

	[Header("ツール名")]
	public string toolName = "";
	[Header("ツールの種類")]
	public ToolType type = ToolType.TOACH;
	[Header("ツールのアイコン画像")]
	public Sprite sprite = null;
	[Header("リキャスト時間")]
	public float recastTime = 0.0f;
	[Header("設置する場合はプレハブ")]
	public GameObject objectPrefab = null;

	[Header("ツール作成に必要な素材")]
	public List<Items> itemMaterials = new List<Items>();

	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		type = tool.type;
		sprite = tool.sprite;
		recastTime = tool.recastTime;
		objectPrefab = tool.objectPrefab;
		itemMaterials = tool.itemMaterials;
	}

}

