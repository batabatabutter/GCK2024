using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData", menuName = "CreateBlock")]
public class BlockData : ScriptableObject
{
	[System.Serializable]
	public enum BlockType
	{
		STONE = 0,		// 石
		COAL,			// 石炭
		STEEL,			// 鉄
		TIN,			// 錫
		LEAD,			// 鉛

		SPECIAL = 100,	// ここから特殊ブロック
		CORE,			// コア
		BEDROCK,		// 岩盤
		DENGEROUS,		// 危険物

		BIRTHDAY_BLOCK = 200,	// ここから誕生石シリーズ
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

		TOACH = 1000,	// 松明
		BOMB,			// 爆弾

		OVER
	}

	[System.Serializable]
	public struct DropItems
	{
		[Header("アイテムの種類")]
		public ItemData.Type type;
		[Header("ドロップ数"), Min(0)]
		public int count;
		[Header("ドロップ率"), Range(0f, 1f)]
		public float rate;
	}

	[Header("ブロック名")]
	public string blockName = "";
	[Header("ブロックの種類")]
	public BlockType type = BlockType.OVER;
	[Header("ブロックの耐久力")]
	public float endurance = 100.0f;
	[Header("破壊不可能")]
	public bool dontBroken = false;
	[Header("光源レベル")]
	public int lightLevel = 0;
	[Header("ブロックのプレハブ")]
	public GameObject prefab = null;
	[Header("ブロックの画像")]
	public Sprite sprite = null;

	[Header("ドロップアイテム")]
	public DropItems[] dropItem;

	[Header("---マップ---")]

	[Header("マップ表示の色")]
	public Color color = Color.white;
	[Header("表示順")]
	public int order = 0;
	[Header("マップ表示アイコン(あれば設定)")]
	public Sprite mapIcon = null;

	public BlockData(BlockData block)
	{
		blockName = block.blockName;
		sprite = block.sprite;
	}

}

