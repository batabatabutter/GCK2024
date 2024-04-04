using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItem")]
public class ItemData : ScriptableObject
{
	[System.Serializable]
	public enum Type
	{
		STONE	= 0,	// 岩
		COAL,			// 石炭
		STEEL,			// 鉄
		TIN,			// 錫
		LEAD,			// 鉛

		BIR_GARNET	= 200,	// ガーネット
		BIR_AMETHYST,		// アメジスト
		BIR_AQUAMARINE,		// アクアマリン
		BIR_DIAMOND,		// ダイヤモンド
		BIR_EMERALD,		// エメラルド
		BIR_PEARL,			// パール
		BIR_RUBY,			// ルビー
		BIR_PERIDOT,		// ペリドット
		BIR_SAPPHIRE,		// サファイア
		BIR_OPAL,			// オパール
		BIR_TOPAZ,			// トパーズ
		BIR_TURQUOISE,		// ターコイズ

		OVER,
	}

	[Header("アイテム名")]
	public string itemName;
	[Header("アイテムの種類")]
	public Type type;
	[Header("アイテムの画像")]
	public Sprite sprite;
	[Header("アイテムのプレハブ")]
	public GameObject prefab = null;

	public ItemData(ItemData item)
	{
		itemName = item.itemName;
		type = item.type;
		sprite = item.sprite;
	}
}

[System.Serializable]
public class Items
{
	public ItemData.Type type;       // 種類
	public int count;       // 数
}

