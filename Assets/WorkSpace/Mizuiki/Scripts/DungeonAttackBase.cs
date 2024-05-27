using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttackBase : MonoBehaviour
{
    //[Header("�U������")]
    //[SerializeField] private float m_attackTime = 5.0f;


    // �U������
    virtual public void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
    {
    }

    // �P�̍U��
    virtual public void AttackOne(Vector3 target, int attackRank = 1)
    {

    }

    // �U���͈͂̐ݒ�
	virtual public void SetAttackRange(float range)
	{
	}

    // �����N�ɉ����������ʂ̐ݒ�
    virtual public void SetRankValue(float value)
    {
    }


	//public float AttackTime
 //   {
 //       get { return m_attackTime; }
 //       set { m_attackTime = value; }
 //   }

}
