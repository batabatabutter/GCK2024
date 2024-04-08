using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData", menuName = "CreateBlock")]
public class BlockData : ScriptableObject
{
	[System.Serializable]
	public enum BlockType
	{
		STONE = 0,		// ��
		COAL,			// �ΒY
		STEEL,			// �S
		TIN,			// ��
		LEAD,			// ��

		SPECIAL = 100,	// �����������u���b�N
		CORE,			// �R�A
		BEDROCK,		// ���
		DENGEROUS,		// �댯��

		BIRTHDAY_BLOCK = 200,	// ��������a���΃V���[�Y
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

		TOACH = 1000,	// ����
		BOMB,			// ���e

		OVER
	}

	[System.Serializable]
	public struct DropItems
	{
		[Header("�A�C�e���̎��")]
		public ItemData.Type type;
		[Header("�h���b�v��"), Min(0)]
		public int count;
		[Header("�h���b�v��"), Range(0f, 1f)]
		public float rate;
	}

	[Header("�u���b�N��")]
	public string blockName = "";
	[Header("�u���b�N�̎��")]
	public BlockType type = BlockType.OVER;
	[Header("�u���b�N�̑ϋv��")]
	public float endurance = 100.0f;
	[Header("�j��s�\")]
	public bool dontBroken = false;
	[Header("�������x��")]
	public int lightLevel = 0;
	[Header("�u���b�N�̃v���n�u")]
	public GameObject prefab = null;
	[Header("�u���b�N�̉摜")]
	public Sprite sprite = null;

	[Header("�h���b�v�A�C�e��")]
	public DropItems[] dropItem;

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

