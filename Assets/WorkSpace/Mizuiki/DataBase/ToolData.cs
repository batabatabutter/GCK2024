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

	[Header("�c�[����")]
	public string toolName = "";
	[Header("�c�[���̎��")]
	public ToolType type = ToolType.BOMB;
	[Header("�c�[���̕���")]
	public ToolCategory category = ToolCategory.SUPPORT;
	[Header("�c�[���̃A�C�R���摜")]
	public Sprite sprite = null;
	[Header("���L���X�g����")]
	public float recastTime = 0.0f;
	[Header("�c�[���̃v���n�u")]
	public GameObject prefab = null;

	[Header("�c�[���쐬�ɕK�v�ȑf��")]
	public List<Items> itemMaterials = new List<Items>();

	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		type = tool.type;
		sprite = tool.sprite;
		recastTime = tool.recastTime;
		prefab = tool.prefab;
		itemMaterials = tool.itemMaterials;
	}

}

