using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[Header("エネミーリスト")]
	[SerializeField] private List<Enemy> m_enemies = new();


	// リストに追加
	public void AddEnemy(Enemy enemy)
	{
		m_enemies.Add(enemy);
	}

	public void SetEnabled(bool enabled)
	{
		// null はリストから削除
		m_enemies.RemoveAll(e => e == null);

		// 有効設定をしていく
		foreach (Enemy enemy in m_enemies)
		{
			enemy.enabled = enabled;
		}
	}
}
