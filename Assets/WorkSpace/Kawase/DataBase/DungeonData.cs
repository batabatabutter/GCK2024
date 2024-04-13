using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "CreateDungeonData")]
public class DungeonData : ScriptableObject
{
    // �����p�^�[��
    [System.Serializable]
    public enum Pattern
    {
        TEN_X_TEN,      // 10 * 10
        DIGGING,        // ���@��
    }

    [Header("�_���W�����̐����p�^�[��")]
    public Pattern pattern;

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


    [Header("�_���W�����̑傫��(X:1 Y:1 = 10 * 10)")]
    public Vector2 dungeonSize;

    [Header("�_���W������CSV")]
    public List<TextAsset> dungeonCSV;

}
