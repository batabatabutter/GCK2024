using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData_", menuName = "CreateDataBase/Block/BlockData")]
public class BlockData : ScriptableObject
{
	[System.Serializable]
	public enum BlockType
	{
		STONE = 0,      // 石

		ORE_BEGIN = 100,// ここから鉱石
		COAL,           // 石炭
		STEEL,          // 鉄
		TIN,            // 錫
		LEAD,           // 鉛
		ORE_END,		// 鉱石終了

		BIRTHDAY = 200,     // ここから誕生石シリーズ
		BIR_GARNET,         // ガーネット
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

		SPECIAL = 500,  // ここから特殊ブロック
		CORE,           // コア
		BEDROCK,        // 岩盤
		DENGEROUS,      // 危険物

		TOACH = 1000,   // 松明
		BOMB,           // 爆弾

		OVER
	}

	[System.Serializable]
	public struct DropItems
	{
		[Header("アイテムの種類")]
		public ItemData.ItemType type;
		[Header("ドロップ数"), Min(0)]
		public int count;
		[Header("ドロップ率"), Range(0f, 1f)]
		public float rate;
	}

	[Header("ブロック名")]
	[SerializeField] private string blockName = "";
	[Header("ブロックの種類"), CustomEnum(typeof(BlockType))]
	[SerializeField] private string typeStr = "";
	private BlockType type;
	[Header("ブロックの耐久力")]
	[SerializeField] private float endurance = 100.0f;
	[Header("破壊不可能")]
	[SerializeField] private bool dontBroken = false;
	[Header("憑依可能")]
	[SerializeField] private bool canPossess = true;
	[Header("光源レベル")]
	[SerializeField] private int lightLevel = 0;
	[Header("ブロックのプレハブ")]
	[SerializeField] private GameObject prefab = null;
	[Header("ブロックの画像")]
	[SerializeField] private Sprite sprite = null;

	[Header("ドロップアイテム")]
	[SerializeField] private DropItems[] dropItem;

	[Header("---マップ---")]

	[Header("マップ表示の色")]
	[SerializeField] private Color color = Color.white;
	[Header("表示順")]
	[SerializeField] private int order = 0;
	[Header("マップ表示アイコン(あれば設定)")]
	[SerializeField] private Sprite mapIcon = null;

	public string Name => blockName;
	public BlockType Type => type;
	public float Endurance => endurance;
	public bool DontBroken => dontBroken;
	public bool CanPossess => canPossess;
	public int LightLevel => lightLevel;
	public GameObject Prefab => prefab;
	public Sprite Sprite => sprite;
	public DropItems[] DropItem => dropItem;
	public Color Color => color;
	public int Order => order;
	public Sprite MapIcon => mapIcon;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<BlockType>(typeStr);
	}

	public BlockData(BlockData block)
	{
		blockName = block.blockName;
		typeStr = block.typeStr;
		endurance = block.endurance;
		dontBroken = block.dontBroken;
		lightLevel = block.lightLevel;
		prefab = block.prefab;
		sprite = block.sprite;
		dropItem = block.dropItem;

		color = block.color;
		order = block.order;
		mapIcon = block.mapIcon;
	}

}

