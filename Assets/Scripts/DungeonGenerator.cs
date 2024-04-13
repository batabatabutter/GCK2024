using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class DungeonGenerator : MonoBehaviour
{
    [Header("明るさをつける(デバッグ)")]
    [SerializeField] private bool m_isBrightness;

    //"ステージ(0～)
    private int m_stageNum;

    [Header("ダンジョンデータベース")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

    [System.Serializable]
    public class BlockOdds
    {
        [Header("種類")]
        public BlockData.BlockType type;       // 種類
        [Header("確率")]
        public int odds;       // 確率
    }

    [Header("核からプレイヤーの出現しない距離")]
    [SerializeField] private int m_playerLength = 35;
    [Header("プレイヤー")]
    [SerializeField] private GameObject m_player;
    [Header("プレイシーンマネージャー")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    [Header("地面背景")]
    [SerializeField] private GameObject m_ground;

    //コアの位置
    private Vector2Int m_corePos;
    //プレイヤーの位置
    private Vector2 m_playerPos;
    //ブロックの親
    private GameObject m_parent;

    //ブロック生成スクリプト
    private BlockGenerator m_blockGenerator;

    [Header("ダンジョン生成スクリプト")]
    [SerializeField] private TestDungeonGenerator1 m_dungeonGeneratorDig = null;


    private void Awake()
    {
        m_blockGenerator = GetComponent<BlockGenerator>();
        m_parent = new GameObject("Blocks");
    }

    /// <summary>
    /// ステージ作成
    /// </summary>
    public void CreateStage()
    {
        // ダンジョンのデータ取得
        DungeonData dungeonData = m_dungeonDataBase.dungeonDatas[m_stageNum];

        // ダンジョンのサイズ取得
        Vector2 dungeonSize = dungeonData.dungeonSize;

        // 生成パターン取得
        DungeonData.Pattern pattern = dungeonData.pattern;

        // 生成パターンごと
        switch (pattern)
        {
            case DungeonData.Pattern.TEN_X_TEN:
                Generate10to10(dungeonData, dungeonSize);
                break;

            case DungeonData.Pattern.DIGGING:
                GenerateDigging(dungeonData);
                break;
        }


    }

    void Generate10to10(DungeonData dungeonData, Vector2 size)
    {
		// コアの生成座標決定
		m_corePos.x = Random.Range(0, (int)size.x * 10);
		m_corePos.y = Random.Range(0, (int)size.y * 10);

		int roop_error = 0;

		//プレイヤーとコアの位置が離れるまで繰り返す
		do
		{
			m_playerPos = new Vector2(Random.Range(0, (int)size.x * 10), Random.Range(0, (int)size.y * 10));

			if (roop_error > 100)
			{
				Debug.Log("コアとプレイヤーが近すぎます。間隔を見直してください");
				break;
			}
			roop_error++;

		}
		while (
		m_playerPos.x < m_corePos.x + m_playerLength &&
		m_playerPos.x > m_corePos.x - m_playerLength &&
		m_playerPos.y < m_corePos.y + m_playerLength &&
		m_playerPos.y > m_corePos.y - m_playerLength
		);

		//  プレイヤーの生成
		GameObject pl = Instantiate<GameObject>(m_player, m_playerPos, Quaternion.identity);

		// coreの生成
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y), null, m_isBrightness);


		//  プレイシーンマネージャーが無かったら格納しない
		if (m_playSceneManager == null)
			Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:DungeonManager");
		else
		{
			m_playSceneManager.SetPlayer(pl);
			m_playSceneManager.SetCore(co);
		}

		//１０＊１０のリスト管理用
		List<List<List<string>>> mapListManager = new ();

		//データベースからリストの取得
		List<TextAsset> dungeonCSV = dungeonData.dungeonCSV;

		for (int i = 0; i < dungeonCSV.Count; i++)
		{
			// ファイルがなければマップ読み込みの処理をしない
			if (dungeonCSV[i] == null)
			{
				Debug.Log("CSV file is not assigned");
				return;

			}

			// マップのリスト
			List<List<string>> mapList = new ();

			// ファイルの内容を1行ずつ処理
			string[] lines = dungeonCSV[i].text.Split('\n');
			foreach (string line in lines)
			{
				string[] values = line.Split(',');

				// 各行のデータを格納するリスト
				List<string> rowData = new ();

				// 各列の値を処理する
				foreach (string value in values)
				{
					// データをリストに追加
					rowData.Add(value);
				}

				// 行のデータをCSVデータに追加
				mapList.Add(rowData);
			}


			//３次元に入れる
			mapListManager.Add(mapList);

		}

		//ブロック生成
		for (int y = 0; y < (int)size.y; y++)
		{
			for (int x = 0; x < (int)size.x; x++)
			{
				int random = Random.Range(0, dungeonCSV.Count);
				Make10_10Block(mapListManager[random], x * 10, y * 10);

			}
		}

		//岩盤で囲う
		for (int i = 0; i < (int)size.y * 10; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3((int)size.y * 10, i, 0), m_parent.transform, m_isBrightness);
		}
		for (int i = 0; i < (int)size.x * 10; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, (int)size.y * 10, 0), m_parent.transform, m_isBrightness);
		}

		//地面の生成
		for (int y = 0; y < (int)size.y * 10; ++y)
		{
			for (int x = 0; x < (int)size.x * 10; ++x)
			{
				// 生成座標
				Vector3 pos = new(x, y, 0.0f);

				// ブロックの生成
				GameObject block = Instantiate(m_ground, pos, Quaternion.identity);

				block.transform.parent = m_parent.transform;

			}
		}

	}
	void Make10_10Block(List<List<string>> mapList, int originX, int originY)
    {
        // 読みだしたデータをもとにダンジョン生成をする
        for (int y = 0; y < mapList.Count; y++)
        {
            for (int x = 0; x < mapList[y].Count; x++)
            {
                //プレイヤー
                if ((int)m_playerPos.x == x + originX && (int)m_playerPos.y == y + originY)
                {
                    continue;
                }

                //コアを生成
                if (m_corePos.x == x + originX && m_corePos.y == y + originY)
                {
                    continue;
                }
                // 0 の場合は何も生成しない
                if (mapList[y][x] == "0" || mapList[y][x] == "")
                    continue;

                // 生成座標
                Vector3 pos = new(originX + x, originY + y, 0.0f);

                // ブロックの生成
                m_blockGenerator.GenerateBlock(
                    m_dungeonDataBase.dungeonDatas[m_stageNum].dungeonOdds
                    [LotteryBlock()].type,
                    pos,
                    m_parent.transform,
                    m_isBrightness
                    );
            }
        }
    }

    void GenerateDigging(DungeonData dungeonData)
    {
		// 派生クラスにキャストする
		DungeonDataDigging dig = dungeonData as DungeonDataDigging;
		// キャストできなかった
		if (dungeonData == null)
			return;

		// ダンジョンのサイズ取得
		Vector2Int dungeonSize = dig.Size;

		// データの設定
		m_dungeonGeneratorDig.SetDungeonData(dig);
		// マップの取得
		List<List<string>> mapList = m_dungeonGeneratorDig.GenerateDungeon(dungeonSize);

		// コアの生成座標決定(無限ループにならないように回数制限をつける)
		for (int i = 0; i < 1000000; i++)
		{
			// コアの生成位置をランダムで取得
			Vector2Int pos = new(Random.Range(0, dungeonSize.x), Random.Range(0, dungeonSize.y));
			// 生成位置がブロックなら設定してループを抜ける
			if (mapList[pos.y][pos.x] == "1")
			{
				m_corePos = pos;
				break;
			}
		}

		int roop_error = 0;

		//プレイヤーとコアの位置が離れるまで繰り返す
		do
		{
			m_playerPos = new Vector2(Random.Range(0.0f, dungeonSize.x), Random.Range(0.0f, dungeonSize.y));

			if (roop_error > 100)
			{
				Debug.Log("コアとプレイヤーが近すぎます。間隔を見直してください");
				break;
			}
			roop_error++;

		}
		// コアに近い限りループ
		while (
		m_playerPos.x < m_corePos.x + m_playerLength &&
		m_playerPos.x > m_corePos.x - m_playerLength &&
		m_playerPos.y < m_corePos.y + m_playerLength &&
		m_playerPos.y > m_corePos.y - m_playerLength
		);

		//  プレイヤーの生成
		GameObject pl = Instantiate(m_player, m_playerPos, Quaternion.identity);

		// coreの生成
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y), null, m_isBrightness);


		//  プレイシーンマネージャーが無かったら格納しない
		if (m_playSceneManager == null)
			Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:DungeonManager");
		else
		{
			m_playSceneManager.SetPlayer(pl);
			m_playSceneManager.SetCore(co);
		}

        // ブロック生成
        for (int y = 0; y < mapList.Count; y++)
        {
            for (int x = 0; x < mapList[y].Count; x++)
            {
				//プレイヤー
				if ((int)m_playerPos.x == x && (int)m_playerPos.y == y)
				{
					continue;
				}
				//コアを生成
				if (m_corePos.x == x && m_corePos.y == y)
				{
					continue;
				}
                // ブロックの生成位置
                if (mapList[y][x] == "1")
                {
                    // 生成する種類
                    BlockData.BlockType type = dungeonData.dungeonOdds[LotteryBlock()].type;
                    // 生成する座標
                    Vector2 position = new(x, y);
                    // ブロック生成
                    m_blockGenerator.GenerateBlock(type, position, m_parent.transform, m_isBrightness);
                }
			}
		}

		//岩盤で囲う
		for (int i = 0; i < dungeonSize.y; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1, i, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(dungeonSize.y, i, 0), m_parent.transform, m_isBrightness);
		}
		for (int i = 0; i < dungeonSize.x; i++)
		{
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, -1, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(i, dungeonSize.y, 0), m_parent.transform, m_isBrightness);
		}

		//地面の生成
		for (int y = 0; y < dungeonSize.y; ++y)
		{
			for (int x = 0; x < dungeonSize.x; ++x)
			{
				// 生成座標
				Vector3 pos = new(x, y, 0.0f);

				// ブロックの生成
				GameObject block = Instantiate(m_ground, pos, Quaternion.identity);

				block.transform.parent = m_parent.transform;

			}
		}

	}

	int LotteryBlock()
    {
        //確率の抽選
        List<int> oddsList = new List<int>();

        //全ての確率合算
        int allOdds = 0;

        List<BlockOdds> blockOddsList = m_dungeonDataBase.dungeonDatas[m_stageNum].dungeonOdds;


        //ブロックの種類の数
        for (int i = 0; i < blockOddsList.Count; i++)
        {
            //ブロックの確率
            for (int j = 0; j < blockOddsList[i].odds; j++)
            {
                oddsList.Add(i);
            }
            //ブロックの確率を加算
            allOdds += blockOddsList[i].odds;
        }
        //抽選
        return oddsList[Random.Range(0, allOdds)];
    }


    public int GetStageNum()
    {
        return m_stageNum;
    }

    //  ステージ番号設定
    public bool SetStageNum(int num)
    {
        //  ステージ番号が範囲外だったらエラー
        if (num < 0 || num >= m_dungeonDataBase.dungeonDatas.Count)
        {
            Debug.Log("ステージが存在しない番号です。num = " + num);
            m_stageNum = int.MaxValue;
            return false;
        }
        else
        {
            m_stageNum = num;
            return true;
        }
    }
}
