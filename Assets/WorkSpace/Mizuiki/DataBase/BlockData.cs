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
		STEEL,          // �S
		TIN,            // ��
		LEAD,           // ��
		ORE_END,		// �z�ΏI��

		BIRTHDAY = 200,     // ��������a���΃V���[�Y
		BIR_GARNET,         // �K�[�l�b�g
		BIR_AMETHYST,       // �A���W�X�g
		BIR_AQUAMARINE,     // �A�N�A�}����
		BIR_DIAMOND,        // �_�C�������h
		BIR_EMERALD,        // �G�������h
		BIR_PEARL,          // �p�[��
		BIR_RUBY,           // ���r�[
		BIR_PERIDOT,        // �y���h�b�g
		BIR_SAPPHIRE,       // �T�t�@�C�A
		BIR_OPAL,           // �I�p�[��
		BIR_TOPAZ,          // �g�p�[�Y
		BIR_TURQUOISE,      // �^�[�R�C�Y

		SPECIAL = 500,  // �����������u���b�N
		CORE,           // �R�A
		BEDROCK,        // ���
		DENGEROUS,      // �댯��

		TOACH = 1000,   // ����
		BOMB,           // ���e

		OVER
	}

	[System.Serializable]
	public struct DropItems
	{
		[Header("�A�C�e���̎��")]
		public ItemData.ItemType type;
		[Header("�h���b�v��"), Min(0)]
		public int count;
		[Header("�h���b�v��"), Range(0f, 1f)]
		public float rate;
	}

	[Header("�u���b�N��")]
	[SerializeField] private string blockName = "";
	[Header("�u���b�N�̎��"), CustomEnum(typeof(BlockType))]
	[SerializeField] private string typeStr = "";
	private BlockType type;
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

	[Header("�h���b�v�A�C�e��")]
	[SerializeField] private DropItems[] dropItem;

	[Header("---�}�b�v---")]

	[Header("�}�b�v�\���̐F")]
	[SerializeField] private Color color = Color.white;
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
	public DropItems[] DropItem => dropItem;
	public Color Color => color;
	public int Order => order;
	public Sprite MapIcon => mapIcon;

	private void OnEnable()
	{
		type = SerializeUtil.Restore<BlockType>(typeStr);
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

