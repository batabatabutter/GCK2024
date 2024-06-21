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

    [Header("ダンジョン番号")]
    [SerializeField] private int m_stageNum = 0;


    // ダンジョン生成
    public void GenerateDungeon(int stageNum)
    {
        // ステージ番号設定
        m_stageNum = stageNum;

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
        // アタッカーを止める
        m_attacker.enabled = false;
        // エネミーの生成を止める
        m_enemyGenerator.enabled = false;

        // データ設定
        SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;
        // クリア設定
        saveData.SetClear(m_stageNum);
        // ブロック配置
        saveData.SetBlocks(m_generator.Blocks, m_stageNum);
        saveData.SaveBlocks(m_stageNum);

    }

}
