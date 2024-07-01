using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using static BlockData;
using static ToolData;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData_", menuName = "CreateDataBase/Item/ItemData")]
public class ItemData : ScriptableObject
{
	[System.Serializable]
	public enum ItemType
	{
		STONE	= 0,	// 岩
		COAL,			// 石炭
		COPPER,			// 銅
		TIN,			// 錫
		LEAD,			// 鉛
		IRON,			// 鉄
		STEEL,			// 鋼

		BIRTHDAY_STONE	= 1000,		// ここから誕生石シリーズ
		BIR_GARNET		= 1010,		// ガーネット
		BIR_AMETHYST	= 1020,		// アメジスト
		BIR_AQUAMARINE	= 1030,		// アクアマリン
		BIR_DIAMOND		= 1040,		// ダイヤモンド
		BIR_EMERALD		= 1050,		// エメラルド
		BIR_PEARL		= 1060,		// パール
		BIR_RUBY		= 1070,		// ルビー
		BIR_PERIDOT		= 1080,		// ペリドット
		BIR_SAPPHIRE	= 1090,		// サファイア
		BIR_OPAL		= 1100,		// オパール
		BIR_TOPAZ		= 1110,		// トパーズ
		BIR_TURQUOISE	= 1120,		// ターコイズ

		OVER,
	}

	[Header("アイテム名"), SerializeField]
	private string itemName;
	[Header("アイテムの種類"), SerializeField ,CustomEnum(typeof(ItemType))]
	private string typeStr;
	private ItemType type;
	[Header("アイテムの画像"), SerializeField]
	private Sprite sprite;
	[Header("アイテムのプレハブ"), SerializeField]
	private GameObject prefab = null;
	[Header("アイテムの色"), SerializeField]
	private Color color = Color.white;

	public string Name => name;
	public ItemType Type => type;
	public Sprite Sprite => sprite;
	public GameObject Prefab => prefab;
	public Color Color => color;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<ItemType>(typeStr);
	}

	public ItemData(ItemData item)
	{
		itemName = item.itemName;
		typeStr = item.typeStr;
		sprite = item.sprite;
	}
}

[System.Serializable]
public class Items
{
    [CustomEnum(typeof(ItemData.ItemType))]
	[SerializeField] private string typeStr;       // 種類
	//[NonSerialized] public ItemData.ItemType type;
	public ItemData.ItemType Type => SerializeUtil.Restore<ItemData.ItemType>(typeStr);
    public int count;       // 数
}

