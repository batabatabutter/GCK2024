using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�G�f�[�^�x�[�X")]
    [SerializeField] EnemyData m_enemyData;

    [Header("�A�C�e���̃f�[�^�x�[�X")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;


    //�U���Ԋu
    float m_attackCoolTime;

    //�h���b�v�A�C�e��
    List<BlockData.DropItems> m_dropItems;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_attackCoolTime = m_enemyData.coolTime;

        m_dropItems = m_enemyData.dropItems;

    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (m_attackCoolTime < 0)
        {
            //�U��
            Attack();

            m_attackCoolTime = m_enemyData.coolTime;
        }
        else
        {
            m_attackCoolTime -= Time.deltaTime;
        }


    }

    public virtual void Attack()
    {

    }
    public virtual void Dead()
    {
        DropItem();

        Destroy(gameObject);
    }

    public virtual void DropItem()
    {
        //�A�C�e�����Ƃ�

        foreach (BlockData.DropItems dropItem in m_dropItems)
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


        }


    }
}