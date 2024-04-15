using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BlockDataBase", menuName = "CreateDataBase/Block/BlockDataBase")]
public class BlockDataBase : ScriptableObject
{
	[Header("ブロック(Type順で設定する)")]
	public List<BlockData> block;

	[Header("Type確認用")]
	public BlockData.BlockType type;
}
