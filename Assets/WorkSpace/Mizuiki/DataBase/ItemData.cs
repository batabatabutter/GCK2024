using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using static BlockData;
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
		COPPER,			// ��
		TIN,			// ��
		LEAD,			// ��
		IRON,			// �S
		STEEL,			// �|

		BIRTHDAY_STONE	= 1000,		// ��������a���΃V���[�Y
		BIR_GARNET		= 1010,		// �K�[�l�b�g
		BIR_AMETHYST	= 1020,		// �A���W�X�g
		BIR_AQUAMARINE	= 1030,		// �A�N�A�}����
		BIR_DIAMOND		= 1040,		// �_�C�������h
		BIR_EMERALD		= 1050,		// �G�������h
		BIR_PEARL		= 1060,		// �p�[��
		BIR_RUBY		= 1070,		// ���r�[
		BIR_PERIDOT		= 1080,		// �y���h�b�g
		BIR_SAPPHIRE	= 1090,		// �T�t�@�C�A
		BIR_OPAL		= 1100,		// �I�p�[��
		BIR_TOPAZ		= 1110,		// �g�p�[�Y
		BIR_TURQUOISE	= 1120,		// �^�[�R�C�Y

		OVER,
	}

	[Header("�A�C�e����"), SerializeField]
	private string itemName;
	[Header("�A�C�e���̎��"), SerializeField ,CustomEnum(typeof(ItemType))]
	private string typeStr;
	private ItemType type;
	[Header("�A�C�e���̉摜"), SerializeField]
	private Sprite sprite;
	[Header("�A�C�e���̃v���n�u"), SerializeField]
	private GameObject prefab = null;
	[Header("�A�C�e���̐F"), SerializeField]
	private Color color = Color.white;

	public string Name => name;
	public ItemType Type => type;
	public Sprite Sprite => sprite;
	public GameObject Prefab => prefab;
	public Color Color => color;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<ItemType>(typeStr);
	}

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
    [CustomEnum(typeof(ItemData.ItemType))]
	[SerializeField] private string typeStr;       // ���
	//[NonSerialized] public ItemData.ItemType type;
	public ItemData.ItemType Type => SerializeUtil.Restore<ItemData.ItemType>(typeStr);
    public int count;       // ��
}

