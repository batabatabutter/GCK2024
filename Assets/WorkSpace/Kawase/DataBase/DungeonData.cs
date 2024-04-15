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

    [Header("�_���W�����̐����p�^�[��")]
    [SerializeField] private Pattern pattern;
    [Header("�_���W�����̃T�C�Y")]
	[SerializeField] private Vector2Int size = new (50, 50);
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

    public Pattern DungeonPattern => pattern;
    public Vector2Int Size => size;
    public List<DungeonGenerator.BlockOdds> BlockOdds => dungeonOdds;
    public List<DungeonAttack.AttackPattern > AttackPattern => dungeonAttackPattern;
    public float DungeonAttackCoolTime => dungeonAttackCoolTime;
    public List<Enemy.Type> Enemy => enemy;
    public float EnemySpawnTime => enemySpawnTime;

}
