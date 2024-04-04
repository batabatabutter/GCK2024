using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("�u���b�N�̑ϋv")]
    [SerializeField] private float m_blockEndurance = 100;

    [Header("�j��s��")]
    [SerializeField] private bool m_dontBroken = false;

    [System.Serializable]
    struct DropItems
    {
        [Header("�A�C�e���̎��")]
        public ItemData.Type type;
        [Header("�h���b�v��"), Min(0)]
        public int count;
        [Header("�h���b�v��"), Range(0f, 1f)]
        public float rate;
    }

    [Header("�h���b�v����A�C�e��")]
    [SerializeField] private DropItems[] m_dropItems = null;
    [Header("�A�C�e���̃f�[�^�x�[�X")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;

    [Header("�������g�̔�����������x��")]
    [SerializeField] private int m_lightLevel = 0;
    [Header("�󂯂Ă���������x��")]
    [SerializeField] private int m_receiveLightLevel = 0;

    [Header("�X�v���C�g�����_�[")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    // �u���b�N���j�󂳂�Ă���
    private bool m_isBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        // �X�v���C�g�����_�[���Ȃ���Ύ擾
        if (!m_spriteRenderer)
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
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
        
        // �󂯂Ă��閾�邳���x���ɉ����ĐF��ݒ�
        if (m_receiveLightLevel > 0)
        {
            // �����x
            float alpha = m_receiveLightLevel / 7.0f * 100.0f;
            // �F��ݒ�
            m_spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }

    }

	/// <summary>
	/// �̌@�_���[�W
	/// </summary>
	/// <param name="power"></param>
	/// <returns></returns>
	public virtual bool AddMiningDamage(float power)
    {
        // �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
        if (m_dontBroken)
            return false;

        // �̌@�_���[�W���Z
        m_blockEndurance -= power;

        // �ϋv��0�ɂȂ���
        if (m_blockEndurance <= 0.0f)
        {
            return BrokenBlock();
        }

        return false;
    }

	// �A�C�e���h���b�v
	public virtual void DropItem()
	{
        foreach (DropItems dropItem in m_dropItems)
        {
            // 0 ~ 1�����擾
            float random = Random.value;

            // �������h���b�v�m�����傫��
            if (dropItem.rate < random)
                continue;

            // �A�C�e���̃f�[�^���擾
            ItemData data = MyFunction.GetItemData(m_itemDataBase, dropItem.type);

			// �A�C�e���̃Q�[���I�u�W�F�N�g�𐶐�
			GameObject obj = Instantiate(data.prefab, transform.position, Quaternion.identity);

            // ���邳�̊T�O��ǉ�
            obj.AddComponent<ChangeBrightness>();

            // ���O��ς���
            obj.name = "Material_" + dropItem.type.ToString();

            // �A�C�e�����h���b�v�����Ƃ��̏���
            if (obj.TryGetComponent(out Item item))
            {
                // ��ނ̐ݒ�
                item.ItemType = dropItem.type;
                // �h���b�v���̐ݒ�
                item.Drop(dropItem.count);
            }

            // �摜��ݒ�
            if (obj.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.sprite = data.sprite;
            }
        }
	}



	/// <summary>
	/// �u���b�N��j��
	/// </summary>
	/// <returns>�u���b�N����ꂽ</returns>
	public bool BrokenBlock()
	{
		// �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
		if (m_dontBroken)
			return false;

        // ���łɔj�󂳂�Ă���
        if (m_isBroken)
            return false;

		// �A�C�e���h���b�v
		DropItem();

		// ���g���폜
		Destroy(gameObject);
        m_isBroken = true;

        return true;
	}



	// �j��s�\��
	public bool DontBroken
    {
        get { return m_dontBroken; }
        set { m_dontBroken = value; }
    }

    // ���g�̎��������x��
    public int LightLevel
    {
        get { return m_lightLevel; }
    }

    // �󂯂Ă��閾�邳
    public int ReceiveLightLevel
    {
        get { return m_receiveLightLevel; }
        set { m_receiveLightLevel = value; }
    }

}
