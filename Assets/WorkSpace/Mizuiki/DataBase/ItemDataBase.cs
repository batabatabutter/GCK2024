
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
	[System.Serializable]
	public enum Type
	{
		STONE,  // ��
		COAL,   // �ΒY

		OVER
	}

	public string name;		// ���O
	public Type type;		// ���
	public int count;		// ��
	public Sprite sprite;   // �摜

	public ItemData(ItemData item)
	{
		name = item.name;
		type = item.type;
		count = item.count;
		sprite = item.sprite;
	}
}

[CreateAssetMenu(fileName = "ToolDataBase", menuName = "CreateToolDataBase")]
public class ItemDataBase : ScriptableObject
{
	public List<ItemData> tool;
}
