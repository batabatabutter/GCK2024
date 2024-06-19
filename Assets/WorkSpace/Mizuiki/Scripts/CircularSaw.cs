using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
	[System.Serializable]
	public struct ToolLevel
	{
		public MiningData.MiningType type;
		public int level;
	}

	[Header("�ۂ̂��̎��")]
	[SerializeField] private MiningData.MiningType m_type;
	[Header("�ۂ̂��̈ړ����x")]
	[SerializeField] private float m_circularSawSpeed = 1.0f;
	[Header("�ۂ̂��̉�]���x")]
	[SerializeField] private float m_circularSawRotate = 100.0f;

	[Header("�����i�K�̋�؂�")]
	[SerializeField] private int m_stageDelimiter = 10;

	[Header("�X�v���C�g�̐ݒ��")]
	[SerializeField] private SpriteRenderer m_spriteRenderer = null;

	[Header("�̌@�f�[�^�x�[�X")]
	[SerializeField] private MiningDataBase m_dataBase = null;
	private readonly Dictionary<MiningData.MiningType, MiningData> m_miningDatas = new();

	[Header("�c�[���̃��x��")]
	[SerializeField] private ToolLevel[] m_toolLevel = null;
	[Tooltip("�C���X�y�N�^�[�̒l�𔽉f������")]
	[SerializeField] private bool m_enabled = false;
	// ���x���̎���
	private Dictionary<MiningData.MiningType, int> m_toolLevels = new();

	[Header("�A�C�e��")]
	[SerializeField] private PlayerItem m_playerItem = null;



	private void Awake()
	{
		// �X�v���C�g�����_�[���Ȃ���ΐݒ�
		if (m_spriteRenderer == null)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		// �c�[���̊�{���
		foreach (MiningData data in m_dataBase.MiningData)
		{
			// �㏑���h�~
			if (m_miningDatas.ContainsKey(data.Type))
				continue;

			m_miningDatas[data.Type] = data;
		}

		if (m_enabled)
		{
			// �c�[���̃��x��
			foreach (ToolLevel toolLevel in m_toolLevel)
			{
				// �㏑���h�~
				if (m_toolLevels.ContainsKey(toolLevel.type))
					continue;

				m_toolLevels[toolLevel.type] = toolLevel.level;
			}
		}
	}

	// ��
	public void Rotate(float speed)
	{
		transform.localEulerAngles += m_circularSawRotate * speed * Time.deltaTime * Vector3.back;
	}

	// �ۂ̂��̈ʒu�擾
	public Vector3 SetPosition(Vector3 miningPoint)
	{
		// �ۂ̂�����̌@�ʒu�ւ̃x�N�g��
		Vector3 circularSawToMining = miningPoint - transform.position;
		// �ۂ̂��ƍ̌@�ʒu�̋���
		float distance = circularSawToMining.magnitude;

		// ������ 1f �̈ړ��ʈȓ��Ȃ炻�̂܂܍̌@�n�_��Ԃ�
		if (distance <= m_circularSawSpeed * Time.deltaTime)
			return miningPoint;

		// �̌@�ʒu�ւ̃x�N�g�����K��
		circularSawToMining.Normalize();

		transform.position += m_circularSawSpeed * Time.deltaTime * circularSawToMining;

		// �ۂ̂��̈ʒu��Ԃ�
		return transform.position + (m_circularSawSpeed * Time.deltaTime * circularSawToMining);
	}

	// �̌@�l�擾
	public MiningData.MiningValue GetMiningValue()
	{
		// ��{�f�[�^�擾
		MiningData miningData = m_miningDatas[m_type];
		// ��b�l�擾
		MiningData.MiningValue value = miningData.Value;
		// �������x���擾
		int level = m_toolLevels[m_type];

		// ���������N
		int rank = 0;

		// ���������N�� 0 ���傫���ԃ��[�v
		while (m_stageDelimiter <= 0)
		{
			// ���Z��
			int lv = level % m_stageDelimiter;
			level -= m_stageDelimiter;

			// �̌@�l���Z
			value += miningData.Upgrades[rank].Value * lv;

			// �����N�������i�K�ȏ�
			if (rank >= miningData.Upgrades.Length - 1)
				continue;

			// �����N�A�b�v
			rank++;
		}

		return value;
	}

	// �K�v�f�ނ̎擾
	private Items[] GetNeedMaterials(int addLevel)
	{
		// �K�v�f��
		Items[] items = new Items[0];

		for (int i = 0; i < addLevel; i++)
		{
			// ���������N�̎擾
			int r = GetRank(m_toolLevels[m_type] + i);

			// �K�v�f�ގ擾
			Items[] cost = m_miningDatas[m_type].Upgrades[r].Cost;
			// [items] �� [cost] ���������邽�߂̓��ꕨ
			Items[] dst = new Items[items.Length + cost.Length];

			// �K�v�f�ނ̒ǉ�
			Array.Copy(items, dst, items.Length);
			Array.Copy(cost, 0, dst, items.Length, cost.Length);
			items = dst;
		}

		return items;
	}

	// ���������N�̎擾
	private int GetRank(int add)
	{
		// ��؂育�Ƃ̃����N�ɂ���
		int rank = add / m_stageDelimiter;

		// ����𒴂��Ȃ��悤�ɃN�����v
		rank = Mathf.Clamp(rank, 0, m_miningDatas[m_type].Upgrades.Length);

		return rank;
	}

	// �������x���A�b�v
	public void Upgrade(int level = 1)
	{
		// �K�v�f�ނ̎擾
		Items[] items = GetNeedMaterials(level);

		// �c�[���̃A�b�v�O���[�h���ł��Ȃ�
		if (!m_playerItem.CheckCreate(items))
		{
			Debug.Log("�A�b�v�O���[�h�ł��Ȃ���");
			return;
		}

		Debug.Log("�A�b�v�O���[�h : " + m_type + " : " + m_toolLevels[m_type] + "->" + (m_toolLevels[m_type] + level));

		// �c�[���̃��x�����Z
		m_toolLevels[m_type] += level;

		// �f�ނ̏���
		m_playerItem.ConsumeMaterials(items);

		// �f�[�^�ۑ�
		SaveDataReadWrite.m_instance.MiningLevel = m_toolLevels;
		SaveDataReadWrite.m_instance.Items = m_playerItem.Items;
		SaveDataReadWrite.m_instance.Save();
	}

	// �̂��̎�ސݒ�
	public void SetType(MiningData.MiningType type)
	{
		// ��ނ̐ݒ�
		m_type = type;
		// �X�v���C�g�̐ݒ�
		m_spriteRenderer.sprite = m_miningDatas[type].Sprite;

		Debug.Log(type + "�ɕύX");
	}
	public void SetType(string typeStr)
	{
		// �啶���ɕϊ�
		typeStr = typeStr.ToUpper();
		// ��ނ̐ݒ�
		if (Enum.IsDefined(typeof(MiningData.MiningType), typeStr))
		{
			SetType(Enum.Parse<MiningData.MiningType>(typeStr));
		}
		else
		{
			Debug.Log(typeStr + "��[" + typeof(MiningData.MiningType) + "]�ɑ��݂��܂���");
		}
	}


	// ���
	public MiningData.MiningType MiningType
	{
		get { return m_type; }
	}

	// ���x���ݒ�
	public Dictionary<MiningData.MiningType, int> MiningLevels
	{
		set { m_toolLevels = value; }
	}

}
