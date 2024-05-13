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
		public AttackType type;	// �U���̎��
		public float coolTime;	// �U�������̃N�[���^�C��
		public float rankValue;		// �U�������N�ɉ�����������
		public float range;		// �U���͈�
	}

	[Header("�U����~����")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("�U������")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("�����_���ȍU��")]
	[SerializeField] private bool isRandom = false;
	[Header("�U���p�^�[��")]
	[SerializeField] private List<AttackData> attackData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public List<AttackData> AttackPattern => attackData;

}
