using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolData_", menuName = "CreateDataBase/Tool/ToolData")]
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

		RARE	= ItemData.ItemType.BIRTHDAY_STONE,		// ここからレアツール

		// エメラルド : 5月
		HEALING_TOACH = ItemData.ItemType.BIR_EMERALD,	// 癒しの松明
		DOUBLE_PICAXE,									// 倍増つるはし
		LIMIT_TOTEM,									// 制限のトーテム

		// ルビー : 7月
		DRILL = ItemData.ItemType.BIR_RUBY,				// ドリル
		DENGEROUS_BOMB,									// 危険爆弾
		HEAVY_ARMOR,									// 重鎧

		// サファイア : 9月
		HAMMER = ItemData.ItemType.BIR_SAPPHIRE,		// ハンマー
		MINING_BOMB,									// 採掘爆弾
		HEALING_AURA,									// 癒しのオーラ

		// トパーズ : 11月
		HOLY_TOACH = ItemData.ItemType.BIR_TOPAZ,		// 聖火
		RANGE_DESTROYER,								// 範囲破壊つるはし
		SHIELD,											// シールド

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
	private ToolType type;
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
	public ToolType Type => type;
	public ToolCategory Category => category;
	public Sprite Icon => icon;
	public float RecastTime => recastTime;
	public GameObject Prefab => prefab;
	public Items[] ItemMaterials => itemMaterials;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<ToolType>(typeStr);
	}

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

