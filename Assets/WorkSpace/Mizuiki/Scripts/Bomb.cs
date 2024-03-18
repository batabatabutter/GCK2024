using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("���j�_���[�W")]
    [SerializeField] private float m_explosionPower = 1.0f;

    [Header("���j�͈�(���a)")]
    [SerializeField] private float m_explosionRange = 2.0f;

    [Header("�����܂ł̎���")]
    [SerializeField] private float m_timeToExplosion = 3.0f;

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
        // �J�E���g�_�E��
        if (m_timeToExplosion > 0.0f)
        {
            m_timeToExplosion -= Time.deltaTime;
        }
        // ���Ԃ�0�ȉ� �Ŕ����O
        else if (!m_isExplosion)
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

            // ������Ԃɂ���
            m_isExplosion = true;

        }
        // ������
        else if (m_isExplosion)
        {
            // �������g��j�󂷂�
            Destroy(gameObject);
        }

    }


	// ���j�_���[�W��^����
	private void OnTriggerStay2D(Collider2D collision)
	{
        // �u���b�N�^�O���t���Ă��Ȃ�
        if (!collision.CompareTag("Block"))
            return;

        // �u���b�N�X�N���v�g�擾
        if (collision.TryGetComponent(out Block block))
        {
            block.AddMiningDamage(m_explosionPower);
        }

		
	}

}
