using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttackBase : MonoBehaviour
{
    [Header("�U������")]
    [SerializeField] private float m_attackTime = 5.0f;


    // �U������
    virtual public void Attack(Transform target, int attackRank = 1)
    {
    }

    // �P�̍U��
    virtual public void AttackOne(Vector3 target, int attackRank = 1)
    {

    }


    public float AttackTime
    {
        get { return m_attackTime; }
    }

}
