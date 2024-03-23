using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItem")]
public class ItemData : ScriptableObject
{
	[System.Serializable]
	public enum Type
	{
		STONE,  // Šâ
		COAL,   // Î’Y

		OVER
	}

	public string itemName;     // –¼‘O
	public Type type;       // í—Ş
	public Sprite sprite;   // ‰æ‘œ

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
	public ItemData.Type type;       // í—Ş
	public int count;       // ”
}

