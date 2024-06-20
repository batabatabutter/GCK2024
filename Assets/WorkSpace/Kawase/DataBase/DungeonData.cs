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
        MAZE,           // 迷路
        CAVE,           // 洞窟

        OVER,
    }

    //  ダンジョンのウェーブデータ
    [System.Serializable]
    public struct DungeonWave
    {
        //[Header("ダンジョンの行動パターン"), SerializeField]
        //public List<DungeonAttack.AttackPattern> dungeonATKPattern;
        //[Header("ダンジョンの行動間隔"), SerializeField]
        //public float dungeonATKCoolTime;
        [Header("敵の種類"), SerializeField]
        public List<Enemy.Type> generateEnemyType;
        [Header("敵の生成数"), SerializeField]
        public int generateEnemyNum;
        [Header("敵の生成間隔"), SerializeField]
        public float geterateEnemyInterval;
    }

    //  ダンジョンの休憩ウェーブデータ
    [System.Serializable]
    public struct DungeonRestTimeData
    {
        [Header("対応距離倍率")]
        [SerializeField] public float distanceRate;
        [Header("休憩時間の倍率")]
        [SerializeField] public float restTimeRate;
    }

    [Header("コアの画像")]
    [SerializeField] private Sprite coreSprite = null;
    [Header("ステージの色")]
    [SerializeField] private Color stageColor = Color.white;

	[Header("ダンジョンの生成パターン"), CustomEnum(typeof(Pattern))]
    [SerializeField] private string patternStr;
    private Pattern pattern;
    [Header("ダンジョンのサイズ")]
	[SerializeField] private Vector2Int size = new(50, 50);
    [Header("ダンジョンの生成確率")]
    [SerializeField] private DungeonGenerator.BlockGenerateData[] blockGenerateData;

    [Header("出現ツール情報")]

    [Header("ウェーブ情報")]
    [SerializeField] private List<DungeonWave> dungeonWaves;
    [Header("休憩ウェーブ時間")]
    [SerializeField] private float restWaveTime = 10.0f;
    [Header("休憩時間の倍率")]
    [SerializeField] private float restWaveMaxRate = 10.0f;
    [Header("休憩時間")]
    [SerializeField] private List<DungeonRestTimeData> restTimeData;

    [Header("攻撃の情報")]
    [SerializeField] private DungeonAttackData attackData;

    public Sprite CoreSprite => coreSprite;
    public Color StageColor => stageColor;
    public Pattern DungeonPattern => pattern;
    public Vector2Int Size => size;
    public DungeonGenerator.BlockGenerateData[] BlockGenerateData => blockGenerateData;
    public List<DungeonWave> DungeonWaves => dungeonWaves;
    public float RestWaveTime => restWaveTime;
    public float RestWaveMaxRate => restWaveMaxRate;
    public List<DungeonRestTimeData> RestTimeData => restTimeData;
    public DungeonAttackData AttackData => attackData;


	private void OnEnable()
	{
		pattern = SerializeUtil.Restore<Pattern>(patternStr);
	}

}
