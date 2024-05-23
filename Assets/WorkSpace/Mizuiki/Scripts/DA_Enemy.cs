using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy : DungeonAttackBase
{
	[Header("�G�̐����X�N���v�g")]
	[SerializeField] private EnemyGenerator m_enemyGenerator = null;

	[Header("�G�̎��")]
	[SerializeField] private Enemy.Type m_type;


	private void Start()
	{
		// �G�̐����X�N���v�g���擾
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
			Debug.Log("DA_Enemy : m_enemyGenerator ��ݒ肵�Ă�");
			return;
		}
		// �w��^�C�v�̓G�𐶐�����
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
