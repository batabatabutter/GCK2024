using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonAttackData;

[CreateAssetMenu(fileName = "AttackPattern_", menuName = "CreateDataBase/Dungeon/Attack/AttackPattern")]
public class DungeonAttackPattern : ScriptableObject
{
	// �ׂ��ȍU�����
	[System.Serializable]
	public struct AttackPattern
	{
		public AttackType type;                 // �U���̎��
		public MyFunction.Direction direction;  // �U�������̕���
		public float rankValue;                 // �U�������N�ɉ�����������
		public float range;                     // �U���͈�
		public float time;                      // �U����̃N�[���^�C��
	}

	[Header("�U�����̃��X�g")]
	[SerializeField] private List<AttackPattern> attackList;


	public List<AttackPattern> AttackList => attackList;
}
