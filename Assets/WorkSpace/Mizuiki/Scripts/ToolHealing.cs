using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHealing : Tool
{
    [Header("回復量")]
    [SerializeField] private int m_healingValue = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	// ツールを使用
	public override void UseTool(GameObject obj)
	{
        // プレイヤーを取得
        if (obj.TryGetComponent(out Player player))
        {
            // 回復
            player.Healing(m_healingValue);
        }

	}

}
