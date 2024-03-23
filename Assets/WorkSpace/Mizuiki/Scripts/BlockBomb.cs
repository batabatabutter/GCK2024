using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBomb : Block
{
    [Header("�u���b�N�ւ̔��j�_���[�W")]
    [SerializeField] private float m_blockExplosionPower = 1.0f;
    [Header("�v���C���[�ւ̔��j�_���[�W")]
    [SerializeField] private int m_playerExplosionDamage = 1;

    [Header("���j�͈�(���a)")]
    [SerializeField] private float m_explosionRange = 2.0f;

    [Header("�����܂ł̎���")]
    [SerializeField] private float m_timeToExplosion = 3.0f;

    [Header("�j��̕���(�_���[�W����/�m��j��)")]
    [SerializeField] private bool m_damage = true;

    [Header("����������p�̃u���b�N")]
    [SerializeField] private GameObject m_detonateBlock = null;

    // ���j�\��
    private bool m_canExplosion = false;
    // �������Ă��邩
    private bool m_isExplosion = false;


    // Start is called before the first frame update
    void Start()
    {
        // ���W�b�h�{�f�B�����ĂȂ���΂���
        if (!GetComponent<Rigidbody2D>())
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            // �d�͂͑��݂��Ȃ�
            rigidbody.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ������
        if (m_isExplosion)
        {
            // �������g��j�󂷂�
            Destroy(gameObject);
        }
        // �J�E���g�_�E��
        else if (m_timeToExplosion > 0.0f)
        {
            m_timeToExplosion -= Time.deltaTime;
        }
		// ���Ԃ�0�ȉ� �Ŕ����O
		else if (!m_isExplosion)
        {
            // �N��
            Detonate();

        }

    }

    // �N������
    public void Detonate()
    {
		// BoxCollider2D ���폜
		Destroy(GetComponent<BoxCollider2D>());

		// CircleCollider ������
		CircleCollider2D circle = gameObject.AddComponent<CircleCollider2D>();
		// ���j�͈͂̐ݒ�
		circle.radius = m_explosionRange;
		// �g���K�[�ɂ���
		circle.isTrigger = true;

		// ���C���[�� Block �ȊO�ɂ���
		gameObject.layer = 0;

        // �����ɕK�v�ȃu���b�N�̐���
        if (m_detonateBlock)
        {
            // �N���u���b�N�̐���
            Instantiate(m_detonateBlock, transform.position, Quaternion.identity);

            // ��񐶐������� null �ɂ���
            m_detonateBlock = null;
        }

        // ���j�\
		m_canExplosion = true;
    }


	// ���j�_���[�W��^����
	private void OnTriggerStay2D(Collider2D collision)
	{
        // ���j�s�\
        if (!m_canExplosion)
            return;

        // �^�O�����C���[���u���b�N
        if (collision.CompareTag("Block") || collision.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            // �u���b�N�X�N���v�g�擾
            if (collision.TryGetComponent(out Block block))
            {
                // �_���[�W����
                if (m_damage)
                {
                    block.AddMiningDamage(m_blockExplosionPower);
                }
                // �j�����
                else
                {
                    block.BrokenBlock();
                }
            }

            // ������Ԃɂ���
            m_isExplosion = true;

            return;
        }

        // �^�O���v���C���[
        if (collision.CompareTag("Player"))
        {
            // �v���C���[�X�N���v�g�擾
            if (collision.TryGetComponent(out Player player))
            {
                player.AddDamage(m_playerExplosionDamage);
            }

            // ������Ԃɂ���
            m_isExplosion = true;

        }

	}

    // �_���[�W���󂯂�
    public override void AddMiningDamage(float power)
    {
        // �N��
        Detonate();

    }

}
