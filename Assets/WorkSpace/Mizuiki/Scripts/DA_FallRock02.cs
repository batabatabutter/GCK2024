using UnityEngine;

public class DA_FallRock02 : DA_FallRock
{
	//[Header("�����_������")]

	//[Header("���΂͈̔�")]
	//[SerializeField, Min(0.0f)] private float m_fallRockRange = 5.0f;


	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// ���΂̔����ʒu�������_���Ɍ��߂�
		Vector3 random = new(Random.Range(-/*m_fallRockRange*/range, /*m_fallRockRange*/range), Random.Range(-/*m_fallRockRange*/range, /*m_fallRockRange*/range), 0.0f);
		// �l�̌ܓ����ău���b�N�Ɖ�ʒu�ɒ���
		random = MyFunction.RoundHalfUp(random);
		// ���Δ���
		AttackOne(target.position + random, attackRank);
	}

	//// �U���͈͂̐ݒ�
	//public override void SetAttackRange(float range)
	//{
	//	// ���Δ͈͂̐ݒ�
	//	m_fallRockRange = range;
	//}

	//// �����N�����ʂ̐ݒ�
	//public override void SetRankValue(float value)
	//{
		
	//}
}
