using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_Enemy03 : DA_Enemy
{
	private void Start()
	{
		// �^�C�v�w��A��
		Type = Enemy.Type.Iwarun;
		IsType = true;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// �U���ʒu
		Vector3 attackPos = target.position;
		attackPos.x -= range / 2;
		attackPos.y -= range / 2;

		if (range < 3.0f)
		{
			return;
		}

		Vector2Int randomEmpty = new(Random.Range(0 + 1, (int)range - 1), Random.Range(0 + 1, (int)range - 1));

		for (int y = 0; y < (int)range; y++)
		{
			for (int x = 0; x < (int)range; x++)
			{
				// �[��������Ȃ�
				if (x != 0 && x != range - 1 &&
					y != 0 && y != range - 1)
					continue;

				// �l���͐������Ȃ�
				if (x == y || Mathf.Abs(x - y) == (int)range - 1)
					continue;

				// �������Ȃ��ʒu������
				if (x == randomEmpty.x || y == randomEmpty.y)
					continue;

				// �G����
				AttackOne(attackPos + new Vector3(x, y), attackRank);
			}
		}

	}
}
