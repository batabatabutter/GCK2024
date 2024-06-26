using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DA_", menuName = "CreateDataBase/Dungeon/Attack/AttackData")]
public class DungeonAttackData : ScriptableObject
{
	// 攻撃タイプ
	[System.Serializable]
	public enum AttackType
	{
		FALL_ROCK_01 = 0,
		FALL_ROCK_02,
		FALL_ROCK_03,
		FALL_ROCK_04,

		ROLL_ROCK_01 = 100,
		ROLL_ROCK_02,

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
		[Header("攻撃が最大になる距離(物理距離)"), Min(0.0f)]
		public float attackMaxDistance;
		[Header("攻撃段階の範囲"), Tooltip("攻撃間隔の時間に掛ける倍率")]
		public MyFunction.MinMaxFloat attackGradeRange;
		[Header("コアとの距離の取り方の種類")]
		public bool attackGradeStep;
		[Header("距離に応じた攻撃段階")]
		public List<AttackPower> attackGrade;
	}

	[Header("攻撃停止時間")]
	[SerializeField] private float stayTime = 10.0f;
	[Header("攻撃時間")]
	[SerializeField] private float attackTime = 10.0f;

	[Header("攻撃ランクの上限")]
	[SerializeField] private int rankLimit = 10;

	[Header("ランダムな攻撃")]
	[SerializeField] private bool isRandom = false;

	[Header("攻撃テーブル")]
	[SerializeField] private List<DungeonAttackTableData> attackTableList;
	[Header("攻撃テーブルの判定範囲")]
	[SerializeField] private float attackTableRange = 5.0f;

	[Header("攻撃段階")]
	[SerializeField] private AttackGrade m_attackGradeData;


	public float StayTime => stayTime;
	public float AttackTime => attackTime;
	public int RankLimit => rankLimit;
	public bool IsRandom => isRandom;
	public List<DungeonAttackTableData> AttackTableList => attackTableList;
	public float AttackTableRange => attackTableRange;
	public AttackGrade AttackGradeData => m_attackGradeData;

}
