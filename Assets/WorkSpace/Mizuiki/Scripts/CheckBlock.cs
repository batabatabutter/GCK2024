using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : Block
{
	[Header("確認ブロック")]

	[Header("意思確認キャンバス")]
	[SerializeField] private CheckCanvas m_checkCanvas = null;

	[Header("コアアイコンのスプライト")]
	[SerializeField] private SpriteRenderer m_spriteRenderer = null;

	[Header("たたいたときの音")]
	[SerializeField] private AudioClip m_tappedAudio = null;



	// ダメージを与えられたら確認キャンバスを出す
	public override bool AddMiningDamage(float power, int dropCount = 1)
	{
		// キャンバス表示
		m_checkCanvas.SetEnabled(true);

		// 採掘音を鳴らす
		AudioManager.Instance.PlaySE(m_tappedAudio, transform.position);

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
