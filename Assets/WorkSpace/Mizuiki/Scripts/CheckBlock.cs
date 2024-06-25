using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : Block
{
	[Header("意思確認キャンバス")]
	[SerializeField] private CheckCanvas m_checkCanvas = null;

	[Header("コアアイコンのスプライト")]
	[SerializeField] private SpriteRenderer m_spriteRenderer = null;



	// ダメージを与えられたら確認キャンバスを出す
	public override bool AddMiningDamage(float power, int dropCount = 1)
	{
		// キャンバス表示
		m_checkCanvas.SetEnabled(true);

		return false;
	}


	// キャンバス
	public CheckCanvas CheckCanvas
	{
		set { m_checkCanvas = value; }
	}
	// コアアイコン
	public SpriteRenderer CoreIcon
	{
		get { return m_spriteRenderer; }
		set { m_spriteRenderer = value; }
	}

}
