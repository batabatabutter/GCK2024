using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItem")]
public class ItemData : ScriptableObject
{
	[System.Serializable]
	public enum Type
	{
		STONE	= 0,	// ��
		COAL,			// �ΒY
		STEEL,			// �S
		TIN,			// ��
		LEAD,			// ��

		BIR_GARNET	= 200,	// �K�[�l�b�g
		BIR_AMETHYST,		// �A���W�X�g
		BIR_AQUAMARINE,		// �A�N�A�}����
		BIR_DIAMOND,		// �_�C�������h
		BIR_EMERALD,		// �G�������h
		BIR_PEARL,			// �p�[��
		BIR_RUBY,			// ���r�[
		BIR_PERIDOT,		// �y���h�b�g
		BIR_SAPPHIRE,		// �T�t�@�C�A
		BIR_OPAL,			// �I�p�[��
		BIR_TOPAZ,			// �g�p�[�Y
		BIR_TURQUOISE,		// �^�[�R�C�Y

		OVER,
	}

	[Header("�A�C�e����")]
	public string itemName;
	[Header("�A�C�e���̎��")]
	public Type type;
	[Header("�A�C�e���̉摜")]
	public Sprite sprite;
	[Header("�A�C�e���̃v���n�u")]
	public GameObject prefab = null;

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

