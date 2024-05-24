using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackBall : MonoBehaviour
{
    //�h���
    protected GameObject m_dwellBlock;

    [Header("�u���b�N�Ƀ_���[�W��^����U����")]
    [SerializeField] int m_blockDamage = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // �e�I�u�W�F�N�g���擾����
        GameObject parentObject = transform.parent.gameObject;

        // �e�I�u�W�F�N�g����X�N���v�g���擾����
        m_dwellBlock = parentObject.GetComponent<EnemyDwell>().DwellBlock;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // �q�I�u�W�F�N�g�̃��[�J����]�𖳌��ɂ���
        transform.localRotation = Quaternion.identity;

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // �u���b�N�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Block") && collision.gameObject != m_dwellBlock)
        {
            //�u���b�N�Ƀ_���[�W��^����
            collision.gameObject.GetComponent<Block>().AddMiningDamage(m_blockDamage);

            // �I�u�W�F�N�g��j�󂷂�
            DestroyThis();
        }

        // �v���C���[�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�Ƀ_���[�W��^����
            collision.gameObject.GetComponent<Player>().AddDamage(1);

            // �I�u�W�F�N�g��j�󂷂�
            DestroyThis();
        }

    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }


    public virtual void MoveStart()
    { 
    }
}
