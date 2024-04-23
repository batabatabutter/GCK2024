using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blast : Enemy_AttackBall
{
    [Header("�u���b�N�ւ̃_���[�W")]
    [SerializeField] int m_addBlockDamege = 500;
    [Header("�v���C���[�ւ̃_���[�W")]
    [SerializeField] int m_playerDamege = 1;

    // Rigidbody2D�R���|�[�l���g���A�^�b�`����I�u�W�F�N�g
    private Rigidbody2D rb;


    protected override void Start()
    {
        base.Start();

        transform.parent = null;

        // Rigidbody2D�R���|�[�l���g���擾����
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // �^�O�����C���[���u���b�N
        if (collision.CompareTag("Block") || collision.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            // ���������I�u�W�F�N�g���� Block �X�N���v�g���擾
            Block[] blocks = collision.GetComponents<Block>();

            // �擾�����X�N���v�g�����[�v�ŏ���
            foreach (Block block in blocks)
            {
                // �X�N���v�g�����݂���ꍇ�̏���
                if (block != null && !block.IsDestroyed() && block)
                {
                    block.AddMiningDamage(m_addBlockDamege);
                }
            }
        }
        //�v���C���[�Ƀ_���[�W
        if(collision.CompareTag("Player"))
        {
            // �v���C���[�X�N���v�g�擾
            if (collision.TryGetComponent(out Player player))
            {
                player.AddDamage(m_playerDamege);
            }
        }
        //�h���͎���
        if (m_dwellBlock.TryGetComponent(out Block dwellblock))
        {
            dwellblock.BrokenBlock();
        }

        //������
        DestroyThis();

    }

    public override void MoveStart()
    {

    }
}
