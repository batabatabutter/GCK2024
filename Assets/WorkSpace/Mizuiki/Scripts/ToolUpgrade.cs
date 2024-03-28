using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUpgrade : Tool
{
    [Header("�����l")]
    [SerializeField] private int m_upgradeValue = 20;
    [Header("������")]
    [SerializeField] private float m_upgradeAmount = 2.0f;

    // �v���C���[�̃c�[��
	private PlayerTool m_playerTool = null;

	// �̌@
	private PlayerMining m_mining = null;
    // �c�[���g�p���̍̌@��
    private int m_useMiningCount = 0;


    // Update is called once per frame
    void Update()
    {
        // �̌@�X�N���v�g���Ȃ���Ώ��������Ȃ�
        if (!m_mining)
            return;

        // �c�[���g�p�ォ��̍̌@��
        int count = m_mining.BrokenCount - m_useMiningCount;

        // �����l���̌@����
        if (count >= m_upgradeValue)
        {
            // �����̃��Z�b�g
            m_mining.MiningSpeedRate = 1.0f;

            // ���L���X�g�J�n
            m_playerTool.SetRecast(true, ToolData.ToolType.UPGRADE);

            m_mining = null;

        }
        
    }

    // �̌@�A�b�v�O���[�h���g�p
	public override void UseTool(GameObject obj)
	{
        Debug.Log("�A�b�v�O���[�h");

        // �v���C���[�̎擾
        if (obj.TryGetComponent(out PlayerMining mining))
        {
            // �̌@�X�N���v�g���擾����
            m_mining = mining;

            // ���݂̍̌@�񐔂��擾
            m_useMiningCount = mining.BrokenCount;

            // �̌@���x�̔{����ݒ�
            m_mining.MiningSpeedRate = m_upgradeAmount;

        }

        // �c�[���̎擾
        m_playerTool = obj.GetComponent<PlayerTool>();
	}

}
