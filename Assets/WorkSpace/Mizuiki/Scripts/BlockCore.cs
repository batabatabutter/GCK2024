using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCore : Block
{
	[Header("---------- �R�A ----------")]

	[Header("�o��")]
	[SerializeField] private GameObject m_exit = null;


	// �j�󎞂̏���
	public override bool BrokenBlock(int dropCount = 1)
	{
		// �o������
		Instantiate(m_exit, transform.position, Quaternion.identity);

		return base.BrokenBlock(dropCount);
	}
}
