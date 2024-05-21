using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonAttackData;

[CreateAssetMenu(fileName = "AttackPattern_", menuName = "CreateDataBase/Dungeon/Attack/AttackPattern")]
public class DungeonAttackPattern : ScriptableObject
{
	// ×‚©‚ÈUŒ‚î•ñ
	[System.Serializable]
	public struct AttackPattern
	{
		public AttackType type;                 // UŒ‚‚Ìí—Ş
		public MyFunction.Direction direction;  // UŒ‚”­¶‚Ì•ûŒü
		public float rankValue;                 // UŒ‚ƒ‰ƒ“ƒN‚É‰‚¶‚½‘‰Á—Ê
		public float range;                     // UŒ‚”ÍˆÍ
		public float time;                      // UŒ‚Œã‚ÌƒN[ƒ‹ƒ^ƒCƒ€
	}

	[Header("UŒ‚î•ñ‚ÌƒŠƒXƒg")]
	[SerializeField] private List<AttackPattern> attackList;


	public List<AttackPattern> AttackList => attackList;
}
