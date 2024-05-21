using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DungeonAttackData_", menuName = "CreateDataBase/Dungeon/Attack/AttackData")]
public class DungeonAttackData : ScriptableObject
{
	// UŒ‚ƒe[ƒuƒ‹
	[System.Serializable]
	public enum AttackTableType
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

	// ‹——£‚É‰‚¶‚½UŒ‚’iŠK
	[System.Serializable, Tooltip("distance ¸‡‚Éƒ\[ƒg")]
	public struct AttackPower
	{
		[Tooltip("ŠJn’n“_‚Ì distance ”{‚Ì‚Æ‚«(¬‚³‚¢‚Ù‚ÇƒRƒA‚É‹ß‚¢)")]
		public float distance;
		[Tooltip("UŒ‚‚Ì”­¶”{—¦(‘å‚«‚¢‚Ù‚Ç‚½‚­‚³‚ñUŒ‚‚ª—ˆ‚é)")]
		public float magnification;
	}
	// UŒ‚’iŠK‚Ìƒf[ƒ^
	[System.Serializable]
	public struct AttackGrade
	{
		[Header("ƒRƒA‚Æ‚Ì‹——£‚Ìæ‚è•û‚Ìí—Ş")]
		public bool attackGradeStep;
		[Header("UŒ‚‚ªÅ‘å‚É‚È‚é‹——£")]
		public float attackMaxDistance;
		[Header("UŒ‚’iŠK‚Ì”ÍˆÍ")]
		public MyFunction.MinMaxFloat attackGradeRange;
		[Header("‹——£‚É‰‚¶‚½UŒ‚’iŠK")]
		public List<AttackPower> attackGrade;
	}

	// UŒ‚ƒe[ƒuƒ‹‚ÆUŒ‚ƒpƒ^[ƒ“
	[System.Serializable]
	public struct AttackTable
	{
		public AttackTableType type;
		public List<DungeonAttackPattern> pattern;
	}

	[Header("UŒ‚’â~ŠÔ")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("UŒ‚ŠÔ")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("ƒ‰ƒ“ƒ_ƒ€‚ÈUŒ‚")]
	[SerializeField] private bool isRandom = false;
	[Header("UŒ‚ƒpƒ^[ƒ“")]
	[SerializeField] private List<AttackTable> attackTableList;

	[Header("UŒ‚’iŠK")]
	[SerializeField] private AttackGrade m_attackGradeData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public List<AttackTable> AttackTableList => attackTableList;
	public AttackGrade AttackGradeData => m_attackGradeData;

}
