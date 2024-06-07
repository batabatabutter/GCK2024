using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchBlock : MonoBehaviour
{
    [Header("サーチ対象")]
    [SerializeField, CustomEnum(typeof(BlockData.BlockType))] private string m_searchBlockType;
    private BlockData.BlockType m_blockType;

	// サーチ対象ブロックのゲームオブジェクトオブジェクト
	private readonly Dictionary<BlockData.BlockType, List<Transform>> m_searchBlocks = new();
    // ターゲットのブロックのゲームオブジェクト
    private List<Transform> m_targetBlocks = new();

    [Header("サーチ範囲(半径)")]
    [SerializeField] private float m_searchRange;

    [Header("サーチ数")]
    [SerializeField] private float m_hitCount = 1;
    [Header("全選択")]
    [SerializeField] private bool m_searchAll = false;

    [Header("ターゲットの位置を示すマーカー")]
    [SerializeField] private SearchMarker m_markerObject = null;

    [Header("マーカーの有効時間(秒)")]
    [SerializeField] private float m_markerLifeTime = 1.0f;
    [Header("マーカーの表示範囲(半径)")]
    [SerializeField] private float m_markerMaxScale = 50.0f;

    [Header("開始時にサーチ")]
    [SerializeField] private bool m_awake = false;


	private void Awake()
	{
        m_blockType = SerializeUtil.Restore<BlockData.BlockType>(m_searchBlockType);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SearchOne(m_blockType);
        }
    }

    // 最も近いブロック
    public void SearchOne(BlockData.BlockType type)
    {
        // ターゲットを空にする
        m_targetBlocks.Clear();

        // サーチ対象がなければ返す
        if (m_searchBlocks.Count == 0)
        {
			Debug.Log("サーチ対象がないよ");
			return;
        }

        // 近いブロック
        List<Transform> nearBlock = m_searchBlocks[type];
        // 近い順にソート
        nearBlock.Sort((lhs, rhs) => Vector2.Distance(transform.position, lhs.position).CompareTo(Vector2.Distance(transform.position, rhs.position)));

        // 最も近いブロックをサーチ対象として追加
        m_targetBlocks.Add(nearBlock[0]);

        // マーカーの作成
		CreateSearchMarker();

	}

	// 範囲内のすべてのブロック
	public void SearchAll(BlockData.BlockType type)
    {
        // ターゲットを空にする
        m_targetBlocks.Clear();

		// サーチ対象がなければ返す
		if (m_searchBlocks.Count == 0)
		{
			Debug.Log("サーチ対象がないよ");
			return;
		}

        foreach (Transform block in m_searchBlocks[type])
        {
            // プレイヤーからの距離
            float distance = Vector2.Distance(transform.position, block.position);

            // すべてがサーチ範囲
            if (m_searchAll)
            {
                m_targetBlocks.Add(block);
                continue;
            }

            // サーチ範囲より近い
            if (distance <= m_searchRange)
            {
                m_targetBlocks.Add(block);
            }
        }

		// マーカーの作成
		CreateSearchMarker();

	}

	// インスペクターで設定された個数のブロック
	public void SearchCount(BlockData.BlockType type, int count)
    {
		// ターゲットを空にする
		m_targetBlocks.Clear();

		// サーチ対象がなければ返す
		if (m_searchBlocks.Count == 0)
		{
            Debug.Log("サーチ対象がないよ");
			return;
		}

        // コピー渡し
        m_targetBlocks = new(m_searchBlocks[type]);

        // サーチ範囲外は削除
        m_targetBlocks.RemoveAll(b => Vector2.Distance(transform.position, b.transform.position) > m_searchRange);

        // 距離の近い順にソート
        m_targetBlocks.Sort((lhs, rhs) => Vector2.Distance(transform.position, lhs.transform.position).CompareTo((int)Vector2.Distance(transform.position, rhs.transform.position)));

		// サーチ対象外を削除
        while (m_targetBlocks.Count > m_hitCount)
        {
            m_targetBlocks.RemoveAt(m_targetBlocks.Count - 1);
        }

		// マーカーの作成
		CreateSearchMarker();

	}

	// ブロックの設定
	public void SetSearchBlocks(List<Block> blocks)
    {
        foreach (Block block in blocks)
        {
            //  nullならスキップ
            if (!block) continue;

            if (block.BlockData == null)
                continue;

            // ブロックの種類取得
            BlockData.BlockType type = block.BlockData.Type;

            // キーが存在しない
            if(!m_searchBlocks.ContainsKey(type))
            {
                m_searchBlocks[type] = new();
            }

            // ブロックの追加
            m_searchBlocks[type].Add(block.transform);
        }

		// 開始と同時にサーチする
		if (m_awake)
		{
			SearchOne(BlockData.BlockType.CORE);
		}

	}



	private void CreateSearchMarker()
    {
        // マーカーがない
        if (m_markerObject == null)
        {
            Debug.Log("サーチ : マーカーがないよ");
            return;
        }
        // すべてのターゲットの位置にマーカーを生成する
        foreach (Transform target in m_targetBlocks)
        {
            // マーカーを生成
            SearchMarker marker = Instantiate(m_markerObject, target.position, Quaternion.identity);
            // 生存時間設定
            marker.LifeTime = m_markerLifeTime;
            // 最大サイズ設定
            marker.MaxScale = m_markerMaxScale;
            // プレイヤー
            marker.Player = transform;
		}
    }



    // マーカーの生存時間
    public float MarkerLifeTime
    {
        set { m_markerLifeTime = value; }
    }

    // マーカーの表示範囲
    public float MarkerMaxScale
    {
        set { m_markerMaxScale = value; }
    }


}
