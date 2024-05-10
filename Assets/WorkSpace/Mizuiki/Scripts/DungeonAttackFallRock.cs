using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DungeonAttackFallRock : DungeonAttackBase
{
	//�v���C���[�̉��ɏo��n�C���C�g�̒Ⴓ
	const float HEIGLIGHT_HEIGHT = 0.5f;

	[Header("�v���n�u")]
	[SerializeField] GameObject m_prefab;
	[Header("�n�C���C�g")]
	[SerializeField] GameObject m_highlight;
	[Header("���΂̐������鍂��")]
	[SerializeField] float m_rockHeight = 3.0f;


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


	// �U��
	public override void Attack(Transform target, int attackRank = 1)
	{
		// �p�^�[������1
		//Vector3 random = new(Random.Range(-5, 5), Random.Range(-5, 5), 0.0f)
		//AttackOne(target.position + random, attackRank);

		// ��̃T�C�Y
		int massSize = 3 + (attackRank * 2);
		int massRange = massSize / 2;
		// �^�[�Q�b�g�̃O���b�h�擾
		Vector2Int targetGrid = MyFunction.RoundHalfUpInt(target.position);
		// �p�^�[������2
		for (int y = targetGrid.y - massRange; y <= targetGrid.y + massRange; y++)
		{
			for (int x = targetGrid.x - massRange; x <= targetGrid.x + massRange; x++)
			{
				// �U�������ʒu
				Vector3 attackPos = new(x, y, 0);
				// �U������
				AttackOne(attackPos);
			}
		}

	}

	// �U��1��
	public override void AttackOne(Vector3 target, int attackRank = 1)
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

		// ��̗�����ʒu
		Vector3 rockfallPos = new(target.x, target.y + m_rockHeight, 0);
		// ��̐���
		Instantiate(m_prefab, rockfallPos, Quaternion.identity);

		// �n�C���C�g�̏o���ʒu
		Vector3 highlightPos = new(target.x, target.y - HEIGLIGHT_HEIGHT, 0);
		// �n�C���C�g�̐���
		Instantiate(m_highlight, highlightPos, Quaternion.identity);
	}

}
