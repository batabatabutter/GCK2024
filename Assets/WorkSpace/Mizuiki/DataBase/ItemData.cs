using UnityEngine;
using static ToolData;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData_", menuName = "CreateDataBase/Item/ItemData")]
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

	[Header("�A�C�e����"), SerializeField]
	private string itemName;
	[Header("�A�C�e���̎��"), SerializeField ,CustomEnum(typeof(ItemType))]
	private string typeStr;
	[Header("�A�C�e���̉摜"), SerializeField]
	private Sprite sprite;
	[Header("�A�C�e���̃v���n�u"), SerializeField]
	private GameObject prefab = null;

	public string Name => name;
	public ItemType Type => SerializeUtil.Restore<ItemType>(typeStr);
	public Sprite Sprite => sprite;
	public GameObject Prefab => prefab;

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
    [CustomEnum(typeof(ItemData.ItemType))] public string typeStr;       // ���
	public ItemData.ItemType type => SerializeUtil.Restore<ItemData.ItemType>(typeStr);
    public int count;       // ��
}

