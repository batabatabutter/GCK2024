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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �̌@�_���[�W
    public void AddMiningDamage(float power)
    {
        // �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
        if (m_dontBroken)
            return;

        // �̌@�_���[�W���Z
        m_blockEndurance -= power;

        // �ϋv��0�ɂȂ���
        if (m_blockEndurance <= 0.0f)
        {
            BrokenBlock();
        }
    }


	// �u���b�N����ꂽ
	private void BrokenBlock()
    {
        // �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
        if (m_dontBroken)
            return;

        // �A�C�e���h���b�v
        DropItem();

        // ���g���폜
        Destroy(gameObject);

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
