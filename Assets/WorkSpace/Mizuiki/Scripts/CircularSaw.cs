using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
	[System.Serializable]
	public struct CircularSawSprite
	{
		public MiningData.MiningType type;
		public Sprite sprite;
	}

	[Header("丸のこの移動速度")]
	[SerializeField] private float m_circularSawSpeed = 1.0f;
	[Header("丸のこの回転速度")]
	[SerializeField] private float m_circularSawRotate = 100.0f;

	[Header("丸のこの画像")]
	[SerializeField] private List<CircularSawSprite> m_sprites = new();

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

	// 回す
	public void Rotate(float speed)
	{
		transform.localEulerAngles += m_circularSawRotate * speed * Time.deltaTime * Vector3.back;
	}

	// 丸のこの位置取得
	public Vector3 SetPosition(Vector3 miningPoint)
	{
		// 丸のこから採掘位置へのベクトル
		Vector3 circularSawToMining = miningPoint - transform.position;
		// 丸のこと採掘位置の距離
		float distance = circularSawToMining.magnitude;

		// 距離が 1f の移動量以内ならそのまま採掘地点を返す
		if (distance <= m_circularSawSpeed * Time.deltaTime)
			return miningPoint;

		// 採掘位置へのベクトル正規化
		circularSawToMining.Normalize();

		// 丸のこの位置を返す
		return transform.position + (circularSawToMining * Time.deltaTime * m_circularSawSpeed);
	}

	// のこの種類設定
	public void SetType(MiningData.MiningType type)
	{

	}

}
