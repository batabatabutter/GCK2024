using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonAttackData;

[CreateAssetMenu(fileName = "AP_", menuName = "CreateDataBase/Dungeon/Attack/AttackPattern")]
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
		public float distance;					// �U����������
		public float time;                      // �U����̃N�[���^�C��
	}

	[Header("�U�����̃��X�g")]
	[SerializeField] private List<AttackPattern> attackList;

	[Header("����")]
	[SerializeField, TextArea(1, 3)] private string memo;

	public List<AttackPattern> AttackList => attackList;
}
