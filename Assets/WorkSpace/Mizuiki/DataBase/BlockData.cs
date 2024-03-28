using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData", menuName = "CreateBlock")]
public class BlockData : ScriptableObject
{
	//[System.Serializable]
	//public enum ToolType
	//{

	//	OVER
	//}

	[Header("ブロック名")]
	public string blockName = "";
	//[Header("ブロックの種類")]
	//public ToolType type = ToolType.OVER;
	[Header("ブロックの画像")]
	public Sprite sprite = null;
	[Header("ブロックの色")]
	public Color color = Color.white;

	public BlockData(BlockData block)
	{
		blockName = block.blockName;
		sprite = block.sprite;
	}

}

