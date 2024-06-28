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

    //[Header("ダンジョンの稼働状態")]
    //[SerializeField] private bool m_isActive = true;




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
    }

    // ダンジョンのサイズ取得
    public Vector2Int GetDungeonSize()
    {
        return m_generator.GetDungeonData().Size;
    }

    // ブロックの取得
    public Block[,] GetBlocks()
    {
        return m_generator.Blocks;
    }

    // ダンジョンの動きを止める
    public void Stop()
    {
		// アタッカーを止める
		m_attacker.enabled = false;
		// エネミーの生成を止める
		m_enemyGenerator.enabled = false;

	}

    // クリア時の呼び出し
    public void Clear()
    {
        // ダンジョンを止める
        Stop();

        // データ設定
        SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;
        // クリア設定
        saveData.SetClear(m_stageNum);
        // ダンジョンのレベルを上げる
        saveData.AddLevel(m_stageNum);
        // ブロック配置
        saveData.SetBlocks(m_generator.Blocks, m_stageNum);
        saveData.SaveBlocks(m_stageNum);

    }


    // プレイヤーのトランスフォーム設定
    public void SetTransform(Transform player)
    {
        // ジェネレータに設定(チャンク用)
        m_generator.SetPlayerTransform(player);
        // アタッカーに設定(攻撃段階用)
        m_attacker.Target = player;
    }

	// ダンジョンジェネレータ
	public DungeonGenerator DungeonGenerator
    {
        get { return m_generator; }
    }
    // コア
    public Block DungeonCore
    {
        get { return m_generator.DungeonCore; }
    }
    // プレイヤーの座標
    public Vector3 PlayerPosition
    {
        get { return m_generator.PlayerPosition; }
    }

}
