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

	[Header("�u���b�N��")]
	public string blockName = "";
	//[Header("�u���b�N�̎��")]
	//public ToolType type = ToolType.OVER;
	[Header("�u���b�N�̉摜")]
	public Sprite sprite = null;
	[Header("�u���b�N�̐F")]
	public Color color = Color.white;

	public BlockData(BlockData block)
	{
		blockName = block.blockName;
		sprite = block.sprite;
	}

}

