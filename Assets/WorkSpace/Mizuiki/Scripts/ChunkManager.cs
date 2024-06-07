using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChunkManager : MonoBehaviour
{
	public struct Chunk
	{
		public GameObject blockChunk;
		public GameObject shadowChunk;

		public Chunk(string name)
		{
			blockChunk = new(name);
			shadowChunk = new(name);
		}

		// 有効か取得
		public readonly bool activeSelf
		{
			get
			{
				if (blockChunk.activeSelf)
					return true;
				if (shadowChunk.activeSelf)
					return true;
				return false;
			}
		}

		// 表示設定
		public void SetActive(bool active)
		{
			blockChunk.SetActive(active);
			shadowChunk.SetActive(active);
		}

	}

	[Header("プレイヤー")]
	[SerializeField] private Transform m_player = null;

	[Header("チャンクのサイズ")]
	[SerializeField] private int m_chunkSize = 10;
	[Header("表示チャンク数")]
	[SerializeField] private int m_activeChunk = 3;

	// チャンクの二次元配列
	private readonly List<List<Chunk>> m_chunks = new();
	//private readonly List<List<GameObject>> m_blockChunk = new();
	//private readonly List<List<GameObject>> m_shadowChunk = new();



	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// プレイヤーのいるチャンク
		Vector2Int playerChunk = new((int)m_player.position.x / m_chunkSize, (int)m_player.position.y / m_chunkSize);

		for (int y = 0; y < m_chunks.Count; y++)
		{
			for (int x = 0; x < m_chunks[y].Count; x++)
			{
				// プレイヤーチャンクとの距離
				float distance = Vector2Int.Distance(playerChunk, new Vector2Int(x, y));

				// 表示チャンク内
				if (distance < m_activeChunk)
				{
					if (m_chunks[y][x].activeSelf == false)
					{
						m_chunks[y][x].SetActive(true);
					}
				}
				// 表示チャンク外
				else
				{
					if (m_chunks[y][x].activeSelf == true)
					{
						m_chunks[y][x].SetActive(false);
					}
				}
			}
		}

	}



	// 座標をもとにチャンクを取得
	public Chunk GetChunk(Vector2 pos)
	{
		// 生成するチャンク
		Vector2Int chunk = new((int)pos.x / m_chunkSize, (int)pos.y / m_chunkSize);

		// 生成チャンク(y)が存在していない
		while (chunk.y >= m_chunks.Count)
		{
			m_chunks.Add(new());
		}
		// 生成チャンク(x)が存在していない
		while (chunk.x >= m_chunks[chunk.y].Count)
		{
			// チャンク生成
			m_chunks[chunk.y].Add(new("(" + chunk.x + ", " + chunk.y + ")"));
		}

		return m_chunks[chunk.y][chunk.x];
	}


	// プレイヤーのトランスフォーム
	public Transform Player
	{
		set { m_player = value; }
	}

	// 1チャンクのサイズ
	public int ChunkSize
	{
		get { return m_chunkSize; }
	}

}
