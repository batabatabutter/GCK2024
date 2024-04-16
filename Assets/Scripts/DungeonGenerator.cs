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

    //"ステージ(0〜)
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

	[System.Serializable]
	public struct Generator
	{
		public DungeonData.Pattern pattern;
		public DungeonGeneratorBase generator;
	}

	[Header("ダンジョン生成スクリプト")]
    [SerializeField] private Generator[] m_generators = null;
	private Dictionary<DungeonData.Pattern, DungeonGeneratorBase> m_dungeonGenerators = new();


    private void Awake()
    {
        m_blockGenerator = GetComponent<BlockGenerator>();
        m_parent = new GameObject("Blocks");

		foreach (Generator generator in m_generators)
		{
			// 上書き防止
			if (m_dungeonGenerators.ContainsKey(generator.pattern))
				continue;
			// ジェネレータの設定
			m_dungeonGenerators[generator.pattern] = generator.generator;
		}
    }

    /// <summary>
    /// ステージ作成
    /// </summary>
    public void CreateStage()
    {
        // ダンジョンのデータ取得
        DungeonData dungeonData = m_dungeonDataBase.dungeonDatas[m_stageNum];

        // 生成パターン取得
        DungeonData.Pattern pattern = dungeonData.DungeonPattern;

		// ダンジョンのマップ取得
		List<List<string>> mapList = m_dungeonGenerators[pattern].GenerateDungeon(dungeonData);

		// ダンジョンのサイズ
		Vector2Int dungeonSize = new(mapList[0].Count, mapList.Count);

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
			m_playerPos = new Vector2(Random.Range(0, dungeonSize.x), Random.Range(0, dungeonSize.y));

			if (roop_error > 100)
			{
				Debug.Log("コアとプレイヤーが近すぎます。間隔を見直してください");
				break;
			}
			roop_error++;

		}
		// コアに近い限りループ
		while (MyFunction.DetectCollision(m_playerPos, m_corePos, new Vector2(m_playerLength, m_playerLength))
		//m_playerPos.x < m_corePos.x + m_playerLength &&
		//m_playerPos.x > m_corePos.x - m_playerLength &&
		//m_playerPos.y < m_corePos.y + m_playerLength &&
		//m_playerPos.y > m_corePos.y - m_playerLength
		);

		//  プレイヤーの生成
		GameObject pl = Instantiate(m_player, m_playerPos, Quaternion.identity);

		// coreの生成
		GameObject co = m_blockGenerator.GenerateBlock(BlockData.BlockType.CORE, new Vector3(m_corePos.x, m_corePos.y), null, m_isBrightness);


		//  プレイシーンマネージャーが無かったら格納しない
		if (m_playSceneManager == null)
		{
			Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:DungeonManager");
		}
		else
		{
			m_playSceneManager.SetPlayer(pl);
			m_playSceneManager.SetCore(co);
		}

		// ブロック生成
		CreateBlock(mapList, dungeonData.BlockOdds);

		//岩盤で囲う
		CreateBedrock(dungeonSize);

		//地面の生成
		CreateGround(dungeonSize);


		//// 生成パターンごと
		//switch (pattern)
  //      {
  //          case DungeonData.Pattern.TEN_X_TEN:
  //              Generate10to10(dungeonData);
  //              break;

  //          case DungeonData.Pattern.DIGGING:
  //              GenerateDigging(dungeonData);
  //              break;
  //      }


    }

    void Generate10to10(DungeonData dungeonData)
    {
		DungeonDataCSV data = dungeonData as DungeonDataCSV;

		Vector2Int size = data.Size;

		// コアの生成座標決定
		m_corePos.x = Random.Range(0, size.x * 10);
		m_corePos.y = Random.Range(0, size.y * 10);

		int roop_error = 0;

		//プレイヤーとコアの位置が離れるまで繰り返す
		do
		{
			m_playerPos = new Vector2(Random.Range(0, size.x * 10), Random.Range(0, size.y * 10));

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
		List<TextAsset> dungeonCSV = data.DungeonCSV;

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
		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				int random = Random.Range(0, dungeonCSV.Count);
				Make10_10Block(mapListManager[random], x, y);

			}
		}

		//岩盤で囲う
		CreateBedrock(size);

		//地面の生成
		CreateGround(size);

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
                    m_dungeonDataBase.dungeonDatas[m_stageNum].BlockOdds
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
		m_dungeonGenerators[DungeonData.Pattern.DIGGING].SetDungeonData(dig);
		// マップの取得
		List<List<string>> mapList = m_dungeonGenerators[DungeonData.Pattern.DIGGING].GenerateDungeon(dig);

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
                    BlockData.BlockType type = dungeonData.BlockOdds[LotteryBlock()].type;
                    // 生成する座標
                    Vector2 position = new(x, y);
                    // ブロック生成
                    m_blockGenerator.GenerateBlock(type, position, m_parent.transform, m_isBrightness);
                }
			}
		}

		//岩盤で囲う
		CreateBedrock(dungeonSize);

		//地面の生成
		CreateGround(dungeonSize);

	}

	int LotteryBlock()
    {
        //確率の抽選
        List<int> oddsList = new List<int>();

        //全ての確率合算
        int allOdds = 0;

        List<BlockOdds> blockOddsList = m_dungeonDataBase.dungeonDatas[m_stageNum].BlockOdds;


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

	// ブロックの生成
	private void CreateBlock(List<List<string>> mapList, List<BlockOdds> odds)
	{
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
					BlockData.BlockType type = odds[LotteryBlock()].type;
					// 生成する座標
					Vector2 position = new(x, y);
					// ブロック生成
					m_blockGenerator.GenerateBlock(type, position, m_parent.transform, m_isBrightness);
				}
			}
		}

	}

	// 岩盤の生成
	private void CreateBedrock(Vector2Int size)
	{
		//岩盤で囲う
		for (int x = 0; x < size.x; x++)
		{
			// 上下
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, -1    , 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(x, size.y, 0), m_parent.transform, m_isBrightness);
		}
		for (int y = 0; y < size.y; y++)
		{
			// 左右
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(-1    , y, 0), m_parent.transform, m_isBrightness);
			m_blockGenerator.GenerateBlock(BlockData.BlockType.BEDROCK, new Vector3(size.y, y, 0), m_parent.transform, m_isBrightness);
		}
	}

	// 地面の生成
	private void CreateGround(Vector2Int size)
	{
		//地面の生成
		for (int y = 0; y < size.y; ++y)
		{
			for (int x = 0; x < size.x; ++x)
			{
				// 生成座標
				Vector3 pos = new(x, y, 0.0f);
				// ブロックの生成
				GameObject block = Instantiate(m_ground, pos, Quaternion.identity);
				// 親の設定
				block.transform.parent = m_parent.transform;
			}
		}
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
