using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItem")]
public class ItemData : ScriptableObject
{
	[System.Serializable]
	public enum Type
	{
		STONE,  // ��
		COAL,   // �ΒY

		OVER
	}

	public string itemName;     // ���O
	public Type type;       // ���
	public Sprite sprite;   // �摜

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
	public ItemData.Type type;       // ���
	public int count;       // ��
}

