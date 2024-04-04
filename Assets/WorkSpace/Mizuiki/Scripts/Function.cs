using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunction
{
	// �l�̌ܓ�
	static public Vector2 RoundHalfUp(Vector2 value)
	{
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);

		return value;
	}
	static public float RoundHalfUp(float value)
	{
		// �����_�ȉ��̎擾
		float fraction = value - MathF.Floor(value);

		// �����_�ȉ���0.5����
		if (fraction < 0.5f)
		{
			// �؂�̂Ă�
			return MathF.Floor(value);
		}
		// �؂�グ��
		return MathF.Floor(value) + 1.0f;

	}
	// �l�̌ܓ�(int)
	static public Vector2Int RoundHalfUpInt(Vector2 value)
	{
		Vector2Int val = new()
		{
			x = (int)RoundHalfUp(value.x),
			y = (int)RoundHalfUp(value.y)
		};

		return val;
	}

	// �����O���b�h�ɂ���
	static public bool CheckSameGrid(Vector2 pos1, Vector2 pos2)
	{
		// �l�̌ܓ������l���擾(int)
		Vector2Int p1 = RoundHalfUpInt(pos1);
		Vector2Int p2 = RoundHalfUpInt(pos2);

		// �����O���b�h
		if (p1 == p2)
		{
			return true;
		}

		// �Ⴄ
		return false;
	}

	static public ItemData GetItemData(ItemDataBase dataBase, ItemData.Type type)
	{
		foreach(ItemData data in dataBase.item)
		{
			if (data.type == type)
			{
				return data;
			}
		}

		return null;
	}
}
