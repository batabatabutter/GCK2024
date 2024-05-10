using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackRollRock : DungeonAttackBase
{
	[Header("�]�����")]
	[SerializeField] GameObject m_prefab;
	[Header("���̃n�C���C�g")]
	[SerializeField] GameObject m_highlight;
	[Header("���̃n�C���C�g���o�����鋗��")]
	[SerializeField] private float m_targetDistance = 5.0f;


	private void Start()
	{
		if (m_prefab == null)
		{
			Debug.Log("RollRock : �v���n�u��ݒ肵�Ă�");
		}
		if (m_highlight == null)
		{
			Debug.Log("RollRock : �v���n�u��ݒ肵�Ă�");
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

		// �����ʒu
		Vector3 rollingPos = target.position;
		// �����p�x
		float rollingRotation = 0.0f;

		//4�ʂ肾����O�`�R
		MyFunction.Direction direction = MyFunction.GetRandomDirection();
		// �����_���ȕ�������U��
		switch (direction)
		{
			case MyFunction.Direction.UP:
				// �ォ��
				rollingPos = new Vector3(target.position.x, target.position.y + m_targetDistance, 0);
				rollingRotation = 180;
				break;

			case MyFunction.Direction.DOWN:
				// ������
				rollingPos = new Vector3(target.position.x, target.position.y - m_targetDistance, 0);
				rollingRotation = 0;
				break;

			case MyFunction.Direction.LEFT:
				// ������
				rollingPos = new Vector3(target.position.x - m_targetDistance, target.position.y, 0);
				rollingRotation = 270;
				break;

			case MyFunction.Direction.RIGHT:
				// �E����
				rollingPos = new Vector3(target.position.x + m_targetDistance, target.position.y, 0);
				rollingRotation = 90;
				break;
		}
		// �]�����̐���
		Instantiate(m_prefab, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
		// �n�C���C�g�̐���
		Instantiate(m_highlight, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
	}


}
