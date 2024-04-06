using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "CreateDungeonData")]
public class DungeonData : ScriptableObject
{
    [Header("�_���W������CSV")]
    public List<TextAsset> dungeonCSV;

    [Header("�_���W�����̑傫��(X:1 Y:1 = 10 * 10)")]
    public Vector2 dungeonSize;

    [Header("�_���W�����̐����m��")]
    public List<DungeonGenerator.BlockOdds> dungeonOdds;

    [Header("�_���W�����̍U���p�^�[��")]
    public List<DungeonAttack.AttackPattern> dungeonAttackPattern;

    [Header("�_���W�����̍U���Ԋu")]
    public float dungeonAttackCoolTime;

    [Header("�G")]
    public List<Enemy.Type> enemy;

    [Header("�G�̏o���p�x")]
    public float enemySpawnTime;

}
