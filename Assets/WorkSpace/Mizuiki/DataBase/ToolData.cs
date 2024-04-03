using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolData", menuName = "CreateTool")]
public class ToolData : ScriptableObject
{
	// ツールの種類
	[System.Serializable]
	public enum ToolType
	{
		BOMB,       // 爆弾
		ARMOR,      // アーマー

		RARE,		// ここからレアツール

		RANGE_DESTROYER,		// 範囲破壊つるはし
		SHIELD,					// シールド
		SACRED_FLAME,			// 聖火

		HAMMER,					// ハンマー
		DRILL,					// ドリル
		LIMIT_TOTEM,			// 制限のトーテム

		OVER
	}

	// ツールの分類
	[System.Serializable]
	public enum ToolCategory
	{
		PUT,		// 設置型
		SUPPORT,	// 適応型

		OVER,
	}

	[Header("ツール名")]
	public string toolName = "";
	[Header("ツールの種類")]
	public ToolType type = ToolType.BOMB;
	[Header("ツールの分類")]
	public ToolCategory category = ToolCategory.SUPPORT;
	[Header("ツールのアイコン画像")]
	public Sprite sprite = null;
	[Header("リキャスト時間")]
	public float recastTime = 0.0f;
	[Header("設置する場合はプレハブ")]
	public GameObject objectPrefab = null;
	[Header("使用時の関数を呼び出すツール")]
	public Tool tool;

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

