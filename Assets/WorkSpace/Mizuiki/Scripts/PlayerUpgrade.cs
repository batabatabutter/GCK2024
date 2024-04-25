using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
	[Header("�v���C���[�̍̌@�X�N���v�g")]
	[SerializeField] private PlayerMining m_playerMining = null;

    [Header("�v���C���[�̃c�[���X�N���v�g")]
    [SerializeField] private PlayerTool m_playerTool = null;

    [Header("�̌@����")]
    [SerializeField] private ToolData m_upgradeData = null;

    [Header("�����l")]
    [SerializeField] private PlayerMining.MiningValue m_upgradeValue;

    [Header("�����i�K")]
    [SerializeField] private PlayerMining.MiningValue[] m_upgradeStage;

    [Header("���������N")]
    [SerializeField] private int m_upgradeRank = 0;



	private void Start()
	{
        // �̌@�X�N���v�g���Ȃ�
		if (!m_playerMining)
        {
            // �̌@�X�N���v�g���擾
            m_playerMining = GetComponent<PlayerMining>();
        }

        // �c�[���X�N���v�g���Ȃ�
        if (!m_playerTool)
        {
            // �v���C���[�X�N���v�g�̎擾
            m_playerTool = GetComponent<PlayerTool>();
        }
	}

	// Update is called once per frame
	void Update()
    {
    }

    // �̌@�A�b�v�O���[�h���g�p
	public void Upgrade(int value = 1)
	{
        // �쐬�ł��Ȃ�
        if (!m_playerTool.CheckCreate(m_upgradeData))
        {
            Debug.Log("�f�ޕs��");
            return;
        }

        Debug.Log("�A�b�v�O���[�h : " + (m_upgradeRank + value).ToString());

        // �A�b�v�O���[�h
        m_upgradeRank += value;

        // �����l
        PlayerMining.MiningValue upgradeValue = GetValue();

        // ����
        m_playerMining.MiningValueBase += upgradeValue * value;

        // �f�ނ̏���
        m_playerTool.ConsumeMaterials(m_upgradeData, value);

	}

    // �����l�̎擾
    private PlayerMining.MiningValue GetValue()
    {
        // �����i�K�̎擾
        int rank = m_upgradeRank / 100;
        // �����i�K�͈̔͂ɃN�����v
        rank = Mathf.Clamp(rank, 0, m_upgradeStage.Length);
        // �����l��Ԃ�
        return m_upgradeStage[rank];
    }

}
