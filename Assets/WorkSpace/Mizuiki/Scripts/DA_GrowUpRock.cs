using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DA_GrowUpRock : DungeonAttackBase
{
	[Header("土手")]
	[SerializeField] GameObject m_bankPrefab;
	[Header("土手ハイライト")]
	[SerializeField] GameObject m_bankHighlight;


	private void Start()
	{
		if (m_bankPrefab == null)
		{
			Debug.Log("FallRock : プレハブを設定してね");
		}
		if (m_bankHighlight == null)
		{
			Debug.Log("FallRock : プレハブを設定してね");
		}
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// プレハブがない
		if (m_bankPrefab == null)
		{
			return;
		}

		// 攻撃対象がない
		if (target == null)
		{
			Debug.Log("攻撃対象がいないよ");
			return;
		}

		Vector3 pos = target.position;

		AttackOne(pos, attackRank);

	}

	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		// ターゲットの位置に攻撃を出す
		// 攻撃生成
		Instantiate(m_bankPrefab, new Vector3(target.x, target.y, 0), Quaternion.identity);
		// ハイライト生成
		Instantiate(m_bankHighlight, new Vector3(target.x, target.y, 0), Quaternion.identity);
	}

}
