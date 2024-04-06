using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dri : MonoBehaviour
{
    //�傫��
    float scale = 0.0f;
    //�h���
    GameObject m_dwellBlock;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.9f,0,1);

        transform.localPosition  = new Vector3(0,0.5f,0);

        // �e�I�u�W�F�N�g���擾����
        GameObject parentObject = transform.parent.gameObject;

        // �e�I�u�W�F�N�g����X�N���v�g���擾����
        m_dwellBlock = parentObject.GetComponent<EnemyDwell>().DwellBlock;
    }

    // Update is called once per frame
    void Update()
    {
        if(scale > 2.0f)
        {
            Destroy(gameObject);

        }
        else
        {
            scale += Time.deltaTime;


            transform.localScale = new Vector3(0.9f, scale, 1);

            transform.localPosition = new Vector3(0, scale / 4 + 0.5f , 0);

            // �q�I�u�W�F�N�g�̃��[�J����]�𖳌��ɂ���
            transform.localRotation = Quaternion.identity;


        }
    }
private void OnCollisionEnter2D(Collision2D collision)
    {
        // �u���b�N�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Block") && collision.gameObject != m_dwellBlock)
        {
            // �I�u�W�F�N�g��j�󂷂�
            Destroy(gameObject);
        }

        // �v���C���[�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�Ƀ_���[�W��^����
            collision.gameObject.GetComponent<Player>().AddDamage(1);

            // �I�u�W�F�N�g��j�󂷂�
            Destroy(gameObject);
        }
    }
}
