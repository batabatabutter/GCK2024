using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dri : MonoBehaviour
{
    //�h���
    GameObject m_dwellBlock;

    // Start is called before the first frame update
    void Start()
    {
        // �e�I�u�W�F�N�g���擾����
        GameObject parentObject = transform.parent.gameObject;

        // �e�I�u�W�F�N�g����X�N���v�g���擾����
        m_dwellBlock = parentObject.GetComponent<EnemyDwell>().DwellBlock;

    }

    // Update is called once per frame
    void Update()
    {
         // �q�I�u�W�F�N�g�̃��[�J����]�𖳌��ɂ���
         transform.localRotation = Quaternion.identity;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �u���b�N�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Block") && collision.gameObject != m_dwellBlock)
        {
            // �I�u�W�F�N�g��j�󂷂�
            DestroyDri();
            Debug.Log(collision.gameObject.transform.position);
        }

        // �v���C���[�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�Ƀ_���[�W��^����
            collision.gameObject.GetComponent<Player>().AddDamage(1);

            // �I�u�W�F�N�g��j�󂷂�
            DestroyDri();
        }

    }


    public void DestroyDri()
    {
        Destroy(gameObject);
    }
}
