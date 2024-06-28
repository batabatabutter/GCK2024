using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy : DungeonAttackBase
{
	[Header("�G�̐����X�N���v�g")]
	[SerializeField] private EnemyGenerator m_enemyGenerator = null;

	[Header("�G�̎��")]
	[SerializeField] private Enemy.Type m_type;
	[SerializeField] private bool m_isType;

	[Header("���a�w��")]
	[SerializeField] private float m_radius = 5.0f;
	[SerializeField] private bool m_isRadius = false;


	private void Start()
	{
		// �G�̐����X�N���v�g���擾
		m_enemyGenerator = GetComponent<EnemyGenerator>();
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		AttackOne(target.position, attackRank);
	}

	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		// �G�l�~�[�W�F�l���[�^���Ȃ�����ݒ肪�K�v
		if (m_enemyGenerator == null)
		{
			Debug.Log("DA_Enemy : m_enemyGenerator ��ݒ肵�Ă�");
			return;
		}

		// �^�C�v�Ɣ��a�����w��A��
		if (m_isType && m_isRadius)
		{
			m_enemyGenerator.Spawn(m_type, m_radius);
			return;
		}

		// �^�C�v�w��
		if (m_isType)
		{
			m_enemyGenerator.Spawn(m_type, target);
			return;
		}

		// ���a�w��
		if (m_isRadius)
		{
			m_enemyGenerator.Spawn(m_radius);
		}

		// �����_������
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
	public bool IsType
	{
		set { m_isType = value; }
	}
	public float Radius
	{
		set { m_radius = value; }
	}
	public bool IsRadius
	{
		set { m_isRadius = value; }
	}
}

