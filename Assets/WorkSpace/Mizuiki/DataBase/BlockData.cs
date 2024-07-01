using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockData_", menuName = "CreateDataBase/Block/BlockData")]
public class BlockData : ScriptableObject
{
	[System.Serializable]
	public enum BlockType
	{
		STONE = 0,      // ��

		ORE_BEGIN = 100,// ��������z��
		COAL,           // �ΒY
		COPPER,         // ��
		TIN,            // ��
		LEAD,           // ��
		IRON,           // �S
		STEEL,          // �|
		ORE_END,		// �z�ΏI��

		BIRTHDAY		= 1000,     // ��������a���΃V���[�Y
		BIR_GARNET		= 1010,     // �K�[�l�b�g
		BIR_AMETHYST	= 1020,		// �A���W�X�g
		BIR_AQUAMARINE	= 1030,     // �A�N�A�}����
		BIR_DIAMOND		= 1040,     // �_�C�������h
		BIR_EMERALD		= 1050,     // �G�������h
		BIR_PEARL		= 1060,     // �p�[��
		BIR_RUBY		= 1070,     // ���r�[
		BIR_PERIDOT		= 1080,     // �y���h�b�g
		BIR_SAPPHIRE	= 1090,     // �T�t�@�C�A
		BIR_OPAL		= 1100,     // �I�p�[��
		BIR_TOPAZ		= 1110,     // �g�p�[�Y
		BIR_TURQUOISE	= 1120,     // �^�[�R�C�Y

		SPECIAL = 500,  // �����������u���b�N
		CORE,           // �R�A
		BEDROCK,        // ���
		DENGEROUS,      // �댯��

		TOACH = 10000,   // ����
		BOMB,           // ���e

		OVER
	}

	[System.Serializable]
	public struct DropItems
	{
		[Tooltip("�A�C�e���̎��"), CustomEnum(typeof(ItemData.ItemType))]
		public string typeStr;
		[NonSerialized]
		public ItemData.ItemType type;
		[Tooltip("�h���b�v��"), Min(0)]
		public int count;
		[Tooltip("�h���b�v��"), Range(0f, 1f)]
		public float rate;
	}

	[Header("�u���b�N��")]
	[SerializeField] private string blockName = "";
	[Header("�u���b�N�̎��"), CustomEnum(typeof(BlockType))]
	[SerializeField] private string typeStr = "";
	private BlockType type;
	[Header("�F")]
	[SerializeField] private Color color = Color.white;
	[Header("�u���b�N�̑ϋv��")]
	[SerializeField] private float endurance = 100.0f;
	[Header("�j��s�\")]
	[SerializeField] private bool dontBroken = false;
	[Header("�߈ˉ\")]
	[SerializeField] private bool canPossess = true;
	[Header("�������x��")]
	[SerializeField] private int lightLevel = 0;
	[Header("�u���b�N�̃v���n�u")]
	[SerializeField] private GameObject prefab = null;
	[Header("�u���b�N�̉摜")]
	[SerializeField] private Sprite sprite = null;
	[Header("�̌@��")]
    [SerializeField] private AudioClip miningSE = null;
    [Header("�j��")]
    [SerializeField] private AudioClip destroySE = null;

    [Header("�h���b�v�A�C�e��")]
	[SerializeField] private DropItems[] dropItem;

	[Header("---�}�b�v---")]

	[Header("�\����")]
	[SerializeField] private int order = 0;
	[Header("�}�b�v�\���A�C�R��(����ΐݒ�)")]
	[SerializeField] private Sprite mapIcon = null;

	public string Name => blockName;
	public BlockType Type => type;
	public float Endurance => endurance;
	public bool DontBroken => dontBroken;
	public bool CanPossess => canPossess;
	public int LightLevel => lightLevel;
	public GameObject Prefab => prefab;
	public Sprite Sprite => sprite;
	public AudioClip MiningSE => miningSE;
	public AudioClip DestroySE => destroySE;
	public DropItems[] DropItem => dropItem;
	public Color Color => color;
	public int Order => order;
	public Sprite MapIcon => mapIcon;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<BlockType>(typeStr);

		for (int i = 0; i < dropItem.Length; i++)
		{
			dropItem[i].type = SerializeUtil.Restore<ItemData.ItemType>(dropItem[i].typeStr);
		}
	}

	public BlockData(BlockData block)
	{
		blockName = block.blockName;
		typeStr = block.typeStr;
		endurance = block.endurance;
		dontBroken = block.dontBroken;
		lightLevel = block.lightLevel;
		prefab = block.prefab;
		sprite = block.sprite;
		dropItem = block.dropItem;

		color = block.color;
		order = block.order;
		mapIcon = block.mapIcon;
	}

}

