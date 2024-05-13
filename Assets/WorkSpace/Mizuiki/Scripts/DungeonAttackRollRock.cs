using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackRollRock : DungeonAttackBase
{
	[Header("�]�����")]
	[SerializeField] GameObject m_rollRockPrefab;
	[Header("���̃n�C���C�g")]
	[SerializeField] GameObject m_rollRockHighlight;
	[Header("�₪�o�����鋗��")]
	[SerializeField] private float m_targetDistance = 5.0f;


	private void Start()
	{
		if (m_rollRockPrefab == null)
		{
			Debug.Log("RollRock : �v���n�u��ݒ肵�Ă�");
		}
		if (m_rollRockHighlight == null)
		{
			Debug.Log("RollRock : �v���n�u��ݒ肵�Ă�");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="target"></param>
	/// <param name="attackRank">����(MyFunction.Direction)</param>
	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		// �v���n�u���Ȃ�
		if (m_rollRockPrefab == null)
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
		Vector3 rollingPos = target;
		// �����p�x
		float rollingRotation = 0.0f;

		//4�ʂ肾����O�`�R
		//MyFunction.Direction direction = MyFunction.GetRandomDirection();
		MyFunction.Direction direction = (MyFunction.Direction)attackRank;
		// �w���������U��
		switch (direction)
		{
			case MyFunction.Direction.UP:
				// �ォ��
				rollingPos = new Vector3(target.x, target.y + m_targetDistance, 0);
				rollingRotation = 180;
				break;

			case MyFunction.Direction.DOWN:
				// ������
				rollingPos = new Vector3(target.x, target.y - m_targetDistance, 0);
				rollingRotation = 0;
				break;

			case MyFunction.Direction.LEFT:
				// ������
				rollingPos = new Vector3(target.x - m_targetDistance, target.y, 0);
				rollingRotation = 270;
				break;

			case MyFunction.Direction.RIGHT:
				// �E����
				rollingPos = new Vector3(target.x + m_targetDistance, target.y, 0);
				rollingRotation = 90;
				break;
		}
		// �]�����̐���
		Instantiate(m_rollRockPrefab, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
		// �n�C���C�g�̐���
		Instantiate(m_rollRockHighlight, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
	}

}
