using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ObjectLight : MonoBehaviour
{
	[Header("光源レベル")]
	[SerializeField] private int m_lightLevel = 0;

	private void Awake()
	{
		FlashLight(m_lightLevel);
	}

	public void FlashLight(int lightLevel)
	{
		// 明るさレベルの設定
		m_lightLevel = lightLevel;

		// 明るさレベルを持ってるか
		if (lightLevel > 0)
		{
			// コライダー設定
			AddCricleColToDelete(lightLevel);
		}
	}


	private void AddCricleColToDelete(int lightLevel)
	{
		//// リジッドボディがなければ追加
		//if (!gameObject.GetComponent<Rigidbody2D>())
		//{
		//	Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		//	rb.isKinematic = true;

		//}

		// 円のコライダーがなければ追加
		if (!gameObject.GetComponent<CircleCollider2D>())
		{
			CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();

			//明るさレベルで大きさ指定
			circleCol.radius = lightLevel;
			circleCol.isTrigger = true;
		}
	}


	public int LightLevel
	{
		get { return m_lightLevel; }
		set {  m_lightLevel = value; }
	}

}
