using System;
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
		COPPER,         // 銅
		TIN,            // 錫
		LEAD,           // 鉛
		IRON,           // 鉄
		STEEL,          // 鋼
		ORE_END,		// 鉱石終了

		BIRTHDAY		= 1000,     // ここから誕生石シリーズ
		BIR_GARNET		= 1010,     // ガーネット
		BIR_AMETHYST	= 1020,		// アメジスト
		BIR_AQUAMARINE	= 1030,     // アクアマリン
		BIR_DIAMOND		= 1040,     // ダイヤモンド
		BIR_EMERALD		= 1050,     // エメラルド
		BIR_PEARL		= 1060,     // パール
		BIR_RUBY		= 1070,     // ルビー
		BIR_PERIDOT		= 1080,     // ペリドット
		BIR_SAPPHIRE	= 1090,     // サファイア
		BIR_OPAL		= 1100,     // オパール
		BIR_TOPAZ		= 1110,     // トパーズ
		BIR_TURQUOISE	= 1120,     // ターコイズ

		SPECIAL = 500,  // ここから特殊ブロック
		CORE,           // コア
		BEDROCK,        // 岩盤
		DENGEROUS,      // 危険物

		TOACH = 10000,   // 松明
		BOMB,           // 爆弾

		OVER
	}

	[System.Serializable]
	public struct DropItems
	{
		[Tooltip("アイテムの種類"), CustomEnum(typeof(ItemData.ItemType))]
		public string typeStr;
		[NonSerialized]
		public ItemData.ItemType type;
		[Tooltip("ドロップ数"), Min(0)]
		public int count;
		[Tooltip("ドロップ率"), Range(0f, 1f)]
		public float rate;
	}

	[Header("ブロック名")]
	[SerializeField] private string blockName = "";
	[Header("ブロックの種類"), CustomEnum(typeof(BlockType))]
	[SerializeField] private string typeStr = "";
	private BlockType type;
	[Header("色")]
	[SerializeField] private Color color = Color.white;
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
	[Header("採掘音")]
    [SerializeField] private AudioClip miningSE = null;
    [Header("破壊音")]
    [SerializeField] private AudioClip destroySE = null;

    [Header("ドロップアイテム")]
	[SerializeField] private DropItems[] dropItem;

	[Header("---マップ---")]

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
	public AudioClip MiningSE => miningSE;
	public AudioClip DestroySE => destroySE;
	public DropItems[] DropItem => dropItem;
	public Color Color => color;
	public int Order => order;
	public Sprite MapIcon => mapIcon;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<BlockType>(typeStr);

		for (int i = 0; i < dropItem.Length; i++)
		{
			dropItem[i].type = SerializeUtil.Restore<ItemData.ItemType>(dropItem[i].typeStr);
		}
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

