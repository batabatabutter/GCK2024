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
		FALL_ROCK_01,
		FALL_ROCK_02,

		ROLL_ROCK_01,

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
		[Header("�R�A�Ƃ̋����̎����̎��")]
		public bool attackGradeStep;
		[Header("�U�����ő�ɂȂ鋗��")]
		public float attackMaxDistance;
		[Header("�U���i�K�͈̔�")]
		public MyFunction.MinMaxFloat attackGradeRange;
		[Header("�����ɉ������U���i�K")]
		public List<AttackPower> attackGrade;
	}

	// �U���e�[�u���ƍU���p�^�[��
	[System.Serializable]
	public struct AttackTable
	{
		public AttackTableType type;
		public List<DungeonAttackPattern> pattern;
	}

	[Header("�U����~����")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("�U������")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("�����_���ȍU��")]
	[SerializeField] private bool isRandom = false;
	[Header("�U���p�^�[��")]
	[SerializeField] private List<AttackTable> attackTableList;

	[Header("�U���i�K")]
	[SerializeField] private AttackGrade m_attackGradeData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public List<AttackTable> AttackTableList => attackTableList;
	public AttackGrade AttackGradeData => m_attackGradeData;

}
