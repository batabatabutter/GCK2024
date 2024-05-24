using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonAttacker : MonoBehaviour
{
	// �U���p�^�[���N���X�ݒ�p
	[System.Serializable]
	public struct AttackPattern
	{
		public DungeonAttackData.AttackType type;
		public DungeonAttackBase attack;
	}

	[Header("---------- �U����� ----------")]
	[Header("�U�����")]
	[SerializeField] private bool m_active = false;
	[Header("�g�p����U���e�[�u��")]
	[SerializeField] private DungeonAttackData.AttackTableType m_attackTableType;
	[Header("�U�������N")]
	[SerializeField] private int m_attackRank = 0;

	[Header("---------- �R�A�ƃ^�[�Q�b�g�̋��� ----------")]
	[Header("�R�A�̈ʒu")]
	[SerializeField] private Vector3 m_corePosition = Vector3.zero;
	[Header("�U���Ώ�")]
	[SerializeField] private Transform m_target = null;
	[Header("�J�n���̃R�A�ƃ^�[�Q�b�g�̋���")]
	[SerializeField] private float m_startCoreDistance = 0.0f;

	[Header("---------- �f�[�^�ݒ肷����̂��� ----------")]
	[Header("�U���̏��")]
	[SerializeField] private DungeonAttackData m_attackData = null;

	// �����_���U��
	private bool m_random = false;
	// �U�������N�̏��
	private int m_attackRankLimit = 10;

	// �����ɉ������U���i�K
	private DungeonAttackData.AttackGrade m_attackGrade;

	// �U����~����
	private float m_stayTime = 10.0f;
	private float m_stayTimer = 0.0f;
	// �U����������
	private float m_attackTime = 10.0f;
	private float m_attackTimer = 0.0f;

	// �U���e�[�u���̔���͈�
	private float m_attackTableRange = 5.0f;

	// �g�p�U���e�[�u����臒l(����)
	//private float m_thresholdValueRate = 0.5f;

	// �_���W�����̍U����
	private List<DungeonAttackData.AttackTable> m_attackTableList = new();
	private readonly Dictionary<DungeonAttackData.AttackTableType, DungeonAttackTable> m_attackTables = new();

	// �^�[�����̍U������
	private readonly DungeonAttackTurn m_turn = new();


	[Header("---------- �C���X�y�N�^�[�Őݒ�K�{ ----------")]
	[Header("�_���W�����̍U���p�^�[��")]
	[SerializeField] private AttackPattern[] m_attackPattern;
	private readonly Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = new();

	[Header("========== �f�o�b�O�p ==========")]
	[Header("[P] �Ŕ���")]
	[SerializeField] private DungeonAttackData.AttackType m_attackerType;


	private void Start()
	{
		// �e�������Ă���
		GameObject parent = transform.parent.gameObject;
		if (parent)
		{
			// �e����v���C�V�[���}�l�[�W�����擾
			if (parent.TryGetComponent(out PlaySceneManager scene))
			{
				// �v���C���[���擾
				m_target = scene.GetPlayer().transform;
				// �R�A�̈ʒu���擾
				m_corePosition = scene.GetCore().transform.position;
			}
			// �e����_���W�����W�F�l���[�^���擾
			if (parent.TryGetComponent(out DungeonGenerator generator))
			{
				m_attackData = generator.GetDungeonData().AttackData;
			}
		}

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



		// �������p
		if (Input.GetKeyDown(KeyCode.P))
		{
			m_attacker[m_attackerType].Attack(m_target, MyFunction.Direction.RANDOM, 5.0f, 0, m_attackRank);
		}
	}

	// �U���̏��
	public bool Active
	{
		get { return m_active; }
	}
	// �R�A�̈ʒu
	public Vector3 CorePosition
	{
		set { m_corePosition = value; }
	}
	// �^�[�Q�b�g�̃g�����X�t�H�[��
	public Transform Target
	{
		set { m_target = value; }
	}
	// �U���̏��
	public DungeonAttackData AttackData
	{
		set { m_attackData = value; }
	}



	// ���ݒ�
	private void SetAttackData()
	{
		// �f�[�^���Ȃ�
		if (m_attackData == null)
		{
			Debug.Log("�U���f�[�^���Ȃ���");
			return;
		}

		// ��~���Ԑݒ�
		m_stayTime = m_attackData.StayTime;
		// �U�����Ԑݒ�
		m_attackTime = m_attackData.AttackTime;
		// �����N���
		m_attackRankLimit = m_attackData.RankLimit;
		// �����_���t���O�ݒ�
		m_random = m_attackData.IsRandom;
		// �_���W�����̍U����
		m_attackTableList = new(m_attackData.AttackTableList);
		// �e�[�u������̔��f������͈�
		m_attackTableRange = m_attackData.AttackTableRange;
		// �g�p�e�[�u���𕪂�臒l
		//m_thresholdValueRate = m_attackData.ThresholdValueRate;
		// �U���i�K�̐ݒ�
		m_attackGrade = m_attackData.AttackGradeData;
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
			return Mathf.Lerp(m_attackGrade.attackGradeRange.min, m_attackGrade.attackGradeRange.max, attackGradeRank);
		}
		else
		{
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

		// ����͈͂̃u���b�N�̊������擾
		float blockRate = blocks.Length / (int)Mathf.Round(Mathf.PI * m_attackTableRange * m_attackTableRange);

		foreach (DungeonAttackData.AttackTable attackTable in m_attackTableList)
		{
			// �u���b�N�����߂�ꂽ�����ȏ�
			if (attackTable.overNum < blockRate)
			{
				m_attackTableType = attackTable.type;
				return;
			}
		}

		m_attackTableType = DungeonAttackData.AttackTableType.CAVITY;

		//// ����͈͂���臒l�ƂȂ�u���b�N�̌����v�Z����
		//int thresholdValue = (int)(Mathf.Round(Mathf.PI * m_attackTableRange * m_attackTableRange) * m_thresholdValueRate);

		//// �U���e�[�u����ݒ肷��
		//if (blocks.Length > thresholdValue)
		//{
		//	// ���肪�u���b�N�Ŗ��܂��Ă�Ƃ��̍U���e�[�u�����g�p
		//	m_attackTableType = DungeonAttackData.AttackTableType.FILL;
		//}
		//else
		//{
		//	// ����Ƀu���b�N���Ȃ����̍U���e�[�u�����g�p
		//	m_attackTableType = DungeonAttackData.AttackTableType.CAVITY;
		//}
	}


}