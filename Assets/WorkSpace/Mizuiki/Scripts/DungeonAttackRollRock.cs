using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackRollRock : DungeonAttackBase
{
	[Header("転がる岩")]
	[SerializeField] GameObject m_rollRockPrefab;
	[Header("矢印のハイライト")]
	[SerializeField] GameObject m_rollRockHighlight;
	[Header("岩が出現する距離")]
	[SerializeField] private float m_targetDistance = 5.0f;


	private void Start()
	{
		if (m_rollRockPrefab == null)
		{
			Debug.Log("RollRock : プレハブを設定してね");
		}
		if (m_rollRockHighlight == null)
		{
			Debug.Log("RollRock : プレハブを設定してね");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="target"></param>
	/// <param name="attackRank">方向(MyFunction.Direction)</param>
	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		// プレハブがない
		if (m_rollRockPrefab == null)
		{
			return;
		}

		// 攻撃対象がない
		if (target == null)
		{
			Debug.Log("攻撃対象がいないよ");
			return;
		}

		// 生成位置
		Vector3 rollingPos = target;
		// 生成角度
		float rollingRotation = 0.0f;

		//4通りだから０～３
		//MyFunction.Direction direction = MyFunction.GetRandomDirection();
		MyFunction.Direction direction = (MyFunction.Direction)attackRank;
		// 指定方向から攻撃
		switch (direction)
		{
			case MyFunction.Direction.UP:
				// 上から
				rollingPos = new Vector3(target.x, target.y + m_targetDistance, 0);
				rollingRotation = 180;
				break;

			case MyFunction.Direction.DOWN:
				// 下から
				rollingPos = new Vector3(target.x, target.y - m_targetDistance, 0);
				rollingRotation = 0;
				break;

			case MyFunction.Direction.LEFT:
				// 左から
				rollingPos = new Vector3(target.x - m_targetDistance, target.y, 0);
				rollingRotation = 270;
				break;

			case MyFunction.Direction.RIGHT:
				// 右から
				rollingPos = new Vector3(target.x + m_targetDistance, target.y, 0);
				rollingRotation = 90;
				break;
		}
		// 転がる岩の生成
		Instantiate(m_rollRockPrefab, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
		// ハイライトの生成
		Instantiate(m_rollRockHighlight, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
	}

}
