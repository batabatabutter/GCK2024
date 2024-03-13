using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class DungeonGenerator : MonoBehaviour
{
    [Header("明るさをつける(デバッグ)")]
    [SerializeField] private bool m_isBrightness;


    [Header("生成するダンジョンのパス")]
    [SerializeField] private List<string> m_dungeonPath;

	[Header("ダンジョンのサイズ(10*10で１サイズ)")]
    [SerializeField] private int m_dungeonSizeX;
    [SerializeField] private int m_dungeonSizeY;

    [Header("核ブロック")]
    [SerializeField] private　GameObject m_blockCore;

    [Header("生成ブロック")]
    [SerializeField] private List<GameObject> m_block = null;
    [Header("生成ブロックの確率％（０～１００）（上のと同じ順番ね）")]
    [SerializeField] private List<int> m_blockOdds = null;

    [Header("何の変哲もないブロック")]
    //[SerializeField] private string m_blockNormalNum = "1";
    [Header("岩盤ブロック")]
    [SerializeField] private string m_blockBedrockNum = "2";
    [Header("ダンジョンの核")]
    [SerializeField] private string m_blockCoreNum = "3";

    [Header("核からプレイヤーの出現しない距離")]
    [SerializeField] private int m_playerLength = 35;
    [Header("プレイヤー")]
    [SerializeField] private GameObject m_player;
    [Header("プレイシーンマネージャー")]
    [SerializeField] private PlaySceneManager m_playSceneManager;


    private int m_corePosX;
    private int m_corePosY;

    private Vector2 m_playerPos;



    private void Awake()
    {
        m_corePosX = Random.Range(0, m_dungeonSizeX * 10);
        m_corePosY = Random.Range(0, m_dungeonSizeY * 10);

        //プレイヤーとコアの位置が離れるまで繰り返す
        do
        {
            m_playerPos = new Vector2(Random.Range(0, m_dungeonSizeX * 10), Random.Range(0, m_dungeonSizeY * 10));

        }
        while (
        m_playerPos.x < m_corePosX + m_playerLength &&
        m_playerPos.x > m_corePosX - m_playerLength &&
        m_playerPos.y < m_corePosY + m_playerLength &&
        m_playerPos.y > m_corePosY - m_playerLength
        );

        //  プレイヤーの生成
        GameObject pl = Instantiate<GameObject>(m_player, m_playerPos, Quaternion.identity);
        //  プレイシーンマネージャーが無かったら格納しない
        if (m_playSceneManager == null)
            Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:DungeonManager");
        else
        {
            m_playSceneManager.SetPlayer(pl);
            m_playSceneManager.AddCore(m_blockCore);
        }
    }


    // Start is called before the first frame update
    void Start()
    {





        //１０＊１０のリスト管理用
        List<List<List<string>>> mapListManager = new List<List<List<string>>>();


        for (int i = 0; i < m_dungeonPath.Count; i++)
        {
            // ファイルがなければマップ読み込みの処理をしない
            if (!File.Exists(m_dungeonPath[i]))
            {
                Debug.Log(m_dungeonPath[i]);
                return;

            }

            // マップのリスト
            List<List<string>> mapList = new List<List<string>>();

            // ファイル読み込み
            StreamReader streamReader = new StreamReader(m_dungeonPath[i]);

            // 改行区切りで読み出す
            foreach (string line in streamReader.ReadToEnd().Split("\n"))
            {
                // 行が存在しなければループを抜ける
                if (line == "")
                    break;

                string lin = line.Remove(line.Length - 1);

                List<string> list = new List<string>();

                // カンマ区切りで読み出す
                foreach (string line2 in lin.Split(","))
                {
                    list.Add(line2);
                }

                mapList.Add(list);
            }
            //３次元に入れる
            mapListManager.Add(mapList);

            // ファイルを閉じる
            streamReader.Close();

        }

        //ブロック生成
        for (int y = 0; y < m_dungeonSizeY;y++)
        {
            for(int x = 0; x < m_dungeonSizeX;x++)
            {
                int random = Random.Range(0, m_dungeonPath.Count);
                Make10_10Block(mapListManager[random], x * 10, y * 10);

            }
        }




    }

    // Update is called once per frame
    void Update()
    {
    }


    void Make10_10Block(List<List<string>> mapList,int originX, int originY)
    {
        // 読みだしたデータをもとにダンジョン生成をする
        for (int y = 0; y < mapList.Count; y++)
        {
            for (int x = 0; x < mapList[y].Count; x++)
            {
                if((int)m_playerPos.x == x + originX && (int)m_playerPos.y == y + originY)
                {
                    continue;
                }

                //コアを生成
                if(m_corePosX == x + originX && m_corePosY == y + originY)
                {
                    MakeCore();

                    continue;
                }
                // 0 の場合は何も生成しない
                if (mapList[y][x] == "0" || mapList[y][x] == "")
                    continue;

                // 生成座標
                Vector3 pos = new(originX + x,originY + y, 0.0f);

                // ブロックの生成
                GameObject block = Instantiate<GameObject>(m_block[LotteryBlock(m_blockOdds)], pos, Quaternion.identity);

                // ブロックスクリプトをつける
                block.AddComponent<Block>();

                // 破壊不可能ブロックにする
                if (mapList[y][x] == m_blockBedrockNum)
                {
                    // 分かりやすいようにとりあえず色を変える
                    block.GetComponent<SpriteRenderer>().color = Color.gray;
                    // 破壊不可能にする
                    block.GetComponent<Block>().DontBroken = true;
                }

                // 核にする
                if (mapList[y][x] == m_blockCoreNum)
                {
                    // 分かりやすいようにとりあえず色を変える
                    block.GetComponent<SpriteRenderer>().color = Color.red;
                    // ダンジョンスクリプトをつける
                    block.AddComponent<Dungeon>();
                }

                //明るさの追加
                if(m_isBrightness)
                    block.AddComponent<ChangeBrightness>();
            }
        }
    }


    int LotteryBlock(List<int> blockOddsList)
    {
        //確率の抽選
        List<int> oddsList = new List<int>();

        int allOdds = 0;

        for (int i = 0; i < blockOddsList.Count; i++)
        {
            for (int j = 0; j < blockOddsList[i]; j++)
            {
                oddsList.Add(i);
            }
            allOdds += blockOddsList[i];
        }

        return oddsList[Random.Range(0,allOdds)];
    }

    private void MakeCore()
    {
        // 生成座標
        Vector3 pos = new(m_corePosX, m_corePosY, 0.0f);

        // ブロックの生成
        GameObject block = Instantiate<GameObject>(m_blockCore, pos, Quaternion.identity);

        // ブロックスクリプトをつける
        block.AddComponent<Block>();

        //明るさの追加
        if (m_isBrightness)
            block.AddComponent<ChangeBrightness>();

    }
}
