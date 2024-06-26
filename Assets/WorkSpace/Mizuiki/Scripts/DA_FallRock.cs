using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DA_FallRock : DungeonAttackBase
{
	//プレイヤーの下に出るハイライトの低さ
	const float HEIGLIGHT_HEIGHT = 0.5f;

	[Header("プレハブ")]
	[SerializeField] GameObject m_fallRockPrefab;
	[Header("ハイライト")]
	[SerializeField] GameObject m_fallRockHighlight;
	[Header("落石の生成する高さ")]
	[SerializeField] float m_rockHeight = 3.0f;


	private void Start()
	{
		if (m_fallRockPrefab == null)
		{
			Debug.Log("FallRock : プレハブを設定してね");
		}
		if (m_fallRockHighlight == null)
		{
			Debug.Log("FallRock : プレハブを設定してね");
		}
	}


	// 攻撃1つ
	public override void AttackOne(Vector3 target, int attackRank = 1)
	{
		// プレハブがない
		if (m_fallRockPrefab == null)
		{
			return;
		}

		// 攻撃対象がない
		if (target == null)
		{
			Debug.Log("攻撃対象がいないよ");
			return;
		}

		// ブロックの位置に決定
		target = MyFunction.RoundHalfUp(target);

		Collider2D col = Physics2D.OverlapCircle(target, 0, LayerMask.GetMask("Block"));
		// 落下予定地にオブジェクトがある
		if (col)
		{
			// ブロックタグが付いてる
			if (col.CompareTag("Block"))
			{
				return;
			}
		}

		// ハイライトの出現位置
		Vector3 highlightPos = new(target.x, target.y, 0);
		// ハイライトの生成
		GameObject highLight = Instantiate(m_fallRockHighlight, highlightPos, Quaternion.identity);

		// 岩の落ちる位置
		Vector3 rockfallPos = new(target.x, target.y + m_rockHeight, 0);
		// 岩の生成
		Instantiate(m_fallRockPrefab, rockfallPos, Quaternion.identity).GetComponent<FallRock>().SetHighLight(highLight);
	}

}
