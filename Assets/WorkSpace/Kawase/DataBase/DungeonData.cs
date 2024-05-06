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
        public List<DungeonAttack.AttackPattern> dungeonATKPattern;
        [Header("�_���W�����̍s���Ԋu"), SerializeField]
        public float dungeonATKCoolTime;
        [Header("�G�̎��"), SerializeField]
        public List<Enemy.Type> generateEnemyType;
        [Header("�G�̐�����"), SerializeField]
        public int generateEnemyNum;
        [Header("�G�̐����Ԋu"), SerializeField]
        public float geterateEnemyInterval;
    }

	[Header("�_���W�����̐����p�^�[��")]
    [SerializeField] private Pattern pattern;
    [Header("�_���W�����̃T�C�Y")]
	[SerializeField] private Vector2Int size;
    [Header("�_���W�����̐����m��")]
	[SerializeField] private List<DungeonGenerator.BlockOdds> dungeonOdds;
    [SerializeField] private DungeonGenerator.BlockGenerateData[] blockGenerateData;
    [Header("�E�F�[�u���")]
    [SerializeField] private List<DungeonWave> dungeonWaves;
    [Header("�x�e�E�F�[�u����")]
    [SerializeField] private float restWaveTime;

    public Pattern DungeonPattern => pattern;
    public Vector2Int Size => size;
    public List<DungeonGenerator.BlockOdds> BlockOdds => dungeonOdds;
    public DungeonGenerator.BlockGenerateData[] BlockGenerateData => blockGenerateData;
    public List<DungeonWave> DungeonWaves => dungeonWaves;
    public float RestWaveTime => restWaveTime;
}
