using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunction
{
	// �͈�
	[System.Serializable]
	public struct MinMax
	{
		[Min(0)]
		public int min;
		[Min(1)]
		public int max;

		public readonly bool Within(int val)
		{
			// �ŏ��l��菬����
			if (val < min)
				return false;
			// �ő�l���傫��
			if (val > max)
				return false;
			// ���l���܂܂�Ă���
			return true;
		}
		public readonly bool Within(float val)
		{
			// �ŏ��l��菬����
			if (val < min)
				return false;
			// �ő�l���傫��
			if (val > max)
				return false;
			// ���l���܂܂�Ă���
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

	// �l�̌ܓ�
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

	// 4�����̎擾
	static public Vector2 GetFourDirection(Vector2 direction)
	{
		// �x�N�g�����K��
		direction.Normalize();

		// �c��艡�̒l���傫��
		if (Mathf.Abs(direction.y) < Mathf.Abs(direction.x))
		{
			direction.y = 0.0f;
		}
		else
		{
			direction.x = 0.0f;
		}

		// �x�N�g�����K��
		direction.Normalize();

		return direction;
	}

	// �����_���ȕ����擾
	static public Direction GetRandomDirection()
	{
		// �����_���ȕ����擾
		int rand = UnityEngine.Random.Range(0, (int)Direction.RANDOM);
		// Enum�ŕԂ�
		return (Direction)rand;
	}

	// �A�C�e���f�[�^�̎擾
	static public ItemData GetItemData(ItemDataBase dataBase, ItemData.ItemType type)
	{
		foreach(ItemData data in dataBase.item)
		{
			if (data.Type == type)
			{
				return data;
			}
		}
		// �f�[�^��������Ȃ�����
		return null;
	}

	// �u���b�N�f�[�^�̎擾
	static public BlockData GetBlockData(BlockDataBase dataBase, BlockData.BlockType type)
	{
		foreach (BlockData data in dataBase.block)
		{
			if (data.Type == type)
			{
				return data;
			}
		}
		// �f�[�^��������Ȃ�����
		return null;
	}

	// �c�[���f�[�^�̎擾
	static public ToolData GetToolData(ToolDataBase dataBase, ToolData.ToolType type)
	{
		foreach (ToolData data in dataBase.tool)
		{
			if (data.Type == type)
			{
				return data;
			}
		}
		// �f�[�^��������Ȃ�����
		return null;
	}

	// �P���ȃ{�b�N�X�R���C�_�[
	static public bool DetectCollision(Vector2 point, Vector2 boxPos, Vector2 boxSize)
	{
		if (point.x < boxPos.x + boxSize.x &&
			point.x > boxPos.x - boxSize.x &&
			point.y < boxPos.y + boxSize.y &&
			point.y > boxPos.y - boxSize.y)
		{
			// ������
			return true;
		}
		// ������Ȃ�
		return false;
	}

	// �����̎擾
	static public int GetLength(Vector2Int p1, Vector2Int p2)
	{
		// x �̋���
		int x = Math.Abs(p1.x - p2.x);
		// y �̋���
		int y = Math.Abs(p1.y - p2.y);

		return x + y;
	}
	static public int GetLength(int x1, int y1, int x2, int y2)
	{
		// x �̋���
		int x = Math.Abs(x1 - x2);
		// y �̋���
		int y = Math.Abs(y1 - y2);

		return x + y;
	}

	// �m�C�Y�̎擾
	static public float GetNoise(Vector2 pos, float val)
	{
		return Mathf.PerlinNoise(pos.x + val, pos.y + val);
	}
}
