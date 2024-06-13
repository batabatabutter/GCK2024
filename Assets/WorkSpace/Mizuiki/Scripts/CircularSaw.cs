using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
	[System.Serializable]
	public enum MiningType
	{
		NORMAL,		// バランス型
		RANGE,		// 範囲型
		POWER,		// パワー型
		SPEED,		// スピード型
		CRITICAL,	// クリティカル型
		DROP,		// ドロップ型

		OVER,
	}

	[System.Serializable]
	public struct CircularSawSprite
	{

	}

	[Header("丸のこの画像")]
	[SerializeField] private List<Sprite> m_sprites = new();

	[Header("スプライトの設定先")]
	[SerializeField] private SpriteRenderer m_spriteRenderer = null;



	private void Start()
	{
		// スプライトレンダーがなければ設定
		if (m_spriteRenderer == null)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}

	}

	// のこの種類設定
	public void SetType(MiningType type)
	{

	}

}
