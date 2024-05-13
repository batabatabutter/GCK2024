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

    [Header("�U�����")]
    [SerializeField] private bool m_active = false;
    [Header("�U�������N")]
    [SerializeField] private int m_attackRank = 0;

    [Header("�U���Ώ�")]
    [SerializeField] private Transform m_target = null;

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

    [Header("�����_���U��")]
    [SerializeField] private bool m_random = false;

    [Header("�U���p�^�[���̃C���f�b�N�X")]
    [SerializeField] private int m_attackPatternIndex = 0;

    [Header("�_���W�����̍U���p�^�[��")]
    [SerializeField] private AttackPattern[] m_attackPattern;
    private readonly Dictionary<DungeonAttackData.AttackType, DungeonAttackBase> m_attacker = new();

    [Header("�_���W�����̍U����")]
    [SerializeField] private List<DungeonAttackData.AttackData> m_attackOrder = new();

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

        // �N�[���^�C���̐ݒ�
        SetCoolTime();

        // ��~���Ԃ�������
        m_stayTimer = m_stayTime;

        // �U���^�C�v�̏�����
        m_type = m_attackOrder[0].type;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_active)
        {
            Attack();
            // �U�����Ԃ̌o��
            m_attackTimer -= Time.deltaTime;
			// �U�����Ԃ��߂���
			if (m_attackTimer <= 0.0f)
			{
				// �U�����~����
				m_active = false;
				m_stayTimer = m_stayTime;
                // �����N�A�b�v
                m_attackRank++;
				// ���̍U���^�C�v����
				NextType();
				Debug.Log("�U����~");
			}
		}
		else
        {
            // ��~���Ԃ̌o��
            m_stayTimer -= Time.deltaTime;
			// ��~���Ԃ��߂���
			if (m_stayTimer <= 0.0f)
			{
				// �U�����J�n����
				m_active = true;
				m_attackTimer = m_attackTime;
				// �U���p�^�[�����ݒ肳��Ă��Ȃ���Ώ������Ȃ�
				if (!m_attacker.ContainsKey(m_type))
				{
					Debug.Log(m_type.ToString() + " : �U���p�^�[�����ݒ肳��ĂȂ���");
                    m_active = false;
					return;
				}
				Debug.Log("�U���J�n : " + m_type.ToString());
			}
		}
	}


    private void Attack()
    {
		// �U���p�^�[�����Ȃ���Ώ������Ȃ�
		if (m_attacker.Count == 0)
		{
			return;
		}
		// ���Ԍo��
		m_attackCoolTime -= Time.deltaTime;
		// �U������
		if (m_attackCoolTime <= 0.0f)
		{
			m_attacker[m_type].Attack(m_target, m_attackRank);
			m_attackCoolTime = m_attacker[m_type].AttackTime;
		}

	}

    // ���̍U���^�C�v����
    private void NextType()
    {
        // �����_���U��
        if (m_random)
        {
            // ���̍U���̃C���f�b�N�X�������_���Ŏ擾
            m_attackPatternIndex = Random.Range(0, m_attackOrder.Count);
            return;
        }
        else
        {
            // �C���f�b�N�X�̃C���N�������g
            m_attackPatternIndex++;
            // �͈͊O�ɂȂ���
            if (m_attackPatternIndex >= m_attackOrder.Count)
            {
                // 0 �ɖ߂�
                m_attackPatternIndex = 0;
            }
        }

        // �^�C�v�̐ݒ�
		m_type = m_attackOrder[m_attackPatternIndex].type;
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
        m_attackOrder = new (m_attackData.AttackPattern);
    }

    private void SetCoolTime()
    {
        foreach (DungeonAttackData.AttackData data in m_attackOrder)
        {
            // �U���^�C�v
            DungeonAttackData.AttackType type = data.type;

            // �L�[���ݒ肵�Ă���
            if (m_attacker.ContainsKey(type))
            {
                // �U�����Ԃ̐ݒ�
                m_attacker[type].AttackTime = data.coolTime;
                // �U���͈͂̐ݒ�
                m_attacker[type].SetAttackRange(data.range);
                // �����N�����l�̐ݒ�
                m_attacker[type].SetRankValue(data.rankValue);
            }
        }
    }

}
