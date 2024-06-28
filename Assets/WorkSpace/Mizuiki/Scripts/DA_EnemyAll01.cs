using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_EnemyAll01 : DA_Enemy
{
	private void Start()
	{
		// ���a���w��
		IsRadius = true;
	}

	public override void Attack(Transform target, MyFunction.Direction direction, float range, float distance, float rankValue, int attackRank = 1)
	{
		// �����͈͂̐ݒ�
		Radius = distance;

		// �����_���ȓG�̎�ނ��擾
		Enemy.Type type = (Enemy.Type)Random.Range(0, (int)Enemy.Type.OverID);
		// ��������G�̎�ނ�ݒ�
		Type = type;

		// ���߂�ꂽ���G�𐶐�
		for (int i = 0; i < (int)range; i++)
		{
			AttackOne(target.position, attackRank);
		}
	}
}
