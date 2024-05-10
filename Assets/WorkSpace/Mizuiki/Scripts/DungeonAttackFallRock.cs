using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackFallRock : DungeonAttackBase
{
	//プレイヤーの下に出るハイライトの低さ
	const float HEIGLIGHT_HEIGHT = 0.5f;

	[Header("プレハブ")]
	[SerializeField] GameObject m_prefab;
	[Header("ハイライト")]
	[SerializeField] GameObject m_highlight;
	[Header("落石の生成する高さ")]
	[SerializeField] float m_rockHeight = 3.0f;


	private void Start()
	{
		if (m_prefab == null)
		{
			Debug.Log("FallRock : プレハブを設定してね");
		}
		if (m_highlight == null)
		{
			Debug.Log("FallRock : プレハブを設定してね");
		}
	}


	// 攻撃
	public override void Attack(Transform target, int attackRank = 1)
	{
		// パターンその1
		//Vector3 random = new(Random.Range(-5, 5), Random.Range(-5, 5), 0.0f)
		//AttackOne(target.position + random, attackRank);

		// 塊のサイズ
		int massSize = 3 + (attackRank * 2);
		int massRange = massSize / 2;
		// ターゲットのグリッド取得
		Vector2Int targetGrid = MyFunction.RoundHalfUpInt(target.position);
		// パターンその2
		for (int y = targetGrid.y - massRange; y <= targetGrid.y + massRange; y++)
		{
			for (int x = targetGrid.x - massRange; x <= targetGrid.x + massRange; x++)
			{
				// 攻撃発生位置
				Vector3 attackPos = new(x, y, 0);
				// 攻撃発生
				AttackOne(attackPos);
			}
		}

	}

	// 攻撃1つ
	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		// プレハブがない
		if (m_prefab == null)
		{
			return;
		}

		// 攻撃対象がない
		if (target == null)
		{
			Debug.Log("攻撃対象がいないよ");
			return;
		}

		// 岩の落ちる位置
		Vector3 rockfallPos = new(target.x, target.y + m_rockHeight, 0);
		// 岩の生成
		Instantiate(m_prefab, rockfallPos, Quaternion.identity);

		// ハイライトの出現位置
		Vector3 highlightPos = new(target.x, target.y - HEIGLIGHT_HEIGHT, 0);
		// ハイライトの生成
		Instantiate(m_highlight, highlightPos, Quaternion.identity);
	}

}
