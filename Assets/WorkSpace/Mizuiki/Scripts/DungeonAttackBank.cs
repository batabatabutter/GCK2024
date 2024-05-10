using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackBank : DungeonAttackBase
{
	[Header("�y��")]
	[SerializeField] GameObject m_prefab;
	[Header("�y��n�C���C�g")]
	[SerializeField] GameObject m_highlight;


	private void Start()
	{
		if (m_prefab == null)
		{
			Debug.Log("FallRock : �v���n�u��ݒ肵�Ă�");
		}
		if (m_highlight == null)
		{
			Debug.Log("FallRock : �v���n�u��ݒ肵�Ă�");
		}
	}

	public override void Attack(Transform target, int attackRank = 1)
	{
		// �v���n�u���Ȃ�
		if (m_prefab == null)
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

		pos.x -= 1;
		pos.y += 1;

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (i == 1 && j == 1)
					continue;


				Instantiate(m_prefab, new Vector3(pos.x + j, pos.y - i, 0), Quaternion.identity);
				Instantiate(m_highlight, new Vector3(pos.x + j, pos.y - i, 0), Quaternion.identity);
			}

		}

	}

}
