using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunction
{
	// 範囲
	[System.Serializable]
	public struct MinMax
	{
		[Min(0)]
		public int min;
		[Min(1)]
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
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);

		return value;
	}
	static public Vector3 RoundHalfUp(Vector3 value)
	{
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);
		value.z = RoundHalfUp(value.z);

		return value;
	}
	static public float RoundHalfUp(float value)
	{
		// 小数点以下の取得
		float fraction = value - MathF.Floor(value);

		// 小数点以下が0.5未満
		if (fraction < 0.5f)
		{
			// 切り捨てる
			return MathF.Floor(value);
		}
		// 切り上げる
		return MathF.Floor(value) + 1.0f;

	}
	// 四捨五入(int)
	static public Vector2Int RoundHalfUpInt(Vector2 value)
	{
		Vector2Int val = new()
		{
			x = (int)RoundHalfUp(value.x),
			y = (int)RoundHalfUp(value.y)
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
}
