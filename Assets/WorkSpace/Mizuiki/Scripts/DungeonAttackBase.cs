using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttackBase : MonoBehaviour
{
    // UŒ‚‚·‚é
    virtual public void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
    {
    }

    // ’P‘ÌUŒ‚
    virtual public void AttackOne(Vector3 target, int attackRank = 1)
    {

    }

    // UŒ‚”ÍˆÍ‚Ìİ’è
	virtual public void SetAttackRange(float range)
	{
	}

    // ƒ‰ƒ“ƒN‚É‰‚¶‚½‘‰Á—Ê‚Ìİ’è
    virtual public void SetRankValue(float value)
    {
    }


}
