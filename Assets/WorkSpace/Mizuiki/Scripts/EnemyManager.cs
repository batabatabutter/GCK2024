using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[Header("�G�l�~�[���X�g")]
	[SerializeField] private List<Enemy> m_enemies = new();


	// ���X�g�ɒǉ�
	public void AddEnemy(Enemy enemy)
	{
		m_enemies.Add(enemy);
	}

	public void SetEnabled(bool enabled)
	{
		// null �̓��X�g����폜
		m_enemies.RemoveAll(e => e == null);

		// �L���ݒ�����Ă���
		foreach (Enemy enemy in m_enemies)
		{
			enemy.enabled = enabled;
		}
	}
}
