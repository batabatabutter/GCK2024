using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int WAVE_MAX_NUM;

    public enum WaveState
    {
        Attack,//攻撃
        Break,//休憩
    }

    //フェーズ管理
    private WaveState m_waveState;

    public WaveState waveState
    {
        get { return m_waveState; }
        set { m_waveState = value; }
    }

    //フェーズの番号
    private int m_waveNum;

    public int WaveNum
    {
        get { return m_waveNum; }
        set { m_waveNum = value; }
    }


    [Header("ダンジョンデータベース")]
    [SerializeField] private DungeonDataBase m_DungeonDataBase;

    //ウェーブの経過時間管理
    private float m_waveTime;
    //ダンジョンデータ
    private DungeonData m_dungeonDatas;
    //ステージ番号
    private int m_stageNum;


    void Start()
    {
        //ステージ番号の取得
        m_stageNum = GetComponent<PlaySceneManager>().StageNum;
        //ダンジョンのデータ取得
        m_dungeonDatas = m_DungeonDataBase.dungeonDatas[m_stageNum];
        //最初は休憩フェーズから
        m_waveState = WaveState.Break;

        //休憩時間の取得
        m_waveTime = m_dungeonDatas.RestWaveTime;
        //フェーズ０
        m_waveNum = 0;

        //ウェーブの上限値
        WAVE_MAX_NUM = m_dungeonDatas.DungeonWaves.Count;
    }

    // Update is called once per frame
    void Update()
    {

        //休憩時間
        if(m_waveTime < 0)
        {
            //ウェーブ切り替え
            // 最初の要素を取得する方法
            m_waveState = WaveState.Attack;
            //休憩時間の取得
            m_waveTime = m_dungeonDatas.RestWaveTime;

        }
        //時間があり休憩状態だったら
        else if(m_waveTime > 0 && m_waveState == WaveState.Break)
        {
            //ウェーブ時間の進行
            m_waveTime -= Time.deltaTime;
        }


        // ここの処理をエネミージェネレーターに組み込む
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    //休憩フェーズ二に移行
        //    m_waveState = WaveState.Break;
        //    m_waveNum++;
        //}
        //if (Time.frameCount % 60 == 0) // 60FPS
        //{
        //    Debug.Log("状態：" + m_waveState);
        //    Debug.Log("ウェーブ番号：" + m_waveNum);
        //}
        //Debug.Log("ウェーブ上限：" + (WAVE_MAX_NUM - 1));
    }

}
