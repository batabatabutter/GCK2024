using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyFunction
{
	// ブロックの弱体化割合
	static public float BLOCK_WEAK => 0.8f;
	// ステージの最大レベル
	static public int MAX_STAGE_LEVEL => 10;


	// 範囲
	[System.Serializable]
	public struct MinMax
	{
		public int min;
		public int max;

		public readonly bool Within(int val)
		{
			// 最小値より小さい
			if (val < min)
				return false;
			// 最大値より大きい
			if (val > max)
				return false;
			// 数値が含まれている
			return true;
		}
		public readonly bool Within(float val)
		{
			// 最小値より小さい
			if (val < min)
				return false;
			// 最大値より大きい
			if (val > max)
				return false;
			// 数値が含まれている
			return true;
		}
	}
	[System.Serializable]
	public struct MinMaxFloat
	{
		[Min(0.0f)]
		public float min;
		[Min(0.0f)]
		public float max;
	}

	public enum Direction
	{
		UP,
		RIGHT,
		DOWN,
		LEFT,

		RANDOM,
	}

	// 四捨五入
	static public Vector2 RoundHalfUp(Vector2 value)
	{
		value.x = Mathf.Round(value.x);
		value.y = Mathf.Round(value.y);

		return value;
	}
	static public Vector3 RoundHalfUp(Vector3 value)
	{
		value.x = Mathf.Round(value.x);
		value.y = Mathf.Round(value.y);
		value.z = Mathf.Round(value.z);

		return value;
	}
	// 四捨五入(int)
	static public Vector2Int RoundHalfUpInt(Vector2 value)
	{
		Vector2Int val = new()
		{
			x = Mathf.RoundToInt(value.x),
			y = Mathf.RoundToInt(value.y)
		};

		return val;
	}

	// 同じグリッドにある
	static public bool CheckSameGrid(Vector2 pos1, Vector2 pos2)
	{
		// 四捨五入した値を取得(int)
		Vector2Int p1 = RoundHalfUpInt(pos1);
		Vector2Int p2 = RoundHalfUpInt(pos2);

		// 同じグリッド
		if (p1 == p2)
		{
			return true;
		}

		// 違う
		return false;
	}

	// 4方向の取得
	static public Vector2 GetFourDirection(Vector2 direction)
	{
		// ベクトル正規化
		direction.Normalize();

		// 縦より横の値が大きい
		if (Mathf.Abs(direction.y) < Mathf.Abs(direction.x))
		{
			direction.y = 0.0f;
		}
		else
		{
			direction.x = 0.0f;
		}

		// ベクトル正規化
		direction.Normalize();

		return direction;
	}

	// ランダムな方向取得
	static public Direction GetRandomDirection()
	{
		// ランダムな方向取得
		int rand = UnityEngine.Random.Range(0, (int)Direction.RANDOM);
		// Enumで返す
		return (Direction)rand;
	}
	static public Direction GetDirection(Direction direction)
	{
		if (direction == Direction.RANDOM)
		{
			return GetRandomDirection();
		}
		return direction;
	}

	// アイテムデータの取得
	static public ItemData GetItemData(ItemDataBase dataBase, ItemData.ItemType type)
	{
		foreach(ItemData data in dataBase.item)
		{
			if (data.Type == type)
			{
				return data;
			}
		}
		// データが見つからなかった
		return null;
	}

	// ブロックデータの取得
	static public BlockData GetBlockData(BlockDataBase dataBase, BlockData.BlockType type)
	{
		foreach (BlockData data in dataBase.block)
		{
			if (data.Type == type)
			{
				return data;
			}
		}
		// データが見つからなかった
		return null;
	}

	// ツールデータの取得
	static public ToolData GetToolData(ToolDataBase dataBase, ToolData.ToolType type)
	{
		foreach (ToolData data in dataBase.tool)
		{
			if (data.Type == type)
			{
				return data;
			}
		}
		// データが見つからなかった
		return null;
	}

	// 単純なボックスコライダー
	static public bool DetectCollision(Vector2 point, Vector2 boxPos, Vector2 boxSize)
	{
		if (point.x < boxPos.x + boxSize.x &&
			point.x > boxPos.x - boxSize.x &&
			point.y < boxPos.y + boxSize.y &&
			point.y > boxPos.y - boxSize.y)
		{
			// 当たる
			return true;
		}
		// 当たらない
		return false;
	}

	// 距離の取得
	static public int GetLength(Vector2Int p1, Vector2Int p2)
	{
		// x の距離
		int x = Math.Abs(p1.x - p2.x);
		// y の距離
		int y = Math.Abs(p1.y - p2.y);

		return x + y;
	}
	static public int GetLength(int x1, int y1, int x2, int y2)
	{
		// x の距離
		int x = Math.Abs(x1 - x2);
		// y の距離
		int y = Math.Abs(y1 - y2);

		return x + y;
	}

	// ノイズの取得
	static public float GetNoise(Vector2 pos, float val)
	{
		return Mathf.PerlinNoise(pos.x + val, pos.y + val);
	}

	// データの読み取り
	static public string Reader(string path)
	{
		// ファイルが存在しない
		if (!File.Exists(path))
		{
			Debug.Log(path + "は存在しません");
			return "";
		}

		// ファイルを開く
		StreamReader reader = new(path);
		// 読み取り
		string data = reader.ReadToEnd();
		// ファイルを閉じる
		reader.Close();
		// 読み取ったデータを返す
		return data;
	}
	// データの書き込み
	static public void Writer(string path, string data)
	{
		// 書き込むファイルを開く
		StreamWriter writer = new(path);
		// 書き込み
		writer.Write(data);
		// ファイルを閉じる
		writer.Close();
	}
	static public void Writer(string path, string[] data)
	{
		// 書き込みファイル
		StreamWriter writer = new(path, false);
		// 文字列をファイルに追加
		for (int i = 0; i < data.Length; i++)
		{
			writer.WriteLine(data[i]);
		}
		// ファイルを閉じる
		writer.Close();
	}
	static public void Writer(string path, List<string> data)
	{
		// 書き込みファイル
		StreamWriter writer = new(path, false);
		// 文字列をファイルに追加
		foreach (string str in data)
		{
			writer.WriteLine(str);
		}
		// ファイルを閉じる
		writer.Close();
	}

}
