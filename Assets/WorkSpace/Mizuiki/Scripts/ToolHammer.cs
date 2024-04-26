using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolHummer : Tool
{
    [Header("ハンマーの範囲")]
    [Header("ハンマーの半径")]
    [SerializeField] private float m_radius = 1.0f;
    [Header("ハンマーの角度")]
    [SerializeField] private float m_degree = 90.0f;

    [Header("ハンマーの攻撃力")]
    [SerializeField] private float m_power = 100.0f;


	public override void UseTool(GameObject obj)
	{
		base.UseTool(obj);

		// 向き(初期値は上にしておく)
		Vector2 direction = MyFunction.GetFourDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition) - obj.transform.position);

		// ハンマーの範囲のブロックを取得
		Collider2D[] blocks = Physics2D.OverlapCircleAll(obj.transform.position, m_radius, LayerMask.GetMask("Block"));

        // 比較用のcos
        float cos = Mathf.Cos(m_degree / 2.0f);

        foreach (Collider2D block in blocks)
        {
            // プレイヤーからブロックへのベクトル
            Vector2 playerToBlock = block.transform.position - obj.transform.position;

            // プレイヤーとブロックの内積
            float dot = Vector2.Dot(playerToBlock, direction);

            // ハンマーの範囲外
            if (dot < cos)
                continue;

            // ブロックスクリプトの取得
            if (block.TryGetComponent(out Block b))
            {
                // ダメージを与える
                b.AddMiningDamage(m_power);
            }

        }

	}

}
