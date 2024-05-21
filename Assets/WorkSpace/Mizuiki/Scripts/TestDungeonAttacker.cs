using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeonAttacker : MonoBehaviour
{
    // �U���p�^�[���N���X�ݒ�p
    [System.Serializable]
    public struct AttackPattern
    {
        public DungeonAttackData.AttackType type;
        public DungeonAttackBase attack;
	}
    // �����ɉ������U���i�K
    [System.Serializable, Tooltip("distance �����Ƀ\�[�g")]
    public struct AttackGrade
    {
        [Tooltip("�J�n�n�_�� distance �{�̂Ƃ�")]
        public float distance;
        [Tooltip("�U���̔����{��")]
        public float grade;
    }

    [Header("�U�����")]
    [SerializeField] private bool m_active = false;
    [Header("�U�������N")]
    [SerializeField] private int m_attackRank = 0;
    [Header("�U�������N�̏��")]
    [SerializeField] private int m_attackRankLimit = 10;

    [Header("�R�A�̈ʒu")]
    [SerializeField] private Vector3 m_corePosition = Vector3.zero;
    [Header("�U���Ώ�")]
    [SerializeField] private Transform m_target = null;

    [Header("---------- �R�A�ƃ^�[�Q�b�g�̋��� ----------")]
    [Header("�U���i�K�͈̔�")]
    [SerializeField] private MyFunction.MinMaxFloat m_attackGradeRange;
    [Header("�U�����ő�ɂȂ鋗��")]
    [SerializeField] private float m_attackMaxDistance = 0.0f;
    // �J�n���̃^�[�Q�b�g�ƃR�A�̋���
    private float m_coreDistance = 0.0f;
    [Header("�R�A�Ƃ̋����̎����̎��")]
    [SerializeField] private bool m_attackGradeStep = false;
    [Header("�����ɉ������U���i�K")]
    [SerializeField] private List<AttackGrade> m_attackGrade;

    [Header("���݂̍U���̃O���[�h(�m�F�p)")]
    [SerializeField] private float m_nowAttackGrade = 1.0f;

    [Header("---------- �U�����̂̏�� ----------")]
    [Header("�U���̃N�[���^�C��(�m�F�p)")]
    [SerializeField] private float m_attackCoolTime = 0.0f;

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
    private DungeonAttackTurn m_turn = new();

    [Header("---------- �U����� ----------")]
    [Header("�����_���U��")]
    [SerializeField] private bool m_random = false;

    [Header("�g�p����U���e�[�u��")]
    [SerializeField] private DungeonAttackData.AttackTableType m_attackTablePattern;
    [Header("�U���p�^�[���̃C���f�b�N�X")]
    [SerializeField] private readonly Dictionary<DungeonAttackData.AttackTableType, int> m_attackPatternIndex = new();

    [Header("---------- �U���e�[�u�� ----------")]
    [Header("�U���e�[�u���̔���͈�")]
    [SerializeField] private float m_attackTableRange = 5.0f;

    [Header("�g�p�U���e�[�u����臒l(����)"), Range(0.0f, 1.0f), Tooltip("���̐��l���傫���ꍇ��[FillTable]�A�������ꍇ��[CavityTable]")]
    [SerializeField] private float m_thresholdValueRate = 0.5f;

    [Header("�_���W�����̍U����")]
	[SerializeField] private List<DungeonAttackData.AttackTable> m_attackTableList = new();

	// �g�p����U���e�[�u��
	private DungeonAttackData.AttackTable m_attackTable = new();

    // �U���̎��
	private DungeonAttackData.AttackType m_type;


	// Start is called before the first frame update
	void Start()
    {
        // �U���p�^�[���̏�����
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

        // �f�[�^�̐ݒ�
        SetAttackData();

        // �^�[�������̏����ݒ�
        m_turn.Attacker = m_attacker;

        // �N�[���^�C���̐ݒ�
        //SetCoolTime();

        // ��~���Ԃ�������
        m_stayTimer = m_stayTime;

        // ����Fill���g�p�U���e�[�u���Ƃ���
        m_attackTable = m_attackTableList[0];
        // �U���^�C�v�̏�����
        m_type = m_attackTable.pattern[0].AttackList[0].type;

        // �U���p�^�[���̃C���f�b�N�X������
        for (DungeonAttackData.AttackTableType i = 0; i < DungeonAttackData.AttackTableType.OVER; i++)
        {
            m_attackPatternIndex.Add(i, 0);
        }

		// �J�n���̋������擾
		if (m_target)
        {
            m_coreDistance = Vector3.Distance(m_target.position, m_corePosition);
        }

        // distance �̏����Ƀ\�[�g
        m_attackGrade.Sort((lhs, rhs) => lhs.distance.CompareTo(rhs.distance));

    }

    // Update is called once per frame
    void Update()
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




    // �U��
    private void Attack()
    {
		// �U���p�^�[�����Ȃ���Ώ������Ȃ�
		if (m_attacker.Count == 0)
		{
			return;
		}
        m_turn.Attack(m_target, m_attackRank, GetAttackGrade());
		//// ���Ԍo��
		//m_attackCoolTime -= Time.deltaTime;
		//// �U������
		//if (m_attackCoolTime <= 0.0f)
		//{
  //          // �U���p�^�[���̏��擾
  //          DungeonAttackData.AttackList pattern = m_attackTable[m_attackPatternIndex[m_attackTablePattern]].attackList;
		//	// �w��^�C�v�̍U������
		//	m_attacker[m_type].Attack(m_target, pattern., m_attackRank);
  //          // �N�[���^�C���v�Z
		//	m_attackCoolTime = m_attacker[m_type].AttackTime * GetAttackGrade();
		//}

	}

    // �U���J�n
    private void BeginAttack()
    {
		// �U�����J�n����
		m_active = true;
		// �U���^�[���̎��Ԑݒ�
		m_attackTimer = m_attackTime;
		// ���̍U���^�C�v����
		NextType();
		// �U���^�C�v���ݒ肳��Ă��Ȃ���Ώ������Ȃ�
		if (!m_attacker.ContainsKey(m_type))
		{
			Debug.Log(m_type.ToString() + " : �U���p�^�[�����ݒ肳��ĂȂ���");
			m_active = false;
			return;
		}
		Debug.Log("�U���J�n : " + m_type.ToString());
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
		Debug.Log("�U����~");
	}

	// �R�A�ƃ^�[�Q�b�g�̋����ɉ������U���Ԋu�̎擾
	private float GetAttackGrade()
    {
		// �R�A�ƃ^�[�Q�b�g�̋���
		float distance = Vector3.Distance(m_target.position, m_corePosition);

		// �R�A�Ƃ̋����ɉ������U���i�K(0 ~ 1)
		float attackGradeNormal = Mathf.InverseLerp(m_attackMaxDistance, m_coreDistance, distance);

        // �����ɉ������i�K�o�[�W����
        float attackGradeRank = 1.0f;
        foreach (AttackGrade grade in m_attackGrade)
        {
            // ����i�K�����������߂�
            if (distance <= m_coreDistance * grade.distance)
            {
                attackGradeRank = grade.grade;
                break;
            }
        }

        // �ŏI�I�ȍU�����Ԃ̊�����Ԃ�
        if (m_attackGradeStep)
        {
            m_nowAttackGrade = attackGradeRank;
            return Mathf.Lerp(m_attackGradeRange.min, m_attackGradeRange.max, attackGradeRank);
        }
        else
        {
            m_nowAttackGrade = attackGradeNormal;
            return Mathf.Lerp(m_attackGradeRange.min, m_attackGradeRange.max, attackGradeNormal);
        }
	}

	// ���̍U���^�C�v����
	private void NextType()
    {
        // �U���e�[�u���̌���
        DetermineAttackTable();

        // �����_���U��
        if (m_random)
        {
            // ���̍U���̃C���f�b�N�X�������_���Ŏ擾
            m_attackPatternIndex[m_attackTablePattern] = Random.Range(0, m_attackTable.pattern.Count);
            return;
        }
        else
        {
            // �C���f�b�N�X�̃C���N�������g
            m_attackPatternIndex[m_attackTablePattern]++;
            // �͈͊O�ɂȂ���
            if (m_attackPatternIndex[m_attackTablePattern] >= m_attackTable.pattern.Count)
            {
                // 0 �ɖ߂�
                m_attackPatternIndex[m_attackTablePattern] = 0;
            }
        }

        // �^�C�v�̐ݒ�
		//m_type = m_attackTable[m_attackPatternIndex[m_attackTablePattern]].attackList[0].type;
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
            m_attackTable = m_attackTableList[0];
        }
        else
        {
            // ����Ƀu���b�N���Ȃ����̍U���e�[�u�����g�p
            m_attackTable = m_attackTableList[1];
        }
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
        m_attackTableList = new (m_attackData.AttackTableList);
    }

    //private void SetCoolTime()
    //{
    //    foreach (DungeonAttackData.AttackPattern data in m_attackTable)
    //    {
    //        // �U���^�C�v
    //        DungeonAttackData.AttackType type = data.type;

    //        // �L�[���ݒ肵�Ă���
    //        if (m_attacker.ContainsKey(type))
    //        {
    //            // �U�����Ԃ̐ݒ�
    //            m_attacker[type].AttackTime = data.[0].time;
    //            // �U���͈͂̐ݒ�
    //            m_attacker[type].SetAttackRange(data.attackList[0].range);
    //            // �����N�����l�̐ݒ�
    //            m_attacker[type].SetRankValue(data.rankValue);
    //        }
    //    }
    //}

}
