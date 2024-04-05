using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHealing : Tool
{
    [Header("�񕜗�")]
    [SerializeField] private int m_healingValue = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	// �c�[�����g�p
	public override void UseTool(GameObject obj)
	{
        // �v���C���[���擾
        if (obj.TryGetComponent(out Player player))
        {
            // ��
            player.Healing(m_healingValue);
        }

	}

}
