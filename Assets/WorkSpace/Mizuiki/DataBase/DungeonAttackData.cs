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
		public AttackType attackType;	// �U���̎��
		public float attackCoolTime;	// �U�������̃N�[���^�C��
		public int attackRank;			// �U�������N�ɉ�����������
		public float attackRange;		// �U���͈�
	}

	[Header("�U����~����")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("�U������")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("�����_���ȍU��")]
	[SerializeField] private bool isRandom = false;
	[Header("�U���p�^�[��")]
	[SerializeField] private AttackData[] attackData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public AttackData[] AttackPattern => attackData;

}
