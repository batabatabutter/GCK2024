using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolData", menuName = "CreateTool")]
public class ToolData : ScriptableObject
{
	[System.Serializable]
	public enum ToolType
	{
		TOACH,      // ����
		BOMB,       // ���e
		ARMOR,		// �A�[�}�[
		UPGRADE,	// �̌@�A�b�v�O���[�h

		OVER
	}

	[Header("�c�[����")]
	public string toolName = "";
	[Header("�c�[���̎��")]
	public ToolType type = ToolType.TOACH;
	[Header("�c�[���̃A�C�R���摜")]
	public Sprite sprite = null;
	[Header("���L���X�g����")]
	public float recastTime = 0.0f;
	[Header("�ݒu����ꍇ�̓v���n�u")]
	public GameObject objectPrefab = null;

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

