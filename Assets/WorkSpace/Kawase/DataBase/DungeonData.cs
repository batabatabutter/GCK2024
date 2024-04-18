using System.Collections.Generic;
using UnityEngine;

public class DungeonData : ScriptableObject
{
    // �����p�^�[��
    [System.Serializable]
    public enum Pattern
    {
        TEN_X_TEN,      // 10 * 10
        DIGGING,        // ���@��

        OVER,
    }

    //  �_���W�����̃E�F�[�u�f�[�^
    [System.Serializable]
    public struct DungeonWave
    {
        [Header("�_���W�����̍s���p�^�[��"), SerializeField]
        List<DungeonAttack.AttackPattern> dungeonATKPattern;
        [Header("�_���W�����̍s���Ԋu"), SerializeField]
        float dungeonATKCoolTime;
        [Header("�G�̎��"), SerializeField]
        List<Enemy.Type> generateEnemyType;
        [Header("�G�̐�����"), SerializeField]
        int generateEnemyNum;
        [Header("�G�̐����Ԋu"), SerializeField]
        float geterateEnemyInterval;
    }

    [Header("�_���W�����̐����p�^�[��")]
    [SerializeField] private Pattern pattern;
    [Header("�_���W�����̃T�C�Y")]
	[SerializeField] private Vector2Int size;
    [Header("�_���W�����̐����m��")]
	[SerializeField] private List<DungeonGenerator.BlockOdds> dungeonOdds;
    [Header("�_���W�����̍U���p�^�[��")]
	[SerializeField] private List<DungeonAttack.AttackPattern> dungeonAttackPattern;
    [Header("�_���W�����̍U���Ԋu")]
    [SerializeField] private float dungeonAttackCoolTime;
    [Header("�G")]
    [SerializeField] private List<Enemy.Type> enemy;
    [Header("�G�̏o���p�x")]
    [SerializeField] private float enemySpawnTime;
    [Header("�E�F�[�u���")]
    [SerializeField] private List<DungeonWave> dungeonWaves;

    public Pattern DungeonPattern => pattern;
    public Vector2Int Size => size;
    public List<DungeonGenerator.BlockOdds> BlockOdds => dungeonOdds;
    public List<DungeonAttack.AttackPattern > AttackPattern => dungeonAttackPattern;
    public float DungeonAttackCoolTime => dungeonAttackCoolTime;
    public List<Enemy.Type> Enemy => enemy;
    public float EnemySpawnTime => enemySpawnTime;
    public List<DungeonWave> DungeonWaves => dungeonWaves;
}
