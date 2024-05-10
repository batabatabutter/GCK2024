using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static TestDungeonGenerator;
/// <summary>
/// enemy��e�ɂ��������^�̓G
/// </summary>

public class EnemyMob : Enemy
{
    //�}�b�v�󋵂̓ǂݍ���
    const float RELOAD_TIME = 5.0f;

    protected List<Vector2Int> m_roadRoute;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //�v���C���[�������Ă��Ȃ��ꍇ�͏��������Ȃ�
        //if (base.Player == null)
        //    return;



    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && base.Player == null)
        {
            base.Player = collision.gameObject;

            // �Q�[���I�u�W�F�N�g���� Collider �R���|�[�l���g���폜����
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                Destroy(collider);
            }

            // �Q�[���I�u�W�F�N�g�Ƀ{�b�N�X�R���C�_�[��ǉ�����
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

            // �{�b�N�X�R���C�_�[�̃T�C�Y��ʒu�𒲐�����
            boxCollider.size = new Vector2(1, 1); // �T�C�Y�̐ݒ�
        }
    }

    //�u���b�N�W�F�l���[�^�[�ɏ��������������Ǐd���������

    //m_roadRoute = m_generator.GetRoute();



    //public List<Vector2Int> GetRoute()
    //{
    //    List<Vector2Int> route = new List<Vector2Int>();

    //    for (int i = 0; i < m_blocks.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < m_blocks.GetLength(1); j++)
    //        {
    //            // �u���b�N���󂳂ꂽ�ꍇ��R�A�̏ꍇ�ɂ̂ݒǉ�����
    //            if (m_blocks[i, j] == null || m_blocks[i, j].tag != "Block")
    //            {
    //                Vector2Int newVector = new Vector2Int(j, i);

    //                // �V�����v�f�����X�g�Ɋ܂܂�Ă��Ȃ��ꍇ�̂ݒǉ�
    //                if (!route.Contains(newVector))
    //                {
    //                    route.Add(newVector);
    //                }
    //            }
    //        }
    //    }

    //    return route;
    //}
}
