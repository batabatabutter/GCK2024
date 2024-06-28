using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningData_", menuName = "CreateDataBase/Mining/MiningData")]
public class MiningData : ScriptableObject
{
	// �̌@�c�[���̎��
	[System.Serializable]
	public enum MiningType
	{
		BALANCE,	// �o�����X�^
		RANGE,      // �͈͌^
		POWER,      // �p���[�^
		SPEED,      // �X�s�[�h�^
		CRITICAL,   // �N���e�B�J���^
		DROP,       // �h���b�v�^

		OVER,
	}


	// �̌@�Ɏg�p����l
	[System.Serializable]
	public class MiningValue
	{
		[Tooltip("�̌@�͈�(���a)")]
		public float range = 3.0f;
		[Tooltip("�̌@�T�C�Y(���a)")]
		public float size = 0.0f;
		[Tooltip("�̌@��")]
		public float power = 50.0f;
		[Tooltip("�̌@���x(/s)")]
		public float speed = 2.0f;
		[Tooltip("�N���e�B�J����(%)")]
		public float critical = 0.0f;
		[Tooltip("�N���e�B�J���_���[�W(%)")]
		public float criticalDamage = 200.0f;
		[Tooltip("�A�C�e���h���b�v��(%)")]
		public float itemDrop = 100.0f;

		public static MiningValue operator +(MiningValue left, MiningValue right)   // �a
		{
			MiningValue val = new()
			{
				range = left.range + right.range,
				size = left.size + right.size,
				power = left.power + right.power,
				speed = left.speed + right.speed,
				critical = left.critical + right.critical,
				criticalDamage = left.criticalDamage + right.criticalDamage,
				itemDrop = left.itemDrop + right.itemDrop,
			};
			return val;
		}
		public static MiningValue operator -(MiningValue left, MiningValue right)   // ��
		{
			MiningValue val = new()
			{
				range = left.range - right.range,
				size = left.size - right.size,
				power = left.power - right.power,
				speed = left.speed - right.speed,
				critical = left.critical - right.critical,
				criticalDamage = left.criticalDamage - right.criticalDamage,
				itemDrop = left.itemDrop - right.itemDrop,
			};
			return val;
		}
		public static MiningValue operator *(MiningValue left, MiningValue right)   // ��
		{
			MiningValue val = new()
			{
				range = left.range * right.range,
				size = left.size * right.size,
				power = left.power * right.power,
				speed = left.speed * right.speed,
				critical = left.critical * right.critical,
				criticalDamage = left.criticalDamage * right.criticalDamage,
				itemDrop = left.itemDrop * right.itemDrop
			};
			return val;
		}
		public static MiningValue operator *(MiningValue value, float product)
		{
			MiningValue val = new()
			{
				range = value.range * product,
				size = value.size * product,
				power = value.power * product,
				speed = value.speed * product,
				critical = value.critical * product,
				criticalDamage = value.criticalDamage * product,
				itemDrop = value.itemDrop * product
			};
			return val;
		}

		public static MiningValue Set(float num)
		{
			MiningValue val = new()
			{
				range = num,
				size = num,
				power = num,
				speed = num,
				critical = num,
				criticalDamage = num,
				itemDrop = num,
			};
			return val;
		}
		public static MiningValue Zero()
		{
			return Set(0.0f);
		}
	}

	[Header("�c�[���̎��")]
	[SerializeField, CustomEnum(typeof(MiningType))] private string typeStr;
	private MiningType type;

	[Header("�ۂ̂��̉摜")]
	[SerializeField] private Sprite sprite = null;

	[Header("�̌@�l")]
	[SerializeField] private MiningValue miningValue;

	[Header("������")]
	[SerializeField] private UpgradeData[] upgradeData;



	public MiningType Type => type;
	public Sprite Sprite => sprite;
	public MiningValue Value => miningValue;
	public UpgradeData[] Upgrades => upgradeData;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<MiningType>(typeStr);
	}

}
