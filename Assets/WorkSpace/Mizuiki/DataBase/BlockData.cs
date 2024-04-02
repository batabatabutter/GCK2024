using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData", menuName = "CreateBlock")]
public class BlockData : ScriptableObject
{
	[System.Serializable]
	public enum ToolType
	{
		CORE,		// �R�A
		BEDROCK,	// ���
		STONE,		// ��
		COAL,		// �ΒY

		OVER
	}

	[Header("�u���b�N��")]
	public string blockName = "";
	[Header("�u���b�N�̎��")]
	public ToolType type = ToolType.OVER;
	[Header("�u���b�N�̉摜")]
	public Sprite sprite = null;
	[Header("�u���b�N�̃v���n�u")]
	public GameObject prefab = null;

	[Header("---�}�b�v---")]

	[Header("�}�b�v�\���̐F")]
	public Color color = Color.white;
	[Header("�\����")]
	public int order = 0;
	[Header("�}�b�v�\���A�C�R��(����ΐݒ�)")]
	public Sprite mapIcon = null;

	public BlockData(BlockData block)
	{
		blockName = block.blockName;
		sprite = block.sprite;
	}

}

