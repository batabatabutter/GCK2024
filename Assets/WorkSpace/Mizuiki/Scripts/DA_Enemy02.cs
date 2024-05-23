using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy02 : DA_Enemy
{
	private void Start()
	{
		// �^�C�v�w��A��
		IsType = true;
	}

	// �h���m�R�T���h
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// �T���h�����擾
		MyFunction.Direction dir = MyFunction.GetDirection(direction);

		// ���T���h
		bool horizontal = (int)dir % 2 == 1;
		//horizontal = true;

		// �^�[�Q�b�g����̋���
		Vector3 distance = Vector3.zero;
		// ���Z��
		Vector3 add = Vector3.zero;
		// �U���ʒu
		Vector3 attackPos = target.position;

		// ���T���h
		if (horizontal)
		{
			distance.x = 3;
			add.y = 1;
			attackPos.y -= Mathf.Round(range / 2);
		}
		// �c�T���h
		else
		{
			distance.y = 3;
			add.x = 1;
			attackPos.x -= Mathf.Round(range / 2);
		}

		for (int i = 0; i < (int)range; i++)
		{
			// �U���ʒu�ɓG�𐶐�
			AttackOne(attackPos + distance, attackRank);
			AttackOne(attackPos - distance, attackRank);
			attackPos += add;
		}

	}
}
