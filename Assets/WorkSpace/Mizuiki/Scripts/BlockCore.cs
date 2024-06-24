using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCore : Block
{
	[Header("oŒû")]
	[SerializeField] private GameObject m_exit = null;


	// ”j‰ó‚Ìˆ—
	public override bool BrokenBlock(int dropCount = 1)
	{
		// oŒû¶¬
		Instantiate(m_exit, transform.position, Quaternion.identity);

		return base.BrokenBlock(dropCount);
	}
}
