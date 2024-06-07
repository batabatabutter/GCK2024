using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// ��Ԑe
/// </summary>
public class Enemy : ObjectAffectLight
{
    //���
    public enum Type
    {
        Dorinoko,
        Iwarun,
        Hotarun,
        Bomurun,

        OverID
    }
    //�n��
    public enum System
    {
        Dwell,//�h��^
        Mob, //Mob�^

        OverID
    }

    [Header("�G�f�[�^�x�[�X")]
    [SerializeField] protected EnemyData m_enemyData;

    [Header("�A�C�e���̃f�[�^�x�[�X")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;

    [Header("��Q�����C���[")]
    [SerializeField] private LayerMask m_blockLayer;

    //�v���C���[
    private GameObject m_player = null;

    public GameObject Player
    {
        get
        {
            return m_player;
        }
        set
        {
            m_player = value;
        }
    }


    //�U���Ԋu
    protected float m_attackCoolTime;

    //�h���b�v�A�C�e��
    List<BlockData.DropItems> m_dropItems;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_attackCoolTime = m_enemyData.coolTime;

        m_dropItems = m_enemyData.dropItems;


        // �T�[�N���R���C�_�[2D���A�^�b�`����Ă��Ȃ��ꍇ�A�ǉ�����
        if (GetComponent<CircleCollider2D>() == null)
        {
            // �T�[�N���R���C�_�[2D��ǉ�����
            CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();

            // �R���C�_�[�̔��a��ݒ肷��
            circleCollider.radius = m_enemyData.radius;

            // �R���C�_�[���g���K�[�Ƃ��ē��삷��
            circleCollider.isTrigger = true; 
        }
        else
        {
            GetComponent<CircleCollider2D>().radius = m_enemyData.radius;
        }

        //  ����������
        AudioManager.Instance.PlaySE(m_enemyData.GenerateSE, transform.position);
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
            float length = m_enemyData.radius;
            Transform startTransform = transform;

            if (m_player && Vector3.Distance(startTransform.position, m_player.transform.position) < length)
            {
                Vector3 direction = m_player.transform.position - startTransform.position;

                RaycastHit hit;
                // ���C���΂�
                if (!Physics.Raycast(startTransform.position, direction, out hit, length, m_blockLayer))
                {
                    // �����f�o�b�O�\��
                    Debug.DrawLine(startTransform.position, m_player.transform.position, Color.red);
                    // �����ŏ������s���i�u���b�N����Ă��Ȃ��ꍇ�̏����j
                    m_attackCoolTime -= Time.deltaTime;
                }
            }
        }
    }

    public virtual void Attack()
    {

    }
    public virtual void Dead()
    {
        DropItem();

        AudioManager.Instance.PlaySE(m_enemyData.DeathSE, transform.position);
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
            GameObject obj = Instantiate(data.Prefab, transform.position, Quaternion.identity);

            ////  ���邳������
            //obj.GetComponent<ObjectAffectLight>().BrightnessFlag = BrightnessFlag;

            // �摜��ݒ�
            if (obj.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.sprite = data.Sprite;
            }

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


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && m_player == null)
        {
            m_player = collision.gameObject;

        }
    }
}