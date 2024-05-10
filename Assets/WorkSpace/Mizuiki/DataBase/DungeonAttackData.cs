using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DungeonAttackData_", menuName = "CreateDataBase/Dungeon/Attack")]
public class DungeonAttackData : ScriptableObject
{
	[System.Serializable]
	public enum AttackType
	{
		FALL_ROCK_01,
		FALL_ROCK_02,

		OVER,
	}

	[System.Serializable]
	public struct AttackData
	{
		public AttackType attackType;	// UŒ‚‚ÌŽí—Þ
		public float attackCoolTime;	// UŒ‚”­¶‚ÌƒN[ƒ‹ƒ^ƒCƒ€
		public int attackRank;			// UŒ‚ƒ‰ƒ“ƒN‚É‰ž‚¶‚½‘‰Á—Ê
		public float attackRange;		// UŒ‚”ÍˆÍ
	}

	[Header("UŒ‚’âŽ~ŽžŠÔ")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("UŒ‚ŽžŠÔ")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("ƒ‰ƒ“ƒ_ƒ€‚ÈUŒ‚")]
	[SerializeField] private bool isRandom = false;
	[Header("UŒ‚ƒpƒ^[ƒ“")]
	[SerializeField] private AttackData[] attackData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public AttackData[] AttackPattern => attackData;

}
