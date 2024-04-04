using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData", menuName = "CreateBlock")]
public class BlockData : ScriptableObject
{
	[System.Serializable]
	public enum BlockType
	{
		CORE,		// コア
		BEDROCK,	// 岩盤
		STONE,		// 石
		COAL,		// 石炭
		STEEL,		// 鉄
		TIN,		// 錫
		LEAD,       // 鉛

		BIRTHDAY_BLOCK = 200,		// ここから誕生石シリーズ
		BIR_GARNET,			// ガーネット
		BIR_AMETHYST,       // アメジスト
		BIR_AQUAMARINE,     // アクアマリン
		BIR_DIAMOND,        // ダイヤモンド
		BIR_EMERALD,        // エメラルド
		BIR_PEARL,          // パール
		BIR_RUBY,           // ルビー
		BIR_PERIDOT,        // ペリドット
		BIR_SAPPHIRE,       // サファイア
		BIR_OPAL,           // オパール
		BIR_TOPAZ,          // トパーズ
		BIR_TURQUOISE,      // ターコイズ

		OVER
	}

	[Header("ブロック名")]
	public string blockName = "";
	[Header("ブロックの種類")]
	public BlockType type = BlockType.OVER;
	[Header("ブロックのプレハブ")]
	public GameObject prefab = null;
	[Header("ブロックの画像")]
	public Sprite sprite = null;

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

