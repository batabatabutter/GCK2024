using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_RollRock01 : DA_RollRock
{
	//[Header("���Ԋ�")]

	//[Header("���ɕ��Ԑ�")]
	//[SerializeField] private int m_lineUpCount = 5;

	//[Header("�����N������")]
	//[SerializeField] private float m_rankValue = 0.5f;


	// �U���J�n
	public override void Attack(Transform target, MyFunction.Direction direction, float range, float rankValue, int attackRank = 1)
	{
		// �]��������̌���
		int dir = (int)MyFunction.GetDirection(direction);

		// ��
		bool horizon = dir % 2 == 1;

		// ��̐�����
		int rockCount = /*m_lineUpCount*/(int)range + ((int)(attackRank * rankValue));

		// �U���ʒu
		Vector3 pos = target.position;
		// ���Z��
		Vector3 addPos;

		// ��
        if (horizon)
        {
			pos.y = target.position.y - (rockCount / 2);
			addPos = Vector3.up;
        }
		// �c
		else
		{
			pos.x = target.position.x - (rockCount / 2);
			addPos = Vector3.right;
		}

        // ���񂾊�𐶐�����
        for (int i = 0; i < rockCount; i++)
		{
			// �U������
			AttackOne(pos, dir);

			pos += addPos;
		}
	}
}
