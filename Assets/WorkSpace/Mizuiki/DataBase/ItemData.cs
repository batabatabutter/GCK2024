using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItem")]
public class ItemData : ScriptableObject
{
	[System.Serializable]
	public enum ItemType
	{
		STONE	= 0,	// ��
		COAL,			// �ΒY
		STEEL,			// �S
		TIN,			// ��
		LEAD,			// ��

		BIRTHDAY_STONE = 200,	// ��������a���΃V���[�Y
		BIR_GARNET,				// �K�[�l�b�g
		BIR_AMETHYST,			// �A���W�X�g
		BIR_AQUAMARINE,			// �A�N�A�}����
		BIR_DIAMOND,			// �_�C�������h
		BIR_EMERALD,			// �G�������h
		BIR_PEARL,				// �p�[��
		BIR_RUBY,				// ���r�[
		BIR_PERIDOT,			// �y���h�b�g
		BIR_SAPPHIRE,			// �T�t�@�C�A
		BIR_OPAL,				// �I�p�[��
		BIR_TOPAZ,				// �g�p�[�Y
		BIR_TURQUOISE,			// �^�[�R�C�Y

		OVER,
	}

	[Header("�A�C�e����")]
	public string itemName;
	[Header("�A�C�e���̎��")]
	//public Type type;
	[CustomEnum(typeof(ItemType))] public string typeStr;
	[Header("�A�C�e���̉摜")]
	public Sprite sprite;
	[Header("�A�C�e���̃v���n�u")]
	public GameObject prefab = null;

	public ItemType type => SerializeUtil.Restore<ItemType>(typeStr);

	public ItemData(ItemData item)
	{
		itemName = item.itemName;
		typeStr = item.typeStr;
		sprite = item.sprite;
	}
}

[System.Serializable]
public class Items
{
	public ItemData.ItemType type;       // ���
	public int count;       // ��
}

