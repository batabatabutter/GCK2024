using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BlockDataBase", menuName = "CreateBlockDataBase")]
public class BlockDataBase : ScriptableObject
{
	[Header("�u���b�N")]
	public List<BlockData> block;
}
