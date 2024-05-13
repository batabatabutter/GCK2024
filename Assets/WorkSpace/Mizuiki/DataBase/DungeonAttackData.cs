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

		ROLL_ROC_01,

		OVER,
	}

	[System.Serializable]
	public struct AttackData
	{
		public AttackType type;	// UŒ‚‚Ìí—Ş
		public float coolTime;	// UŒ‚”­¶‚ÌƒN[ƒ‹ƒ^ƒCƒ€
		public float rankValue;		// UŒ‚ƒ‰ƒ“ƒN‚É‰‚¶‚½‘‰Á—Ê
		public float range;		// UŒ‚”ÍˆÍ
	}

	[Header("UŒ‚’â~ŠÔ")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("UŒ‚ŠÔ")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("ƒ‰ƒ“ƒ_ƒ€‚ÈUŒ‚")]
	[SerializeField] private bool isRandom = false;
	[Header("UŒ‚ƒpƒ^[ƒ“")]
	[SerializeField] private List<AttackData> attackData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public List<AttackData> AttackPattern => attackData;

}
