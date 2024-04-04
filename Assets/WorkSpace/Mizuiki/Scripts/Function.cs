using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunction
{
	// 四捨五入
	static public Vector2 RoundHalfUp(Vector2 value)
	{
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);

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

	// アイテムデータの取得
	static public ItemData GetItemData(ItemDataBase dataBase, ItemData.Type type)
	{
		foreach(ItemData data in dataBase.item)
		{
			if (data.type == type)
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
			if (data.type == type)
			{
				return data;
			}
		}
		// データが見つからなかった
		return null;
	}
}
