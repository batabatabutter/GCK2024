using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
/// <summary>
/// �p����
/// Enemy -> EnemyDwell -> Dorinoko
/// </summary>
public class Enemy_Dorinoko : EnemyDwell
{
    // Start is called before the first frame update

    [Header("�ǂ胋")]
    [SerializeField] GameObject m_dori;
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (base.Player != null)
        {
            Transform target = base.Player.transform;
            // �^�[�Q�b�g�̕����x�N�g�����v�Z
            Vector3 direction = target.position - transform.position;

            // �����x�N�g���𐳋K��
            direction.Normalize();

            // �����x�N�g������p�x���v�Z
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // �p�x���l�̌ܓ����āA0���A90���A180���A270���̂����ꂩ�Ɋۂ߂�
            angle = Mathf.Round(angle / 90) * 90 - 90;

            // �I�u�W�F�N�g����]
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public override void Attack()
    {
        Instantiate(m_dori,gameObject.transform.position,Quaternion.identity,gameObject.transform);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }

}
