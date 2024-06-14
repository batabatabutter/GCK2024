using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningData_", menuName = "CreateDataBase/Mining/MiningData")]
public class MiningData : ScriptableObject
{
	// 採掘ツールの種類
	[System.Serializable]
	public enum MiningType
	{
		BALANCE,	// バランス型
		RANGE,      // 範囲型
		POWER,      // パワー型
		SPEED,      // スピード型
		CRITICAL,   // クリティカル型
		DROP,       // ドロップ型

		OVER,
	}


	// 採掘に使用する値
	[System.Serializable]
	public class MiningValue
	{
		[Tooltip("採掘範囲(半径)")]
		public float range = 3.0f;
		[Tooltip("採掘サイズ(半径)")]
		public float size = 0.0f;
		[Tooltip("採掘力")]
		public float power = 50.0f;
		[Tooltip("採掘速度(/s)")]
		public float speed = 2.0f;
		[Tooltip("クリティカル率(%)")]
		public float critical = 0.0f;
		[Tooltip("クリティカルダメージ(%)")]
		public float criticalDamage = 200.0f;
		[Tooltip("アイテムドロップ率(%)")]
		public float itemDrop = 100.0f;

		public static MiningValue operator +(MiningValue left, MiningValue right)   // 和
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
		public static MiningValue operator -(MiningValue left, MiningValue right)   // 差
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
		public static MiningValue operator *(MiningValue left, MiningValue right)   // 積
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

	[Header("ツールの種類")]
	[SerializeField, CustomEnum(typeof(MiningType))] private string typeStr;
	private MiningType type;

	[Header("丸のこの画像")]
	[SerializeField] private Sprite sprite = null;

	[Header("採掘値")]
	[SerializeField] private MiningValue miningValue;

	[Header("増加量")]
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
