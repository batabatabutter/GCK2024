using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("�u���b�N�̑ϋv")]
    [SerializeField] private float m_blockEndurance = 100;

    [Header("�j��s��")]
    [SerializeField] private bool m_dontBroken = false;

    [Header("�h���b�v����A�C�e��")]
    [SerializeField] private List<GameObject> m_dropItems = new List<GameObject>();

    [Header("�������g�̌������x��")]
    [SerializeField] private int m_lightLevel = 0;

    [Header("�}�b�v�\���p�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject m_mapObject = null;
    [SerializeField] private GameObject m_mapBlind = null;
    [SerializeField] private Color m_blockColor = Color.white;
    [SerializeField] private int m_order = 0;

    // �u���b�N���j�󂳂�Ă���
    private bool m_isBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        if (m_mapObject)
        {
            // �}�b�v�I�u�W�F�N�g�̐���
            GameObject mapObj = Instantiate(m_mapObject, transform);
            // �F�̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().color = m_blockColor;
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = m_order;
            mapObj.GetComponent<MapObject>().BlockColor = m_blockColor;
            // �X�v���C�g�̐ݒ�
            mapObj.GetComponent<MapObject>().ParentSprite = gameObject.GetComponent<SpriteRenderer>();
        }
        if (m_mapBlind)
        {
            // �}�b�v�̖ډB������
            Instantiate(m_mapBlind, transform);
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
	public virtual void DropItem(int count = 1)
	{
        foreach (GameObject dropItem in m_dropItems)
        {
            // �A�C�e���̃Q�[���I�u�W�F�N�g�𐶐�
            GameObject obj = Instantiate(dropItem);
            obj.transform.position = transform.position;

            // ���邳�̊T�O��ǉ�
            obj.AddComponent<ChangeBrightness>();

            // �A�C�e�����h���b�v�����Ƃ��̏���
            if (obj.TryGetComponent(out Item item))
            {
                item.Drop(count);
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

    // �������x��
    public int LightLevel
    {
        get { return m_lightLevel; }
    }

}
