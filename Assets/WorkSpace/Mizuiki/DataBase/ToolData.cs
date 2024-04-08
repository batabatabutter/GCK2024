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

		NORMAL_NUM,		//	現在のツール数

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

	[Header("ツール名"), SerializeField]
	private string toolName = "";
	[Header("ツールの種類"), SerializeField, CustomEnum(typeof(ToolType))]
	private string typeStr;
	[Header("ツールの分類"), SerializeField]
	private ToolCategory category = ToolCategory.SUPPORT;
	[Header("ツールのアイコン画像"), SerializeField]
	private Sprite icon = null;
	[Header("リキャスト時間"), SerializeField]
	private float recastTime = 0.0f;
	[Header("ツールのプレハブ"), SerializeField]
	private GameObject prefab = null;

	[Header("ツール作成に必要な素材"), SerializeField]
	private Items[] itemMaterials = null;

	public string Name => toolName;
	public ToolType Type => SerializeUtil.Restore<ToolType>(typeStr);
	public ToolCategory Category => category;
	public Sprite Icon => icon;
	public float RecastTime => recastTime;
	public GameObject Prefab => prefab;
	public Items[] ItemMaterials => itemMaterials;


	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		typeStr = tool.typeStr;
		icon = tool.icon;
		recastTime = tool.recastTime;
		prefab = tool.prefab;
		itemMaterials = tool.itemMaterials;
	}

}

