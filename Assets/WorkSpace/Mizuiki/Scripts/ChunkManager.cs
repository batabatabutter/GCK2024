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

		// �L�����擾
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

		// �\���ݒ�
		public void SetActive(bool active)
		{
			blockChunk.SetActive(active);
			shadowChunk.SetActive(active);
		}

	}

	[Header("�v���C���[")]
	[SerializeField] private Transform m_player = null;

	[Header("�`�����N�̃T�C�Y")]
	[SerializeField] private int m_chunkSize = 10;
	[Header("�\���`�����N��")]
	[SerializeField] private int m_activeChunk = 3;

	// �`�����N�̓񎟌��z��
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
		// �v���C���[�̂���`�����N
		Vector2Int playerChunk = new((int)m_player.position.x / m_chunkSize, (int)m_player.position.y / m_chunkSize);

		for (int y = 0; y < m_chunks.Count; y++)
		{
			for (int x = 0; x < m_chunks[y].Count; x++)
			{
				// �v���C���[�`�����N�Ƃ̋���
				float distance = Vector2Int.Distance(playerChunk, new Vector2Int(x, y));

				// �\���`�����N��
				if (distance < m_activeChunk)
				{
					if (m_chunks[y][x].activeSelf == false)
					{
						m_chunks[y][x].SetActive(true);
					}
				}
				// �\���`�����N�O
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



	// ���W�����ƂɃ`�����N���擾
	public Chunk GetChunk(Vector2 pos)
	{
		// ��������`�����N
		Vector2Int chunk = new((int)pos.x / m_chunkSize, (int)pos.y / m_chunkSize);

		// �����`�����N(y)�����݂��Ă��Ȃ�
		while (chunk.y >= m_chunks.Count)
		{
			m_chunks.Add(new());
		}
		// �����`�����N(x)�����݂��Ă��Ȃ�
		while (chunk.x >= m_chunks[chunk.y].Count)
		{
			// �`�����N����
			m_chunks[chunk.y].Add(new("(" + chunk.x + ", " + chunk.y + ")"));
		}

		return m_chunks[chunk.y][chunk.x];
	}


	// �v���C���[�̃g�����X�t�H�[��
	public Transform Player
	{
		set { m_player = value; }
	}

	// 1�`�����N�̃T�C�Y
	public int ChunkSize
	{
		get { return m_chunkSize; }
	}

}
