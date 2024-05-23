using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy : DungeonAttackBase
{
	[Header("敵の生成スクリプト")]
	[SerializeField] private EnemyGenerator m_enemyGenerator = null;

	[Header("敵の種類")]
	[SerializeField] private Enemy.Type m_type;


	private void Start()
	{
		// 敵の生成スクリプトを取得
		m_enemyGenerator = GetComponent<EnemyGenerator>();
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		AttackOne(target.position, attackRank);
	}

	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		if (m_enemyGenerator == null)
		{
			Debug.Log("DA_Enemy : m_enemyGenerator を設定してね");
			return;
		}
		// 指定タイプの敵を生成する
		m_enemyGenerator.Spawn();
	}


	public EnemyGenerator EnemyGenerator
	{
		get { return m_enemyGenerator; }
	}
	public Enemy.Type Type
	{
		set { m_type = value; }
	}

}
