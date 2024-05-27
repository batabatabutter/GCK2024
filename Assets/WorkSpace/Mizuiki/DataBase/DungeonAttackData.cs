using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DungeonAttackData_", menuName = "CreateDataBase/Dungeon/Attack/AttackData")]
public class DungeonAttackData : ScriptableObject
{
	// �U���e�[�u��
	[System.Serializable]
	public enum AttackTableType
	{
		FILL,
		CAVITY,

		OVER,
	}
	// �U���^�C�v
	[System.Serializable]
	public enum AttackType
	{
		FALL_ROCK_01 = 0,
		FALL_ROCK_02,
		FALL_ROCK_03,

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

	// �����ɉ������U���i�K
	[System.Serializable, Tooltip("distance �����Ƀ\�[�g")]
	public struct AttackPower
	{
		[Tooltip("�J�n�n�_�� distance �{�̂Ƃ�(�������قǃR�A�ɋ߂�)")]
		public float distance;
		[Tooltip("�U���̔����{��(�傫���قǂ�������U��������)")]
		public float magnification;
	}
	// �U���i�K�̃f�[�^
	[System.Serializable]
	public struct AttackGrade
	{
		[Header("�U�����ő�ɂȂ鋗��")]
		public float attackMaxDistance;
		[Header("�U���i�K�͈̔�")]
		public MyFunction.MinMaxFloat attackGradeRange;
		[Header("�R�A�Ƃ̋����̎����̎��")]
		public bool attackGradeStep;
		[Header("�����ɉ������U���i�K")]
		public List<AttackPower> attackGrade;
	}

	// �U���e�[�u���ƍU���p�^�[��
	[System.Serializable]
	public struct AttackTable
	{
		public AttackTableType type;
		public float overNum;
		public List<DungeonAttackPattern> pattern;
	}

	[Header("�U����~����")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("�U������")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("�U�������N�̏��")]
	[SerializeField] private int rankLimit = 10;

	[Header("�����_���ȍU��")]
	[SerializeField] private bool isRandom = false;

	[Header("�U���e�[�u��")]
	[SerializeField] private List<AttackTable> attackTableList;
	[Header("�U���e�[�u���̔���͈�")]
	[SerializeField] private float attackTableRange = 5.0f;

	[Header("�U���i�K")]
	[SerializeField] private AttackGrade m_attackGradeData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public int RankLimit => rankLimit;
	public bool IsRandom => isRandom;
	public List<AttackTable> AttackTableList => attackTableList;
	public float AttackTableRange => attackTableRange;
	//public float ThresholdValueRate => thresholdValueRate;
	public AttackGrade AttackGradeData => m_attackGradeData;

}
