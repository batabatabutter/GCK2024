using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSearchBlock : MonoBehaviour
{
    [Header("サーチ対象")]
    [SerializeField, CustomEnum(typeof(BlockData.BlockType))] private string m_searchBlockType;
    private BlockData.BlockType m_blockType;

	// サーチ対象ブロックのゲームオブジェクトオブジェクト
	private readonly List<Transform> m_searchBlocks = new();
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
    [Header("マーカーの有効時間")]
    [SerializeField] private float m_markerLifeTime = 1.0f;



	private void Awake()
	{
        m_blockType = SerializeUtil.Restore<BlockData.BlockType>(m_searchBlockType);
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SearchOne();
            CreateSearchMarker();
        }
    }

    // 最も近いブロック
    public void SearchOne()
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
        Transform nearBlock = m_searchBlocks[0];
        // 近いブロックの距離
        float nearDistance = Vector2.Distance(transform.position, m_searchBlocks[0].transform.position);

        foreach (Transform block in m_searchBlocks)
        {
            float distance = Vector2.Distance(transform.position, block.transform.position);

            // より近い
            if (distance < nearDistance)
            {
                nearDistance = distance;
                nearBlock = block;
            }
        }

        // 最も近いブロックをサーチ対象として追加
        m_targetBlocks.Add(nearBlock);

    }

    // 範囲内のすべてのブロック
    public void SearchAll()
    {
        // ターゲットを空にする
        m_targetBlocks.Clear();

		// サーチ対象がなければ返す
		if (m_searchBlocks.Count == 0)
		{
			Debug.Log("サーチ対象がないよ");
			return;
		}

        foreach (Transform block in m_searchBlocks)
        {
            // プレイヤーからの距離
            float distance = Vector2.Distance(transform.position, block.transform.position);

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
	}

    // インスペクターで設定された個数のブロック
    public void Search()
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
        m_targetBlocks = new (m_searchBlocks);

        // 削除条件
        m_targetBlocks.RemoveAll(b => Vector2.Distance(transform.position, b.transform.position) > m_searchRange);

        // 距離の近い順にソート
        m_targetBlocks.Sort((lhs, rhs) => Vector2.Distance(transform.position, lhs.transform.position).CompareTo((int)Vector2.Distance(transform.position, rhs.transform.position)));

		// サーチ対象外を削除
        while (m_targetBlocks.Count > m_hitCount)
        {
            m_targetBlocks.RemoveAt(m_targetBlocks.Count - 1);
        }

	}

    // ブロックの設定
    public void SetSearchBlocks(List<Block> blocks)
    {
        foreach (Block block in blocks)
        {
            if (block.BlockData == null)
                continue;

            // サーチ対象のブロックである
            if (block.BlockData.Type == m_blockType)
            {
                // トランスフォームの追加
                m_searchBlocks.Add(block.transform);
            }
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
            Instantiate(m_markerObject, target).LifeTime = m_markerLifeTime;
        }
    }

}
