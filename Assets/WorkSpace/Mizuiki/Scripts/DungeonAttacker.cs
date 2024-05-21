using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttacker : MonoBehaviour
{
	// �U���p�^�[���N���X�ݒ�p
	[System.Serializable]
	public struct AttackPattern
	{
		public DungeonAttackData.AttackType type;
		public DungeonAttackBase attack;
	}

	[Header("�U�����")]
	[SerializeField] private bool m_active = false;
	[Header("�U�������N")]
	[SerializeField] private int m_attackRank = 0;
	[Header("�U�������N�̏��")]
	[SerializeField] private int m_attackRankLimit = 10;


	[Header("---------- �R�A�ƃ^�[�Q�b�g�̋��� ----------")]
	[Header("�R�A�̈ʒu")]
	[SerializeField] private Vector3 m_corePosition = Vector3.zero;
	[Header("�U���Ώ�")]
	[SerializeField] private Transform m_target = null;
	[Header("�J�n���̃R�A�ƃ^�[�Q�b�g�̋���")]
	[SerializeField] private float m_startCoreDistance = 0.0f;

	[Header("�����ɉ������U���i�K")]
	[SerializeField] private DungeonAttackData.AttackGrade m_attackGrade;
	[Header("���݂̍U���̃O���[�h(�m�F�p)")]
	[SerializeField] private float m_nowAttackGrade = 1.0f;


	[Header("---------- �U�����̂̏�� ----------")]
	[Header("�U���̏��")]
	[SerializeField] private DungeonAttackData m_attackData = null;
	[SerializeField] private bool m_useData = true;

	[Header("�U����~����")]
	[SerializeField] private float m_stayTime = 10.0f;
	private float m_stayTimer = 0.0f;
	[Header("�U����������")]
	[SerializeField] private float m_attackTime = 10.0f;
	private float m_attackTimer = 0.0f;

	[Header("�_���W�����̍U���p�^�[��")]
	[SerializeField] private AttackPattern[] m_attackPattern;
	private readonly Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = new();

	// �^�[�����̍U������
	private readonly DungeonAttackTurn m_turn = new();


	[Header("---------- �U���e�[�u�� ----------")]
	[Header("�U���e�[�u���̔���͈�")]
	[SerializeField] private float m_attackTableRange = 5.0f;

	[Header("�g�p�U���e�[�u����臒l(����)"), Range(0.0f, 1.0f), Tooltip("���̐��l���傫���ꍇ��[FillTable]�A�������ꍇ��[CavityTable]")]
	[SerializeField] private float m_thresholdValueRate = 0.5f;

	[Header("�_���W�����̍U����")]
	[SerializeField] private List<DungeonAttackData.AttackTable> m_attackTableList = new();
	private readonly Dictionary<DungeonAttackData.AttackTableType, DungeonAttackTable> m_attackTables = new();


	[Header("---------- �U����� ----------")]
	[Header("�����_���U��")]
	[SerializeField] private bool m_random = false;

	[Header("�g�p����U���e�[�u��")]
	[SerializeField] private DungeonAttackData.AttackTableType m_attackTableType;



	private void Start()
	{
		// �f�[�^�̐ݒ�
		SetAttackData();
		// �U���p�^�[��������
		for (int i = 0; i < m_attackPattern.Length; i++)
		{
			// �^�C�v�̎擾
			DungeonAttackData.AttackType type = m_attackPattern[i].type;
			// �㏑���h�~
			if (m_attacker.ContainsKey(type))
			{
				continue;
			}
			// �����ɒǉ�
			m_attacker[type] = m_attackPattern[i].attack;
		}

		// �U���e�[�u��������
		for (int i = 0; i < m_attackTableList.Count; i++)
		{
			// �^�C�v�̎擾
			DungeonAttackData.AttackTableType type = m_attackTableList[i].type;
			// �����̏㏑���h�~
			if (m_attackTables.ContainsKey(type))
			{
				continue;
			}
			// �V���ȃf�[�^���쐬
			DungeonAttackTable data = new()
			{
				PatternIndex = 0,				// �U���C���f�b�N�X�̏�����
				Table = m_attackTableList[i]	// �U���e�[�u���̐ݒ�
			};
			// �����ɒǉ�
			m_attackTables[type] = data;
		}

		// �^�[�������̏����ݒ�
		m_turn.Attacker = m_attacker;

		// ��~���Ԃ�������
		m_stayTimer = m_stayTime;

		//// ����Fill���g�p�U���e�[�u���Ƃ���
		//m_useAttackTable = m_attackTables[DungeonAttackData.AttackTableType.FILL].Table;

		// �J�n���̋������擾
		if (m_target)
		{
			m_startCoreDistance = Vector3.Distance(m_target.position, m_corePosition);
		}

		// distance �̏����Ƀ\�[�g
		m_attackGrade.attackGrade.Sort((lhs, rhs) => lhs.distance.CompareTo(rhs.distance));

	}

	private void Update()
	{
		// �U��������
		if (m_active)
		{
			// �U������
			Attack();
			// �U���^�[���̎��Ԍo��
			m_attackTimer -= Time.deltaTime;
			// �U���^�[���I��
			if (m_attackTimer <= 0.0f)
			{
				// �U���I��
				EndAttack();
			}
		}
		else
		{
			// ��~���Ԃ̌o��
			m_stayTimer -= Time.deltaTime;
			// ��~���Ԃ��߂���
			if (m_stayTimer <= 0.0f)
			{
				// �U���J�n
				BeginAttack();
			}
		}
	}

	// �R�A�̈ʒu
	public Vector3 CorePosition
	{
		set { m_corePosition = value; }
	}



	// ���ݒ�
	private void SetAttackData()
	{
		// �f�[�^���Ȃ�
		if (m_attackData == null)
			return;

		// �f�[�^���g��Ȃ�
		if (!m_useData)
			return;

		// ��~���Ԑݒ�
		m_stayTime = m_attackData.StayTime;
		// �U�����Ԑݒ�
		m_attackTime = m_attackData.AttackTime;
		// �����_���t���O�ݒ�
		m_random = m_attackData.IsRandom;
		// �_���W�����̍U����
		m_attackTableList = new(m_attackData.AttackTableList);
	}

	// �U��
	private void Attack()
	{
		// �U���p�^�[�����Ȃ���Ώ������Ȃ�
		if (m_attacker.Count == 0)
		{
			return;
		}
		m_turn.Attack(m_target, m_attackRank, GetAttackGrade());

	}

	// �R�A�ƃ^�[�Q�b�g�̋����ɉ������U���Ԋu�̎擾
	private float GetAttackGrade()
	{
		// �R�A�ƃ^�[�Q�b�g�̋���
		float distance = Vector3.Distance(m_target.position, m_corePosition);

		// �R�A�Ƃ̋����ɉ������U���i�K(0 ~ 1)
		float attackGradeNormal = Mathf.InverseLerp(m_attackGrade.attackMaxDistance, m_startCoreDistance, distance);

		// �����ɉ������i�K�o�[�W����
		float attackGradeRank = 1.0f;
		foreach (DungeonAttackData.AttackPower grade in m_attackGrade.attackGrade)
		{
			// ����i�K�����������߂�
			if (distance <= m_startCoreDistance * grade.distance)
			{
				attackGradeRank = 1.0f / grade.magnification;
				break;
			}
		}

		// �ŏI�I�ȍU�����Ԃ̊�����Ԃ�
		if (m_attackGrade.attackGradeStep)
		{
			m_nowAttackGrade = attackGradeRank;
			return Mathf.Lerp(m_attackGrade.attackGradeRange.min, m_attackGrade.attackGradeRange.max, attackGradeRank);
		}
		else
		{
			m_nowAttackGrade = attackGradeNormal;
			return Mathf.Lerp(m_attackGrade.attackGradeRange.min, m_attackGrade.attackGradeRange.max, attackGradeNormal);
		}
	}

	// �U���J�n
	private void BeginAttack()
	{
		// �U�����J�n����
		m_active = true;
		// �U���^�[���̎��Ԑݒ�
		m_attackTimer = m_attackTime;
		// �U���e�[�u���̌���
		DetermineAttackTable();
		// �U���e�[�u���擾
		DungeonAttackTable data = m_attackTables[m_attackTableType];
		// �U���p�^�[����ݒ肷��
		m_turn.AttackPattern = data.Table.pattern[data.PatternIndex];

		Debug.Log("�U���J�n : " + m_attackTableType);
	}

	// �U���I��
	private void EndAttack()
	{
		// �U�����~����
		m_active = false;
		// ��~���Ԃ̐ݒ�
		m_stayTimer = m_stayTime;
		// �����N�A�b�v
		m_attackRank++;
		// �U�������N������𒴂��Ȃ��悤�ɃN�����v
		m_attackRank = Mathf.Clamp(m_attackRank, 0, m_attackRankLimit);
		// ���̍U���^�C�v����
		NextType();

		Debug.Log("�U����~");
	}

	// ���̍U���^�C�v����
	private void NextType()
	{
		// �����_���U��
		if (m_random)
		{
			// ���̍U���̃C���f�b�N�X�������_���Ŏ擾
			m_attackTables[m_attackTableType].PatternIndex = Random.Range(0, m_attackTables[m_attackTableType].Table.pattern.Count);
			return;
		}
		else
		{
			// �C���f�b�N�X�̃C���N�������g
			m_attackTables[m_attackTableType].PatternIndex++;
			// �͈͊O�ɂȂ���
			if (m_attackTables[m_attackTableType].PatternIndex >= m_attackTables[m_attackTableType].Table.pattern.Count)
			{
				// 0 �ɖ߂�
				m_attackTables[m_attackTableType].PatternIndex = 0;
			}
		}
	}

	// �U���e�[�u�������肷��
	private void DetermineAttackTable()
	{
		// �^�[�Q�b�g���ӂ̃u���b�N���擾
		Collider2D[] blocks = Physics2D.OverlapCircleAll(m_target.position, m_attackTableRange, LayerMask.NameToLayer("Block"));

		// ����͈͂���臒l�ƂȂ�u���b�N�̌����v�Z����
		int thresholdValue = (int)(Mathf.Round(Mathf.PI * m_attackTableRange * m_attackTableRange) * m_thresholdValueRate);

		// �U���e�[�u����ݒ肷��
		if (blocks.Length > thresholdValue)
		{
			// ���肪�u���b�N�Ŗ��܂��Ă�Ƃ��̍U���e�[�u�����g�p
			m_attackTableType = DungeonAttackData.AttackTableType.FILL;
		}
		else
		{
			// ����Ƀu���b�N���Ȃ����̍U���e�[�u�����g�p
			m_attackTableType = DungeonAttackData.AttackTableType.CAVITY;
		}
	}


}