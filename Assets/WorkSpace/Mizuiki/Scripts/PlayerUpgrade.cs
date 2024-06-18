using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
	[Header("�v���C���[�̍̌@�X�N���v�g")]
	[SerializeField] private PlayerMining m_playerMining = null;

    [Header("�v���C���[�̃c�[���X�N���v�g")]
    [SerializeField] private PlayerTool m_playerTool = null;

    [Header("�����i�K�̋�؂�")]
    [SerializeField] private int m_stageDelimiter = 10;

    [Header("�����i�K")]
    [SerializeField] private UpgradeData[] m_upgradeStage;

    [System.Serializable]
    public struct UpgradeStageValue
    {
        public int stage;                       // �������n�߂鋭���i�K
        public MiningData.MiningValue value;  // ������
    }
    [Header("�����i�K���Ƃ̑�����"), Tooltip("[stage] �̒i�K����1�i�K�オ�邲�Ƃ�[value]�̐��l�����Z�����")]
    [SerializeField] private UpgradeStageValue[] m_upgradeStageValue;

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

    // �̌@�A�b�v�O���[�h���g�p
	public void Upgrade(int value = 1)
	{
        // �K�v�f�ނ̎擾
        Items[] items = GetNeedMaterials(value);

        // �f�ނ�����Ȃ�
        if (!m_playerTool.CheckCreate(items))
        {
            Debug.Log("�f�ޕs��");
            return;
        }

        // �f�ނ̏���
        m_playerTool.ConsumeMaterials(items, value);

        Debug.Log("�A�b�v�O���[�h : " + (m_upgradeRank + value).ToString());

        // �A�b�v�O���[�h
        m_upgradeRank += value;

        // �����l
        UpgradeData upgradeValue = GetValue();
        MiningData.MiningValue upgradeStageValue = GetStageValue(m_upgradeRank - value, m_upgradeRank);

	}

    // �K�v�f�ނ̎擾
    private Items[] GetNeedMaterials(int rank)
    {
        // �K�v�f��
        Items[] items = new Items[0];

        for (int i = 0; i < rank; i++)
        {
            // �K�v�f�ގ擾
            Items[] cost = GetValue(m_upgradeRank + i).Cost;

			Items[] dst = new Items[items.Length + cost.Length];

			// �K�v�f�ނ̒ǉ�
			Array.Copy(items, dst, items.Length);
			Array.Copy(cost, 0, dst, items.Length, cost.Length);
            items = dst;
        }

        return items;
    }

    // �����l�̎擾(�����N����)
    private UpgradeData GetValue()
    {
        // �����l��Ԃ�
        return GetValue(m_upgradeRank);
    }
    private UpgradeData GetValue(int rank)
    {
		// �����i�K�̎擾
		int stage = GetStage(rank);
		// �����i�K�͈̔͂ɃN�����v
		stage = Mathf.Clamp(stage, 0, m_upgradeStage.Length);
		// �����l��Ԃ�
		return m_upgradeStage[stage];
	}

	// �����l�̎擾(�i�K����)
	private MiningData.MiningValue GetStageValue(int beforeRank, int afterRank)
    {
        // �����O�̒i�K
        int beforeStage = GetStage(beforeRank);
        // ������̒i�K
        int afterStage = GetStage(afterRank);

        // �����i�K���オ���Ă��Ȃ�
        if (beforeStage >= afterStage)
            return MiningData.MiningValue.Zero();

        // ������
        MiningData.MiningValue val = MiningData.MiningValue.Zero();
        foreach (UpgradeStageValue stageValue in m_upgradeStageValue)
        {
            // �����J�n�i�K
            int stage = stageValue.stage;

            // �������n�߂�i�K�ł͂Ȃ�
            if (stage < afterStage)
                continue;

            // �����l���Z
            val += stageValue.value;
        }
        // �����i�K���|����
        return val * (afterStage - beforeRank);
    }

    // �����i�K�擾
    private int GetStage(int rank)
    {
        return rank / m_stageDelimiter;
    }

}
