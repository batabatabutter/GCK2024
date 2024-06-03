using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolData_", menuName = "CreateDataBase/Tool/ToolData")]
public class ToolData : ScriptableObject
{
	// �c�[���̎��
	[System.Serializable]
	public enum ToolType
	{
		TOACH	= 0,	// ����
		BOMB,			// ���e
		ARMOR,			// �A�[�}�[

		NORMAL_NUM,		//	���݂̃c�[����

		RARE	= ItemData.ItemType.BIRTHDAY_STONE,		// �������烌�A�c�[��

		// �G�������h : 5��
		HEALING_TOACH = ItemData.ItemType.BIR_EMERALD,	// �����̏���
		DOUBLE_PICAXE,									// �{����͂�
		LIMIT_TOTEM,									// �����̃g�[�e��

		// ���r�[ : 7��
		DRILL = ItemData.ItemType.BIR_RUBY,				// �h����
		DENGEROUS_BOMB,									// �댯���e
		HEAVY_ARMOR,									// �d�Z

		// �T�t�@�C�A : 9��
		HAMMER = ItemData.ItemType.BIR_SAPPHIRE,		// �n���}�[
		MINING_BOMB,									// �̌@���e
		HEALING_AURA,									// �����̃I�[��

		// �g�p�[�Y : 11��
		HOLY_TOACH = ItemData.ItemType.BIR_TOPAZ,		// ����
		RANGE_DESTROYER,								// �͈͔j���͂�
		SHIELD,											// �V�[���h

		OVER
	}

	// �c�[���̕���
	[System.Serializable]
	public enum ToolCategory
	{
		PUT,		// �ݒu�^
		SUPPORT,	// �K���^

		OVER,
	}

	[Header("�c�[����"), SerializeField]
	private string toolName = "";
	[Header("�c�[���̎��"), SerializeField, CustomEnum(typeof(ToolType))]
	private string typeStr;
	private ToolType type;
	[Header("�c�[���̕���"), SerializeField]
	private ToolCategory category = ToolCategory.SUPPORT;
	[Header("�c�[���̃A�C�R���摜"), SerializeField]
	private Sprite icon = null;
	[Header("���L���X�g����"), SerializeField]
	private float recastTime = 0.0f;
	[Header("�c�[���̃v���n�u"), SerializeField]
	private GameObject prefab = null;

	[Header("�c�[���쐬�ɕK�v�ȑf��"), SerializeField]
	private Items[] itemMaterials = null;

	public string Name => toolName;
	public ToolType Type => type;
	public ToolCategory Category => category;
	public Sprite Icon => icon;
	public float RecastTime => recastTime;
	public GameObject Prefab => prefab;
	public Items[] ItemMaterials => itemMaterials;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<ToolType>(typeStr);
	}

	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		typeStr = tool.typeStr;
		icon = tool.icon;
		recastTime = tool.recastTime;
		prefab = tool.prefab;
		itemMaterials = tool.itemMaterials;
	}

}

