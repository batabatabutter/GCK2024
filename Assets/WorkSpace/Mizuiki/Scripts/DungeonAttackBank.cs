using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackBank : DungeonAttackBase
{
	[Header("�y��")]
	[SerializeField] GameObject m_bankPrefab;
	[Header("�y��n�C���C�g")]
	[SerializeField] GameObject m_bankHighlight;


	private void Start()
	{
		if (m_bankPrefab == null)
		{
			Debug.Log("FallRock : �v���n�u��ݒ肵�Ă�");
		}
		if (m_bankHighlight == null)
		{
			Debug.Log("FallRock : �v���n�u��ݒ肵�Ă�");
		}
	}

	public override void Attack(Transform target, int attackRank = 1)
	{
		// �v���n�u���Ȃ�
		if (m_bankPrefab == null)
		{
			return;
		}

		// �U���Ώۂ��Ȃ�
		if (target == null)
		{
			Debug.Log("�U���Ώۂ����Ȃ���");
			return;
		}

		Vector3 pos = target.position;

		AttackOne(pos, attackRank);

	}

	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		target.x -= 1;
		target.y += 1;

		// �^�[�Q�b�g�̎���ɍU�����o��
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				// �^�[�Q�b�g�̈ʒu�ɂ͔������Ȃ�
				if (i == 1 && j == 1)
					continue;

				// �U������
				Instantiate(m_bankPrefab, new Vector3(target.x + j, target.y - i, 0), Quaternion.identity);
				// �n�C���C�g����
				Instantiate(m_bankHighlight, new Vector3(target.x + j, target.y - i, 0), Quaternion.identity);
			}
		}
	}

}
