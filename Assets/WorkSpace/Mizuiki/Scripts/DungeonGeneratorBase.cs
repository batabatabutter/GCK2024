using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorBase : MonoBehaviour
{
    [Header("ダンジョンの生成パターン")]
	[SerializeField] private DungeonData.Pattern m_pattern;

	public virtual List<List<string>> GenerateDungeon(DungeonData dungeonData)
    {
        return new();
    }
    public virtual List<List<string>> GenerateDungeon(Vector2Int size)
    {
        return new();
    }

    // 生成パターン
    public DungeonData.Pattern Pattern
    {
        get { return m_pattern; }
    }
}
