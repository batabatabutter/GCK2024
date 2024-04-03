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
		//TOACH,      // ����
		//UPGRADE,	// �̌@�A�b�v�O���[�h
		BOMB,       // ���e
		ARMOR,		// �A�[�}�[

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
	[Header("�ݒu����ꍇ�̓v���n�u")]
	public GameObject objectPrefab = null;
	[Header("�g�p���̊֐����Ăяo���c�[��")]
	public Tool tool;

	[Header("�c�[���쐬�ɕK�v�ȑf��")]
	public List<Items> itemMaterials = new List<Items>();

	public ToolData(ToolData tool)
	{
		toolName = tool.toolName;
		type = tool.type;
		sprite = tool.sprite;
		recastTime = tool.recastTime;
		objectPrefab = tool.objectPrefab;
		itemMaterials = tool.itemMaterials;
	}

}

