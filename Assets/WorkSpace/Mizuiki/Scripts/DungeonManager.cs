using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [Header("プレイシーンマネージャ")]
    [SerializeField] private PlaySceneManager m_playSceneManager = null;

    [Header("ジェネレータ")]
    [SerializeField] private DungeonGenerator m_generator = null;

    [Header("アタッカー")]
    [SerializeField] private DungeonAttacker m_attacker = null;

    [Header("エネミー")]
    [SerializeField] private EnemyGenerator m_enemyGenerator = null;



    // ダンジョン生成
    public void GenerateDungeon(int stageNum)
    {
        m_generator.CreateStage(stageNum);
    }

    // シーンマネージャ設定
    public void SetSceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

        // 子の設定
        m_generator.PlaySceneManager = playSceneManager;
    }

    // クリア時の呼び出し
    public void Clear()
    {
        m_attacker.enabled = false;
        m_enemyGenerator.enabled = false;
    }

}
