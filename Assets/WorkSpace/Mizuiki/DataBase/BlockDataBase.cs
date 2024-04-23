using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BlockDataBase", menuName = "CreateDataBase/Block/BlockDataBase")]
public class BlockDataBase : ScriptableObject
{
	[Header("�u���b�N(Type���Őݒ肷��)")]
	public List<BlockData> block;

	[Header("Type�m�F�p")]
	public BlockData.BlockType type;
}
