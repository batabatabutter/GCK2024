using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBomb : Block
{
    [System.Serializable]
    public enum BombState
    {
        STAY,       // �ҋ@
        DETONATE,   // �N��
        EXPLOSION,  // ���j
        DESTROY,    // �j��

        OVER,
    }

    [Header("---------- ���e ----------")]

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
    [Header("�������j�_���[�W")]
    [SerializeField] private float m_immediateDamage = 1000.0f;

    [Header("�u���b�N�j�󎞂̃A�C�e���h���b�v�{��")]
    [SerializeField] private int m_itemDropRate = 0;

    [Header("���j���")]
    [SerializeField] private BombState m_state = BombState.STAY;
    //[SerializeField] private bool m_detonate = false;

    [Header("����������p�̃u���b�N")]
    [SerializeField] private GameObject m_detonateBlock = null;

    [Header("��������")]
    [SerializeField] private AudioClip m_bombSE = null;

    //// ���j�\��
    //private bool m_canExplosion = false;
    //// �������Ă��邩
    //private bool m_isExplosion = false;


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
        // ���j���
        switch (m_state)
        {
            case BombState.STAY:    // �ҋ@��

                break;

            case BombState.DETONATE:    // �N����
                // �J�E���g�_�E��
                m_timeToExplosion -= Time.deltaTime;
                // ���Ԍo�߂Ŕ��j��ԂɑJ��
                if (m_timeToExplosion < 0)
                {
                    m_state = BombState.EXPLOSION;
                }

                break;

            case BombState.EXPLOSION:   // ���j
				// �����ɕK�v�ȃu���b�N�̐���
				if (m_detonateBlock)
				{
					// �N���u���b�N�̐���
					Instantiate(m_detonateBlock, transform.position, Quaternion.identity);

                    //  ������
                    AudioManager.Instance.PlaySE(m_bombSE, transform.position);

					// ��񐶐������� null �ɂ���
					m_detonateBlock = null;
				}
                else
                {
                    break;
                }

                // BoxCollider2D ���폜
				Destroy(GetComponent<BoxCollider2D>());

				// CircleCollider ������
				CircleCollider2D circle = gameObject.AddComponent<CircleCollider2D>();
				// ���j�͈͂̐ݒ�
				circle.radius = m_explosionRange;
				// �g���K�[�ɂ���
				circle.isTrigger = true;

				// ���C���[�� Bomb �ɂ���
				gameObject.layer = LayerMask.NameToLayer("Bomb");

				break;

            case BombState.DESTROY:     // �j��
				// �������g��j�󂷂�
				Destroy(gameObject);

				break;
        }

    }

    // �N������
    public void Detonate()
    {
        // �N���O
        if (m_state == BombState.STAY)
        {
			// ���j��Ԃɂ���
			m_state = BombState.DETONATE;

			// �X�v���C�g�̐F��ς���
			GetComponent<SpriteRenderer>().color = Color.red;
		}
	}


	// ���j�_���[�W��^����
	private void OnTriggerEnter2D(Collider2D collision)
	{
        // ���j��Ԃł͂Ȃ�
        if (m_state < BombState.EXPLOSION)
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
                    block.AddMiningDamage(m_blockExplosionPower, m_itemDropRate);
                }
                // �j�����
                else
                {
                    block.BrokenBlock();
                }
            }

            // �j���Ԃɂ���
            m_state = BombState.DESTROY;
        }
        // �^�O���v���C���[
        else if (collision.CompareTag("Player"))
        {
            // �v���C���[�X�N���v�g�擾
            if (collision.TryGetComponent(out Player player))
            {
                player.AddDamage(m_playerExplosionDamage);
            }

			// �j���Ԃɂ���
			m_state = BombState.DESTROY;
		}

	}

    // �_���[�W���󂯂�
    public override bool AddMiningDamage(float power, int dropCount = 1)
    {
        // �N��
        Detonate();

        return true;
    }
	// �j�󂵂悤�Ƃ���Ă���
	public override bool BrokenBlock(int dropCount = 1)
	{
        // �N��
        Detonate();

        return true;
	}

}
