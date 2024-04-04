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
		TOACH	= 0,	// 松明
		BOMB,			// 爆弾
		ARMOR,			// アーマー

		RARE	= 100,		// ここからレアツール

		HOLY_TOACH,				// 聖火
		RANGE_DESTROYER,		// 範囲破壊つるはし
		SHIELD,					// シールド

		DRILL,					// ドリル
		DENGEROUS_BOMB,         // 危険爆弾
		HEAVY_ARMOR,			// 重鎧

		HEALING_TOACH,			// 癒しの松明
		DOUBLE_PICAXE,			// 倍増つるはし
		LIMIT_TOTEM,			// 制限のトーテム

		HAMMER,					// ハンマー
		MINING_BOMB,			// 採掘爆弾
		HEALING_AURA,			// 癒しのオーラ

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
	[Header("ツールのプレハブ")]
	public GameObject prefab = null;

	[Header("ツール作成に必要な素材")]
	public List<Items> itemMaterials = new List<Items>();

	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		type = tool.type;
		sprite = tool.sprite;
		recastTime = tool.recastTime;
		prefab = tool.prefab;
		itemMaterials = tool.itemMaterials;
	}

}

