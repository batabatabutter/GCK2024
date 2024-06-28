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
	// �̌@������Ray
	public struct MiningRay
	{
		public Vector2 direction;
		public Vector2 origin;
		public float length;

		public readonly Vector2 MiningPos()
		{
			return origin + (direction * length);
		}
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
	[Header("�A�C�e��")]
	[SerializeField] private PlayerItem m_playerItem = null;

	[Header("�A�j���[�^�[")]
	[SerializeField] private Animator m_animator = null;

	// ���x���̎���
	private Dictionary<MiningData.MiningType, int> m_toolLevels = new();

	// �v���C���[
	private Transform m_player = null;
	// �̌@�͈�
	private float m_miningRange = 2.0f;


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

		// �C���X�y�N�^�[�ݒ�L��
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

		// �e�̐ݒ�
		m_player = transform.parent;

	}

	private void Start()
	{
		// �Z�[�u�f�[�^�擾
		SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;

		if (saveData)
		{
			// �����ݒ�
			SetType(saveData.MiningType);
			// �̌@���x��
			MiningLevels = saveData.MiningLevel;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			m_animator.SetTrigger("LevelUp");
		}
	}

	// ������
	public void Move(Transform player)
	{
		// �}�E�X�̈ʒu���擾
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// �ړ���̈ʒu
		Vector3 afterPos = mousePos;

		// �v���C���[����̋������̌@�͈͂��傫��
		if (Vector3.Distance(player.position, afterPos) > m_miningRange)
		{
			// �̌@�͈͂Ɏ��߂��ʒu��ݒ�
			afterPos = player.position + (afterPos - player.position).normalized * m_miningRange;
		}

		// �ۂ̂�����}�E�X�ւ̃x�N�g��
		Vector3 circularSawToMining = afterPos - transform.position;
		// �ۂ̂��ƃ}�E�X�̋���
		float distance = circularSawToMining.magnitude;

		// �̌@�ʒu�ւ̃x�N�g�����K��
		circularSawToMining.Normalize();

		// ������ 1f �̈ړ��ʈȓ��Ȃ炻�̂܂�
		if (distance <= m_circularSawSpeed * Time.deltaTime)
		{
			//afterPos = mousePos;
		}
		else
		{
			afterPos = transform.position + (m_circularSawSpeed * Time.deltaTime * circularSawToMining);
		}

		// ���W��ݒ�
		transform.position = afterPos;
	}

	// ��
	public void Rotate(float speed)
	{
		transform.localEulerAngles += m_circularSawRotate * speed * Time.deltaTime * Vector3.back;
	}

	// �ۂ̂��̈ʒu�ݒ�
	public void SetPosition(Vector3 miningPoint)
	{
		transform.position = miningPoint;
	}

	// �̌@�͈͐ݒ�
	public void SetRange(float range, float size)
	{
		// �͈͂̐ݒ�
		m_miningRange = range;

		// �X�P�[��
		float scale = Mathf.Max(size, 1.0f);

		// �ۂ̂��̃T�C�Y�ݒ�
		transform.localScale = Vector3.one * scale;


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
	// �̌@�p��Ray�擾
	public MiningRay GetMiningRay(Transform player)
	{
		// �v���C���[�̈ʒu����ۂ̂��̈ʒu�ւ̃x�N�g��
		Vector2 playerToCircular = transform.position - player.position;
		// �v���C���[����ۂ̂��܂ł̋���
		float length = playerToCircular.magnitude;
		// �x�N�g�����K��
		playerToCircular.Normalize();

		MiningRay miningRay = new()
		{
			direction = playerToCircular,
			origin = player.position,
			length = length,
		};

		return miningRay;
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
		rank = Mathf.Clamp(rank, 0, m_miningDatas[m_type].Upgrades.Length - 1);

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

		// �G�t�F�N�g�o��
		m_animator.transform.position = new Vector3(0.0f, transform.localScale.y / 2.0f, 0.0f);
		m_animator.SetTrigger("LevelUp");

		// �c�[���̃��x�����Z
		m_toolLevels[m_type] += level;

		// �f�ނ̏���
		m_playerItem.ConsumeMaterials(items);

		// �f�[�^�ۑ�
		SaveDataReadWrite.m_instance.MiningLevel = m_toolLevels;
		SaveDataReadWrite.m_instance.Items = m_playerItem.Items;
		//SaveDataReadWrite.m_instance.Save();
	}

	// �̂��̎�ސݒ�
	public void SetType(MiningData.MiningType type)
	{
		// ��ނ̐ݒ�
		m_type = type;
		// �X�v���C�g�̐ݒ�
		m_spriteRenderer.sprite = m_miningDatas[type].Sprite;

		//Debug.Log(type + "�ɕύX");
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
