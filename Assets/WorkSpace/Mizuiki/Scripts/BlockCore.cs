using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCore : Block
{
	[Header("---------- コア ----------")]

	[Header("出口")]
	[SerializeField] private GameObject m_exit = null;


	// 破壊時の処理
	public override bool BrokenBlock(int dropCount = 1)
	{
		// 出口生成
		Instantiate(m_exit, transform.position, Quaternion.identity);

		return base.BrokenBlock(dropCount);
	}
}
