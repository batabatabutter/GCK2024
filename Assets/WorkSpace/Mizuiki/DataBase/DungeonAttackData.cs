using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DA_", menuName = "CreateDataBase/Dungeon/Attack/AttackData")]
public class DungeonAttackData : ScriptableObject
{
	// UŒ‚ƒ^ƒCƒv
	[System.Serializable]
	public enum AttackType
	{
		FALL_ROCK_01 = 0,
		FALL_ROCK_02,
		FALL_ROCK_03,
		FALL_ROCK_04,

		ROLL_ROCK_01 = 100,

		GROW_UP_ROCK_01 = 200,
		GROW_UP_ROCK_02,
		GROW_UP_ROCK_03,

		ENEMY_ALL_01 = 1000,

		ENEMY_DORI_01 = 1100,

		ENEMY_IWA_01 = 1200,

		ENEMY_HOTA_01 = 1300,

		ENEMY_BOM_01 = 1400,

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
		[Header("UŒ‚‚ªÅ‘å‚É‚È‚é‹——£(•¨—‹——£)"), Min(0.0f)]
		public float attackMaxDistance;
		[Header("UŒ‚’iŠK‚Ì”ÍˆÍ"), Tooltip("UŒ‚ŠÔŠu‚ÌŠÔ‚ÉŠ|‚¯‚é”{—¦")]
		public MyFunction.MinMaxFloat attackGradeRange;
		[Header("ƒRƒA‚Æ‚Ì‹——£‚Ìæ‚è•û‚Ìí—Ş")]
		public bool attackGradeStep;
		[Header("‹——£‚É‰‚¶‚½UŒ‚’iŠK")]
		public List<AttackPower> attackGrade;
	}

	[Header("UŒ‚’â~ŠÔ")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("UŒ‚ŠÔ")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("UŒ‚ƒ‰ƒ“ƒN‚ÌãŒÀ")]
	[SerializeField] private int rankLimit = 10;

	[Header("ƒ‰ƒ“ƒ_ƒ€‚ÈUŒ‚")]
	[SerializeField] private bool isRandom = false;

	[Header("UŒ‚ƒe[ƒuƒ‹")]
	[SerializeField] private List<DungeonAttackTableData> attackTableList;
	[Header("UŒ‚ƒe[ƒuƒ‹‚Ì”»’è”ÍˆÍ")]
	[SerializeField] private float attackTableRange = 5.0f;

	[Header("UŒ‚’iŠK")]
	[SerializeField] private AttackGrade m_attackGradeData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public int RankLimit => rankLimit;
	public bool IsRandom => isRandom;
	public List<DungeonAttackTableData> AttackTableList => attackTableList;
	public float AttackTableRange => attackTableRange;
	public AttackGrade AttackGradeData => m_attackGradeData;

}
