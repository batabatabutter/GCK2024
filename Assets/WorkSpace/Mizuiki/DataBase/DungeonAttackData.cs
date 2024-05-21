using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DungeonAttackData_", menuName = "CreateDataBase/Dungeon/Attack/AttackData")]
public class DungeonAttackData : ScriptableObject
{
	// 攻撃テーブル
	[System.Serializable]
	public enum AttackTableType
	{
		FILL,
		CAVITY,

		OVER,
	}
	// 攻撃タイプ
	[System.Serializable]
	public enum AttackType
	{
		FALL_ROCK_01,
		FALL_ROCK_02,

		ROLL_ROCK_01,

		OVER,
	}

	// 距離に応じた攻撃段階
	[System.Serializable, Tooltip("distance 昇順にソート")]
	public struct AttackPower
	{
		[Tooltip("開始地点の distance 倍のとき(小さいほどコアに近い)")]
		public float distance;
		[Tooltip("攻撃の発生倍率(大きいほどたくさん攻撃が来る)")]
		public float magnification;
	}
	// 攻撃段階のデータ
	[System.Serializable]
	public struct AttackGrade
	{
		[Header("コアとの距離の取り方の種類")]
		public bool attackGradeStep;
		[Header("攻撃が最大になる距離")]
		public float attackMaxDistance;
		[Header("攻撃段階の範囲")]
		public MyFunction.MinMaxFloat attackGradeRange;
		[Header("距離に応じた攻撃段階")]
		public List<AttackPower> attackGrade;
	}

	// 攻撃テーブルと攻撃パターン
	[System.Serializable]
	public struct AttackTable
	{
		public AttackTableType type;
		public List<DungeonAttackPattern> pattern;
	}

	[Header("攻撃停止時間")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("攻撃時間")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("ランダムな攻撃")]
	[SerializeField] private bool isRandom = false;
	[Header("攻撃パターン")]
	[SerializeField] private List<AttackTable> attackTableList;

	[Header("攻撃段階")]
	[SerializeField] private AttackGrade m_attackGradeData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public bool IsRandom => isRandom;
	public List<AttackTable> AttackTableList => attackTableList;
	public AttackGrade AttackGradeData => m_attackGradeData;

}
