using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_GrowUpRock03 : DA_GrowUpRock
{
    [Header("---------- ���肭��� ----------")]
    [Header("��𐶂₷�N���X")]
    [SerializeField] private DA_GrowUpRock02 m_growUpRock02;

    [Header("�U���t���O")]
    [SerializeField] private bool m_attack = false;

    [Header("���肭�鎞��")]
    [SerializeField] private float m_loomingTime = 0.5f;
    private float m_loomingTimer = 0.0f;

    [Header("���Ԑ�")]
    [SerializeField] private int m_range = 5;

    [Header("�����N")]
    [SerializeField] private int m_rank = 0;
    [Header("�����N�ɉ�����������")]
    [SerializeField] private float m_rankValue = 1.0f;

    [Header("���肭�����")]
    [SerializeField] private MyFunction.Direction m_direction;

    [Header("�^�[�Q�b�g�̈ʒu")]
    [SerializeField] private Vector3 m_targetPosition = Vector3.zero;
    [Header("�^�[�Q�b�g����̋���")]
    [SerializeField] private float m_distance = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (m_growUpRock02 == null)
        {
			// ��𐶂₻��
			m_growUpRock02 = GetComponent<DA_GrowUpRock02>();
		}
	}

    // Update is called once per frame
    void Update()
    {
        // �U�����Ȃ�
        if (!m_attack)
            return;

        // �^�[�Q�b�g�ɏ\���߂Â���
        if (m_distance < 0.0f)
        {
            // �U���t���O�I�t
            m_attack = false;
            return;
        }

        // �^�C�}�[�ғ���
        if (m_loomingTimer > 0.0f)
        {
			// ���Ԍo��
			m_loomingTimer -= Time.deltaTime;

            return;
		}

        // �^�C�}�[�[��
        // �U������
        m_growUpRock02.AttackLump(m_targetPosition, m_direction, m_range, m_distance, m_rankValue, m_rank);
        // �^�C�}�[�ݒ�
        m_loomingTimer = m_loomingTime;
        // �������߂Â���
        m_distance--;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
        // �^�[�Q�b�g�̈ʒu�擾
        m_targetPosition = target.position;
        // ���肭������擾
        m_direction = MyFunction.GetDirection(direction);
        // ���Ԑ��擾
        m_range = (int)range;
        // �^�[�Q�b�g����̋����擾
        m_distance = distance;
        // �����N�␳�l
        m_rankValue = rankValue;
        // �����N
        m_rank = attackRank;

        // �ꔭ�ڂ̍U��
        m_growUpRock02.Attack(target, m_direction, m_range, m_distance, rankValue, attackRank);

        // �U���t���O�I��
        m_attack = true;
        // �^�C�}�[�ݒ�
        m_loomingTimer = m_loomingTime;
        // �������߂Â���
        m_distance--;
	}
}
