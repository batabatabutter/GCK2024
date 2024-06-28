using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DA_FallRock : DungeonAttackBase
{
	//�v���C���[�̉��ɏo��n�C���C�g�̒Ⴓ
	const float HEIGLIGHT_HEIGHT = 0.5f;

	[Header("�v���n�u")]
	[SerializeField] GameObject m_fallRockPrefab;
	[Header("�n�C���C�g")]
	[SerializeField] GameObject m_fallRockHighlight;
	[Header("���΂̐������鍂��")]
	[SerializeField] float m_rockHeight = 3.0f;


	private void Start()
	{
		if (m_fallRockPrefab == null)
		{
			Debug.Log("FallRock : �v���n�u��ݒ肵�Ă�");
		}
		if (m_fallRockHighlight == null)
		{
			Debug.Log("FallRock : �v���n�u��ݒ肵�Ă�");
		}
	}


	// �U��1��
	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		// �v���n�u���Ȃ�
		if (m_fallRockPrefab == null)
		{
			return;
		}

		// �U���Ώۂ��Ȃ�
		if (target == null)
		{
			Debug.Log("�U���Ώۂ����Ȃ���");
			return;
		}

		// �u���b�N�̈ʒu�Ɍ���
		target = MyFunction.RoundHalfUp(target);

		Collider2D col = Physics2D.OverlapCircle(target, 0, LayerMask.GetMask("Block"));
		// �����\��n�ɃI�u�W�F�N�g������
		if (col)
		{
			// �u���b�N�^�O���t���Ă�
			if (col.CompareTag("Block"))
			{
				return;
			}
		}

		// �n�C���C�g�̏o���ʒu
		Vector3 highlightPos = new(target.x, target.y, 0);
		// �n�C���C�g�̐���
		GameObject highLight = Instantiate(m_fallRockHighlight, highlightPos, Quaternion.identity);

		// ��̗�����ʒu
		Vector3 rockfallPos = new(target.x, target.y + m_rockHeight, 0);
		// ��̐���
		Instantiate(m_fallRockPrefab, rockfallPos, Quaternion.identity).GetComponent<FallRock>().SetHighLight(highLight);
	}

}
