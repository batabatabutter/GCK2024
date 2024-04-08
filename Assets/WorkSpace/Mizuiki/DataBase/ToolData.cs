using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolData", menuName = "CreateTool")]
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

		RARE	= 100,		// �������烌�A�c�[��

		HOLY_TOACH,				// ����
		RANGE_DESTROYER,		// �͈͔j���͂�
		SHIELD,					// �V�[���h

		DRILL,					// �h����
		DENGEROUS_BOMB,         // �댯���e
		HEAVY_ARMOR,			// �d�Z

		HEALING_TOACH,			// �����̏���
		DOUBLE_PICAXE,			// �{����͂�
		LIMIT_TOTEM,			// �����̃g�[�e��

		HAMMER,					// �n���}�[
		MINING_BOMB,			// �̌@���e
		HEALING_AURA,			// �����̃I�[��

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
	public ToolType Type => SerializeUtil.Restore<ToolType>(typeStr);
	public ToolCategory Category => category;
	public Sprite Icon => icon;
	public float RecastTime => recastTime;
	public GameObject Prefab => prefab;
	public Items[] ItemMaterials => itemMaterials;


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

