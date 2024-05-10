using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttackBase : MonoBehaviour
{
    [Header("UŒ‚ŠÔ")]
    [SerializeField] private float m_attackTime = 5.0f;


    // UŒ‚‚·‚é
    virtual public void Attack(Transform target, int attackRank = 1)
    {
    }

    // ’P‘ÌUŒ‚
    virtual public void AttackOne(Vector3 target, int attackRank = 1)
    {

    }


    public float AttackTime
    {
        get { return m_attackTime; }
    }

}
