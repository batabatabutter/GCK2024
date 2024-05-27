using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy02 : DA_Enemy
{
	[Header("---------- �h���m�R�T���h ----------")]
	[Header("�T���h����")]
	[SerializeField] private float m_distance = 3.0f;

	private void Start()
	{
		// �^�C�v�w��A��
		Type = Enemy.Type.Dorinoko;
		IsType = true;
	}

	// �h���m�R�T���h
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// �T���h�����擾
		MyFunction.Direction dir = MyFunction.GetDirection(direction);

		// ���T���h
		bool horizontal = (int)dir % 2 == 1;

		// �^�[�Q�b�g����̋���
		Vector3 targetDistance = Vector3.zero;
		// ���Z��
		Vector3 add = Vector3.zero;
		// �U���ʒu
		Vector3 attackPos = target.position;

		// ���T���h
		if (horizontal)
		{
			targetDistance.x = m_distance;
			add.y = 1;
			attackPos.y -= Mathf.Round(range / 2);
		}
		// �c�T���h
		else
		{
			targetDistance.y = m_distance;
			add.x = 1;
			attackPos.x -= Mathf.Round(range / 2);
		}

		for (int i = 0; i < (int)range; i++)
		{
			// �U���ʒu�ɓG�𐶐�
			AttackOne(attackPos + targetDistance, attackRank);
			AttackOne(attackPos - targetDistance, attackRank);
			attackPos += add;
		}

	}
}
