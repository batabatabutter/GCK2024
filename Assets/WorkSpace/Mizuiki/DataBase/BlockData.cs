using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData", menuName = "CreateBlock")]
public class BlockData : ScriptableObject
{
	[System.Serializable]
	public enum ToolType
	{
		CORE,		// コア
		BEDROCK,	// 岩盤
		STONE,		// 石
		COAL,		// 石炭

		OVER
	}

	[Header("ブロック名")]
	public string blockName = "";
	[Header("ブロックの種類")]
	public ToolType type = ToolType.OVER;
	[Header("ブロックの画像")]
	public Sprite sprite = null;
	[Header("ブロックのプレハブ")]
	public GameObject prefab = null;

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

