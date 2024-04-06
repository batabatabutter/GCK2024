using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "CreateDungeonData")]
public class DungeonData : ScriptableObject
{
    [Header("ダンジョンのCSV")]
    public List<TextAsset> dungeonCSV;

    [Header("ダンジョンの大きさ(X:1 Y:1 = 10 * 10)")]
    public Vector2 dungeonSize;

    [Header("ダンジョンの生成確率")]
    public List<DungeonGenerator.BlockOdds> dungeonOdds;

    [Header("ダンジョンの攻撃パターン")]
    public List<DungeonAttack.AttackPattern> dungeonAttackPattern;

    [Header("ダンジョンの攻撃間隔")]
    public float dungeonAttackCoolTime;

    [Header("敵")]
    public List<Enemy.Type> enemy;

    [Header("敵の出現頻度")]
    public float enemySpawnTime;

}
