
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
	[System.Serializable]
	public enum Type
	{
		STONE,  // Šâ
		COAL,   // Î’Y

		OVER
	}

	public string name;		// –¼‘O
	public Type type;		// í—Ş
	public int count;		// ”
	public Sprite sprite;   // ‰æ‘œ

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
