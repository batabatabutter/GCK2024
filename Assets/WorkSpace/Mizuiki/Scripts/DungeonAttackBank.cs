using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackBank : DungeonAttackBase
{
	[Header("土手")]
	[SerializeField] GameObject m_prefab;
	[Header("土手ハイライト")]
	[SerializeField] GameObject m_highlight;


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

	public override void Attack(Transform target, int attackRank = 1)
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

		Vector3 pos = target.position;

		pos.x -= 1;
		pos.y += 1;

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (i == 1 && j == 1)
					continue;


				Instantiate(m_prefab, new Vector3(pos.x + j, pos.y - i, 0), Quaternion.identity);
				Instantiate(m_highlight, new Vector3(pos.x + j, pos.y - i, 0), Quaternion.identity);
			}

		}

	}

}
