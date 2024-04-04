using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.UI.Image;

public class DungeonGenerator : MonoBehaviour
{
    [Header("明るさをつける(デバッグ)")]
    [SerializeField] private bool m_isBrightness;

    [Header("生成するダンジョンのCSVファイル")]
    [SerializeField] private List<TextAsset> m_dungeonPath;

	[Header("ダンジョンのサイズ(10*10で１サイズ)")]
    [SerializeField] private int m_dungeonSizeX;
    [SerializeField] private int m_dungeonSizeY;

    [System.Serializable]
    public class BlockOdds
    {
        [Header("種類")]
        public BlockData.BlockType type;       // 種類
        [Header("確率")]
        public int odds;       // 確率
    }

    [SerializeField]
    List<BlockOdds> m_blockOddsList;

    [Header("核からプレイヤーの出現しない距離")]
    [SerializeField] private int m_playerLength = 35;
    [Header("プレイヤー")]
    [SerializeField] private GameObject m_player;
    [Header("プレイシーンマネージャー")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    [Header("地面背景")]
    [SerializeField] private GameObject m_ground;


    private int m_corePosX;
    private int m_corePosY;

    private Vector2 m_playerPos;

    private GameObject m_parent;

    private BlockGenerator m_blockGenerator;



    private void Awake()
    {
        m_blockGenerator = GetComponent<BlockGenerator>();

        m_parent = new GameObject("Blocks");

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



        // coreの生成
        GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePosX, m_corePosY), m_parent.transform, m_isBrightness);

        //GameObject co = Instantiate<GameObject>(m_blockCore, new Vector3(m_corePosX,m_corePosY), Quaternion.identity);
        //co.transform.parent = m_parent.transform;
        //明るさの追加
        //if (m_isBrightness)
        //    co.AddComponent<ChangeBrightness>();


        //  プレイシーンマネージャーが無かったら格納しない
        if (m_playSceneManager == null)
            Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:DungeonManager");
        else
        {
            m_playSceneManager.SetPlayer(pl);
            m_playSceneManager.SetCore(co);
        }

        //１０＊１０のリスト管理用
        List<List<List<string>>> mapListManager = new List<List<List<string>>>();


        for (int i = 0; i < m_dungeonPath.Count; i++)
        {
            // ファイルがなければマップ読み込みの処理をしない
            if (m_dungeonPath[i] == null)
            {
                Debug.Log("CSV file is not assigned");
                return;

            }

            // マップのリスト
            List<List<string>> mapList = new List<List<string>>();

            // ファイルの内容を1行ずつ処理
            string[] lines = m_dungeonPath[i].text.Split('\n');
            foreach (string line in lines)
            {
                string[] values = line.Split(',');

                // 各行のデータを格納するリスト
                List<string> rowData = new List<string>();

                // 各列の値を処理する
                foreach (string value in values)
                {
                    // データをリストに追加
                    rowData.Add(value);
                }

                // 行のデータをCSVデータに追加
                mapList.Add(rowData);
            }

            {
                //// ファイル読み込み
                //StreamReader streamReader = new StreamReader(m_dungeonPath[i].text);

                //// 改行区切りで読み出す
                //foreach (string line in streamReader.ReadToEnd().Split("\n"))
                //{
                //    // 行が存在しなければループを抜ける
                //    if (line == "")
                //        break;

                //    string lin = line.Remove(line.Length - 1);

                //    List<string> list = new List<string>();

                //    // カンマ区切りで読み出す
                //    foreach (string line2 in lin.Split(","))
                //    {
                //        list.Add(line2);
                //    }

                //    mapList.Add(list);
                //}
            }


            //３次元に入れる
            mapListManager.Add(mapList);

            // ファイルを閉じる
            //streamReader.Close();

        }

        //ブロック生成
        for (int y = 0; y < m_dungeonSizeY; y++)
        {
            for (int x = 0; x < m_dungeonSizeX; x++)
            {
                int random = Random.Range(0, m_dungeonPath.Count);
                Make10_10Block(mapListManager[random], x * 10, y * 10);

            }
        }

        //岩盤で囲う
        for (int i = 0; i < m_dungeonSizeY * 10; i++)
        {
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(m_dungeonSizeY * 10, i, 0), m_parent.transform, m_isBrightness);
        }
        for (int i = 0; i < m_dungeonSizeX * 10; i++)
        {
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
            m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, m_dungeonSizeY * 10, 0), m_parent.transform, m_isBrightness);
        }

        //地面の生成
        for (int y = 0; y < m_dungeonSizeY * 10; ++y)
        {
            for (int x = 0; x < m_dungeonSizeX * 10; ++x)
            {
                // 生成座標
                Vector3 pos = new(x, y, 0.0f);

                // ブロックの生成
                GameObject block = Instantiate<GameObject>(m_ground, pos, Quaternion.identity);

                block.transform.parent = m_parent.transform;

            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {

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
                //プレイヤー
                if((int)m_playerPos.x == x + originX && (int)m_playerPos.y == y + originY)
                {
                    continue;
                }

                //コアを生成
                if(m_corePosX == x + originX && m_corePosY == y + originY)
                {
                    continue;
                }
                // 0 の場合は何も生成しない
                if (mapList[y][x] == "0" || mapList[y][x] == "")
                    continue;

                // 生成座標
                Vector3 pos = new(originX + x,originY + y, 0.0f);

                // ブロックの生成
                m_blockGenerator.GenerateBlock(m_blockOddsList[LotteryBlock()].type, pos, m_parent.transform, m_isBrightness);
            }
        }
    }


    int LotteryBlock()
    {
        //確率の抽選
        List<int> oddsList = new List<int>();

        int allOdds = 0;

        //ブロックの種類の数
        for (int i = 0; i < m_blockOddsList.Count; i++)
        {
            //ブロックの確率
            for (int j = 0; j < m_blockOddsList[i].odds; j++)
            {
                oddsList.Add(i);
            }
            //ブロックの確率を加算
            allOdds += m_blockOddsList[i].odds;
        }
        //抽選
        return oddsList[Random.Range(0,allOdds)];
    }

}
