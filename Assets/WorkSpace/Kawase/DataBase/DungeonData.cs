using System.Collections.Generic;
using UnityEngine;

public class DungeonData : ScriptableObject
{
    // 生成パターン
    [System.Serializable]
    public enum Pattern
    {
        TEN_X_TEN,      // 10 * 10
        DIGGING,        // 穴掘り

        OVER,
    }

    //  ダンジョンのウェーブデータ
    [System.Serializable]
    public struct DungeonWave
    {
        [Header("ダンジョンの行動パターン"), SerializeField]
        public List<DungeonAttack.AttackPattern> dungeonATKPattern;
        [Header("ダンジョンの行動間隔"), SerializeField]
        public float dungeonATKCoolTime;
        [Header("敵の種類"), SerializeField]
        public List<Enemy.Type> generateEnemyType;
        [Header("敵の生成数"), SerializeField]
        public int generateEnemyNum;
        [Header("敵の生成間隔"), SerializeField]
        public float geterateEnemyInterval;
    }

    [Header("ダンジョンの生成パターン")]
    [SerializeField] private Pattern pattern;
    [Header("ダンジョンのサイズ")]
	[SerializeField] private Vector2Int size;
    [Header("ダンジョンの生成確率")]
	[SerializeField] private List<DungeonGenerator.BlockOdds> dungeonOdds;
    [Header("ダンジョンの攻撃パターン")]
	[SerializeField] private List<DungeonAttack.AttackPattern> dungeonAttackPattern;
    [Header("ダンジョンの攻撃間隔")]
    [SerializeField] private float dungeonAttackCoolTime;
    [Header("敵")]
    [SerializeField] private List<Enemy.Type> enemy;
    [Header("敵の出現頻度")]
    [SerializeField] private float enemySpawnTime;
    [Header("ウェーブ情報")]
    [SerializeField] private List<DungeonWave> dungeonWaves;
    [Header("休憩ウェーブ時間")]
    [SerializeField] private float restWaveTime;

    public Pattern DungeonPattern => pattern;
    public Vector2Int Size => size;
    public List<DungeonGenerator.BlockOdds> BlockOdds => dungeonOdds;
    public List<DungeonAttack.AttackPattern > AttackPattern => dungeonAttackPattern;
    public float DungeonAttackCoolTime => dungeonAttackCoolTime;
    public List<Enemy.Type> Enemy => enemy;
    public float EnemySpawnTime => enemySpawnTime;
    public List<DungeonWave> DungeonWaves => dungeonWaves;
    public float RestWaveTime => restWaveTime;
}
