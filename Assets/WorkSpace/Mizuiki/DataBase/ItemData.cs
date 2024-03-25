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

	[Header("�A�C�e����")]
	public string itemName;
	[Header("�A�C�e���̎��")]
	public Type type;
	[Header("�A�C�e���̉摜")]
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
	public ItemData.Type type;       // ���
	public int count;       // ��
}

