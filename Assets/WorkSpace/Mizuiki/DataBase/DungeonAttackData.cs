using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DungeonAttackData_", menuName = "CreateDataBase/Dungeon/Attack")]
public class DungeonAttackData : ScriptableObject
{
	// �U���e�[�u��
	[System.Serializable]
	public enum AttackTablePattern
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

	[System.Serializable]
	public struct AttackList
	{
		public MyFunction.Direction direction;	// �U�������̕���
		public float range;						// �U���͈�
		public float time;						// �U����̃N�[���^�C��
	}
	[System.Serializable]
	public struct AttackPattern
	{
		public AttackType type;					// �U���̎��
		public float rankValue;					// �U�������N�ɉ�����������
		public List<AttackList> attackList;		// �ׂ��ȍU����
	}

	[Header("�U����~����")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("�U������")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("�����_���ȍU��")]
	[SerializeField] private bool isRandom = false;
	[Header("�U���p�^�[��")]
	[SerializeField] private List<AttackPattern> attackPattern;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public List<AttackPattern> AttackPatternList => attackPattern;

}
