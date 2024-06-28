using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorCave : DungeonGeneratorBase
{
	[Header("空洞の生成割合"), Range(0.0f, 1.0f)]
	[SerializeField] private float m_cavity = 0.5f;

	[Header("ノイズのスケール"), Min(0.0f)]
	[SerializeField] private float m_noiseScale = 1.0f;


	// マップのサイズ
	private Vector2Int m_mapSize;

	// マップリスト
	readonly List<List<string>> m_mapList = new();


	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
	{
		// データのキャスト
		DungeonDataCave data = dungeonData as DungeonDataCave;

		// データの設定
		if (data)
		{
			SetDungeonData(data);
		}

		return GenerateDungeon(dungeonData.Size);

	}
	public override List<List<string>> GenerateDungeon(Vector2Int size)
	{
		// マップのサイズ取得
		m_mapSize = size;

		// マップ初期化
		for (int y = 0; y < size.y; y++)
		{
			List<string> list = new();
			for (int x = 0; x < size.x; x++)
			{
				// 列の追加
				list.Add("1");
			}
			// 行の追加
			m_mapList.Add(list);
		}

		// 空洞の生成
		CreateCave();

		// 生成したマップを返す
		return m_mapList;
	}

	public void SetDungeonData(DungeonDataCave data)
    {
		// 空洞率
		m_cavity = data.Cavity;
	}

	
	private void CreateCave()
	{
		// 乱数取得
		float random = Random.value;

		for (int y = 0; y < m_mapSize.y; y++)
		{
			for (int x = 0; x < m_mapSize.x; x++)
			{
				// ノイズ取得
				float noise = MyFunction.GetNoise(new Vector2(x, y) * m_noiseScale, random);

				// ノイズの値が生成割合以下
				if (noise <= m_cavity)
				{
					m_mapList[y][x] = "0";
				}
			}
		}
	}

}
