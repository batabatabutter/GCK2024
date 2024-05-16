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
        MAZE,           // ���H

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

    //  �_���W�����̋x�e�E�F�[�u�f�[�^
    [System.Serializable]
    public struct DungeonRestTimeData
    {
        [Header("�Ή������{��")]
        [SerializeField] public float distanceRate;
        [Header("�x�e���Ԃ̔{��")]
        [SerializeField] public float restTimeRate;
    }

	[Header("�_���W�����̐����p�^�[��")]
    [SerializeField] private Pattern pattern;
    [Header("�_���W�����̃T�C�Y")]
	[SerializeField] private Vector2Int size;
    [Header("�_���W�����̐����m��")]
    [SerializeField] private DungeonGenerator.BlockGenerateData[] blockGenerateData;

    [Header("�E�F�[�u���")]
    [SerializeField] private List<DungeonWave> dungeonWaves;
    [Header("�x�e�E�F�[�u����")]
    [SerializeField] private float restWaveTime;
    [Header("�x�e���Ԃ̔{��")]
    [SerializeField] private float restWaveMaxRate;
    [Header("�x�e����")]
    [SerializeField] private List<DungeonRestTimeData> restTimeData;

    public Pattern DungeonPattern => pattern;
    public Vector2Int Size => size;
    public DungeonGenerator.BlockGenerateData[] BlockGenerateData => blockGenerateData;
    public List<DungeonWave> DungeonWaves => dungeonWaves;
    public float RestWaveTime => restWaveTime;
    public float RestWaveMaxRate => restWaveMaxRate;
    public List<DungeonRestTimeData> RestTimeData => restTimeData;
}
