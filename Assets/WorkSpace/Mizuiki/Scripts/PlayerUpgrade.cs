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

    [Header("�̌@���x")]
    [SerializeField] private float m_upgradeSpeed = 0.1f;
    [Header("�̌@��")]
    [SerializeField] private float m_upgradePower = 10.0f;
    [Header("�N���e�B�J����")]
    [SerializeField] private float m_upgradeCritical = 1.0f;

    [Header("���������N")]
    [SerializeField] private int m_upgradeRank = 1;



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

        // �̌@���x����
        m_playerMining.MiningSpeed += m_upgradeSpeed * value;
        // �̌@�͋���
        m_playerMining.MiningPower += m_upgradePower * value;
        // �N���e�B�J��������
        m_playerMining.CriticalRate += m_upgradeCritical * value;

        // �f�ނ̏���
        m_playerTool.ConsumeMaterials(m_upgradeData, value);

	}

}
