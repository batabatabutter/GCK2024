using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy02 : DA_Enemy
{
	[Header("---------- ドリノコサンド ----------")]
	[Header("サンド距離")]
	[SerializeField] private float m_distance = 3.0f;

	private void Start()
	{
		// タイプ指定アリ
		Type = Enemy.Type.Dorinoko;
		IsType = true;
	}

	// ドリノコサンド
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// サンド方向取得
		MyFunction.Direction dir = MyFunction.GetDirection(direction);

		// 横サンド
		bool horizontal = (int)dir % 2 == 1;

		// ターゲットからの距離
		Vector3 targetDistance = Vector3.zero;
		// 加算量
		Vector3 add = Vector3.zero;
		// 攻撃位置
		Vector3 attackPos = target.position;

		// 横サンド
		if (horizontal)
		{
			targetDistance.x = m_distance;
			add.y = 1;
			attackPos.y -= Mathf.Round(range / 2);
		}
		// 縦サンド
		else
		{
			targetDistance.y = m_distance;
			add.x = 1;
			attackPos.x -= Mathf.Round(range / 2);
		}

		for (int i = 0; i < (int)range; i++)
		{
			// 攻撃位置に敵を生成
			AttackOne(attackPos + targetDistance, attackRank);
			AttackOne(attackPos - targetDistance, attackRank);
			attackPos += add;
		}

	}
}
