using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolArmor : Tool
{
    [Header("�A�[�}�[�̐�")]
    [SerializeField] private int m_armorCount = 1;

    // �v���C���[
    private Player m_player = null;
    // �v���C���[�̃c�[��
    private PlayerTool m_playerTool = null;


	private void Update()
	{
        // �v���C���[���Ȃ���Ώ������Ȃ�
        if (!m_player)
            return;
		
        // �A�[�}�[���Ȃ��Ȃ�΃��L���X�g�J�n
        if (m_player.Armor <= 0)
        {
            // �A�[�}�[�̃��L���X�g�J�n
            m_playerTool.SetRecast(true, ToolData.ToolType.ARMOR);

            m_player = null;
        }

	}

	public override void UseTool(GameObject obj)
	{
        Debug.Log("�A�[�}�[");

        // �v���C���[�̎擾
        if (obj.TryGetComponent(out Player player))
        {
            // �v���C���[�̎擾
            m_player = player;

            // �A�[�}�[�̐ݒ�
            player.Armor = m_armorCount;
        }

        m_playerTool = obj.GetComponent<PlayerTool>();

	}

}
