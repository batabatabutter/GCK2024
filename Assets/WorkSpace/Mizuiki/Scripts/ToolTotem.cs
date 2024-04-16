using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTotem : Block
{
	[Header("�󂯂�_���[�W")]
	[SerializeField] private float m_ownDamage = 100;

	[Header("�_���[�W�Ԋu")]
	[SerializeField] private float m_damageInterval = 1.0f;
	// �_���[�W�p�̃^�C�}�[
	private float m_damageTimer = 0.0f;

	private void Update()
	{
		// ���g�Ƀ_���[�W(Interval����)
		if (m_damageTimer <= 0.0f)
		{
			// �^�C�}�[�ɃC���^�[�o�������Z
			m_damageTimer += m_damageInterval;
			// �_���[�W
			AddMiningDamage(m_ownDamage);
		}

	}
}
