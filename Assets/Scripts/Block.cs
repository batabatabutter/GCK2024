using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : ObjectAffectLight
{
    [Header("�u���b�N�̑ϋv")]
    [SerializeField] private float m_blockEndurance = 100;

    [Header("�j��s��")]
    [SerializeField] private bool m_dontBroken = false;

    [Header("�u���b�N�̏��")]
    [SerializeField] private BlockData m_blockData = null;

    [Header("�A�C�e���̃f�[�^�x�[�X")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;

    //  �n�ʂ̃��C�g���
    private Ground m_ground = null;

    // �u���b�N���j�󂳂�Ă���
    private bool m_isBroken = false;


	private void Awake()
	{
		// �f�[�^�x�[�X���ݒ肳��ĂȂ�
		if (m_itemDataBase == null)
		{
			Debug.Log(gameObject.name + "�̃A�C�e���f�[�^�x�[�X��ݒ肵�Ă�");
		}
	}

    // Update is called once per frame
    void Update()
    {
		// �������g��j�󂷂�
		if (m_isBroken)
        {
            Destroy(gameObject);
        }

        //  ��������
        if (Ground)
            ReceiveLightLevel = m_ground.ReceiveLightLevel;
    }

	/// <summary>
	/// �̌@�_���[�W
	/// </summary>
	/// <param name="power"></param>
	/// <returns>�_���[�W��^������</returns>
	public virtual bool AddMiningDamage(float power, int dropCount = 1)
    {
        // �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
        if (m_dontBroken)
            return false;

        // �̌@�_���[�W���Z
        m_blockEndurance -= power;

        // �ϋv��0�ɂȂ���
        if (m_blockEndurance <= 0.0f)
        {
            BrokenBlock(dropCount);
        }

        return true;
    }

	// �A�C�e���h���b�v
	public virtual void DropItem(int dropCount = 1)
	{
        foreach (BlockData.DropItems dropItem in m_blockData.DropItem)
        {
            // 0 ~ 1�����擾
            float random = Random.value;

            // �������h���b�v�m�����傫��
            if (dropItem.rate < random)
                continue;

            // �A�C�e���̃f�[�^���擾
            ItemData data = MyFunction.GetItemData(m_itemDataBase, dropItem.type);

            // �f�[�^���Ȃ��ꍇ�̓h���b�v���Ȃ�
            if (data == null)
                continue;

			// �A�C�e���̃Q�[���I�u�W�F�N�g�𐶐�
			GameObject obj = Instantiate(data.Prefab, transform.position, Quaternion.identity);

            // ���O��ς���
            obj.name = "Material_" + dropItem.type.ToString();

            // �A�C�e�����h���b�v�����Ƃ��̏���
            if (obj.TryGetComponent(out Item item))
            {
                // ��ނ̐ݒ�
                item.ItemType = dropItem.type;
                // �h���b�v���̐ݒ�
                item.Drop(dropItem.count * dropCount);
            }

            //  ���邳������
            obj.GetComponent<ObjectAffectLight>().BrightnessFlag = BrightnessFlag;

            // �摜��ݒ�
            if (obj.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.sprite = data.Sprite;
            }
        }
	}



	/// <summary>
	/// �u���b�N��j��
	/// </summary>
	/// <returns>�u���b�N����ꂽ</returns>
	public bool BrokenBlock(int dropCount = 1)
	{
		// �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
		if (m_dontBroken)
			return false;

        // ���łɔj�󂳂�Ă���
        if (m_isBroken)
            return false;

		// �A�C�e���h���b�v
		DropItem(dropCount);

		// ���g���폜
		Destroy(gameObject);
        m_isBroken = true;

        return true;
	}


    // �ϋv��
    public float Endurance
    {
        set { m_blockEndurance = value; }
    }

    // �j��s�\��
    public bool DontBroken
    {
        get { return m_dontBroken; }
        set { m_dontBroken = value; }
    }

    // �u���b�N�f�[�^
    public BlockData BlockData
    {
        get { return m_blockData; }
        set { m_blockData = value; }
    }

    //  �n��
    public Ground Ground 
    { 
        get { return m_ground; } 
        set { m_ground = value; }
    }

}
