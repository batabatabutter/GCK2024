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
		STEEL,			// 鉄
		TIN,			// 錫
		LEAD,			// 鉛
		COPPER,			// 銅

		BIRTHDAY_STONE = 200,	// ここから誕生石シリーズ
		BIR_GARNET,				// ガーネット
		BIR_AMETHYST,			// アメジスト
		BIR_AQUAMARINE,			// アクアマリン
		BIR_DIAMOND,			// ダイヤモンド
		BIR_EMERALD,			// エメラルド
		BIR_PEARL,				// パール
		BIR_RUBY,				// ルビー
		BIR_PERIDOT,			// ペリドット
		BIR_SAPPHIRE,			// サファイア
		BIR_OPAL,				// オパール
		BIR_TOPAZ,				// トパーズ
		BIR_TURQUOISE,			// ターコイズ

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

	public string Name => name;
	public ItemType Type => type;
	public Sprite Sprite => sprite;
	public GameObject Prefab => prefab;

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

