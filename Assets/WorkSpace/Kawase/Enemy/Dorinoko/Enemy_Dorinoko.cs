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


    }

    public override void Attack()
    {
        // �q���̃I�u�W�F�N�g���擾����
        Instantiate(m_dori, gameObject.transform.position, Quaternion.identity, gameObject.transform);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }

}
