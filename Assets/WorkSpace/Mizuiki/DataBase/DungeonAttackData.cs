using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DungeonAttackData_", menuName = "CreateDataBase/Dungeon/Attack")]
public class DungeonAttackData : ScriptableObject
{
	// UŒ‚ƒe[ƒuƒ‹
	[System.Serializable]
	public enum AttackTablePattern
	{
		FILL,
		CAVITY,

		OVER,
	}
	// UŒ‚ƒ^ƒCƒv
	[System.Serializable]
	public enum AttackType
	{
		FALL_ROCK_01,
		FALL_ROCK_02,

		ROLL_ROCK_01,

		OVER,
	}

	[System.Serializable]
	public struct AttackList
	{
		public MyFunction.Direction direction;	// UŒ‚”­¶‚Ì•ûŒü
		public float range;						// UŒ‚”ÍˆÍ
		public float time;						// UŒ‚Œã‚ÌƒN[ƒ‹ƒ^ƒCƒ€
	}
	[System.Serializable]
	public struct AttackPattern
	{
		public AttackType type;					// UŒ‚‚Ìí—Ş
		public float rankValue;					// UŒ‚ƒ‰ƒ“ƒN‚É‰‚¶‚½‘‰Á—Ê
		public List<AttackList> attackList;		// ×‚©‚ÈUŒ‚‡
	}

	[Header("UŒ‚’â~ŠÔ")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("UŒ‚ŠÔ")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("ƒ‰ƒ“ƒ_ƒ€‚ÈUŒ‚")]
	[SerializeField] private bool isRandom = false;
	[Header("UŒ‚ƒpƒ^[ƒ“")]
	[SerializeField] private List<AttackPattern> attackPattern;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public List<AttackPattern> AttackPatternList => attackPattern;

}
