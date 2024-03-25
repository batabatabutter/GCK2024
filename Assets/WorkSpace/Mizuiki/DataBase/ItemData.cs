using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItem")]
public class ItemData : ScriptableObject
{
	[System.Serializable]
	public enum Type
	{
		STONE,  // 岩
		COAL,   // 石炭

		OVER
	}

	[Header("アイテム名")]
	public string itemName;
	[Header("アイテムの種類")]
	public Type type;
	[Header("アイテムの画像")]
	public Sprite sprite;

	public ItemData(ItemData item)
	{
		itemName = item.itemName;
		type = item.type;
		sprite = item.sprite;
	}
}

[System.Serializable]
public class Items
{
	public ItemData.Type type;       // 種類
	public int count;       // 数
}

