using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class PlayerMining : MonoBehaviour
{
    // �̌@������Ray
    struct MiningRay
    {
        public Vector2 direction;
        public Vector2 origin;
        public float length;

        public readonly Vector2 MiningPos()
        {
            return origin + (direction * length);
        }
    }

    [Header("�ۂ̂�")]
    [SerializeField] private CircularSaw m_circularSaw = null;

    [Header("���C���[�}�X�N")]
    [SerializeField] private LayerMask m_layerMask;

    [Header("�{��")]
    [SerializeField] private MiningData.MiningValue m_miningValueRate;
    [Header("�����l")]
    [SerializeField] private MiningData.MiningValue m_miningValueBoost;

    [Header("�̌@�p�[�e�B�N��")]
    [SerializeField] private ParticleSystem m_miningParticle = null;

    // �ŏI�I�ȍ̌@�l
    private MiningData.MiningValue m_miningValue;

    // �̌@�̃N�[���^�C��
    private float m_miningCoolTime = 0.0f;

    // �̌@��
    private int m_miningCount = 0;
    // �^�����_���[�W
    private float m_takenDamage = 0.0f;


    [Header("�f�o�b�O�\��")]
    [SerializeField] private bool m_debug = true;
    [SerializeField] private GameObject m_debugMiningRange;
    [SerializeField] private GameObject m_debugMiningPoint;

    // Start is called before the first frame update
    void Start()
    {
        // �̌@�l�̌v�Z
        m_miningValue = m_circularSaw.GetMiningValue() * m_miningValueRate * m_miningValueBoost;

        if (m_debug)
        {
            m_debugMiningRange.SetActive(true);
            m_debugMiningPoint.SetActive(true);
            // �̌@�͈͂̐ݒ�
            m_debugMiningRange.transform.localScale = new Vector3(m_miningValue.range * 2.0f, m_miningValue.range * 2.0f, m_miningValue.range * 2.0f);
        }
        else
        {
            m_debugMiningRange.SetActive(false);
            m_debugMiningPoint.SetActive(false);
        }

        // �ۂ̂��̃T�C�Y�ݒ�
        SetCircularSawScale();
    }

    // Update is called once per frame
    void Update()
    {
        // �̌@�l�̌v�Z
        m_miningValue = m_circularSaw.GetMiningValue() * m_miningValueRate * m_miningValueBoost;

        // �̌@�N�[���^�C��
        if (m_miningCoolTime > 0.0f)
        {
            m_miningCoolTime -= Time.deltaTime;
        }

        // �}�E�X�̈ʒu���擾
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // �̌@�ʒu
        Vector2 miningPoint = Vector2.zero;

        // �̌@�p��Ray�擾
        MiningRay miningRay = GetMiningPoint(mousePos);

        // �v���C���[����̌@�����ւ�RayCast
        RaycastHit2D[] rayCasts = Physics2D.RaycastAll(miningRay.origin, miningRay.direction, miningRay.length, m_layerMask);
        bool hit = false;
        foreach (RaycastHit2D rayCast in rayCasts)
        {
            // �^�O�� Block
            if (rayCast.transform.CompareTag("Block"))
            {
                // ���������ʒu���̌@�|�C���g�ɂ���
                miningPoint = rayCast.point;
                hit = true;
                break;
            }

            // �^�O�� Tool
            if (rayCast.transform.CompareTag("Tool"))
            {
                // �}�E�X�J�[�\���Ɠ����O���b�h
                if (MyFunction.CheckSameGrid(rayCast.transform.position, mousePos))
                {
                    miningPoint = rayCast.transform.position;
                    hit = true;
                    break;
                }
            }
        }
        // �����������̂��Ȃ�
        if (!hit)
        {
            // �̌@�|�C���g
            miningPoint = miningRay.origin + (miningRay.direction * miningRay.length);
        }

        // �ۂ̂��̃T�C�Y�ݒ�
        SetCircularSawScale();

        // �̌@�|�C���g�ݒ�
        m_circularSaw.SetPosition(miningPoint);

        if (m_debug)
        {
            m_debugMiningPoint.transform.position = miningPoint;
        }

    }

    // �̌@����
    public void Mining()
    {
        // ��]������
        m_circularSaw.Rotate(m_miningValue.speed);

        // �̌@�N�[���^�C����
        if (m_miningCoolTime > 0.0f)
            return;

        // �͈͍̌@
        MiningOfRange(m_circularSaw.transform.position);

        // �N�[���^�C���ݒ�
        m_miningCoolTime = 1.0f / m_miningValue.speed;

    }

    // �̌@�x�N�g���̎擾
    public Vector2 GetMiningVector()
    {
        return m_circularSaw.transform.position - transform.position;
    }


    // �{��
    public MiningData.MiningValue MiningValueRate
    {
        get { return m_miningValueRate; }
        set { m_miningValueRate = value; }
    }
    // �����l
    public MiningData.MiningValue MiningValueBoost
    {
        get { return m_miningValueBoost; }
        set { m_miningValueBoost = value; }
    }

    // �̌@�͈�
    public float MiningRange
    {
        get { return m_miningValue.range; }
    }
    // �̌@��
    public float MiningPower
    {
        get { return m_miningValue.power; }
    }

    // �̌@��
    public int MiningCount
    {
        get { return m_miningCount; }
        set { m_miningCount = value; }
    }
    // �^�����_���[�W
    public float TakenDamage
    {
        get { return m_takenDamage; }
    }






    private MiningRay GetMiningPoint(Vector2 mousePos)
    {
        // �v���C���[�̈ʒu
        Vector2 playerPos = transform.position;

        // �v���C���[�̈ʒu����}�E�X�̈ʒu�ւ̃x�N�g��
        Vector2 playerToMouse = mousePos - playerPos;
        // �v���C���[����}�E�X�܂ł̋���
        float length = playerToMouse.magnitude;
        if (length > m_miningValue.range)
        {
            length = m_miningValue.range;
        }
        // �x�N�g�����K��
        playerToMouse.Normalize();

        MiningRay miningRay = new()
        {
            direction = playerToMouse,
            origin = playerPos,
            length = length,
        };

        return miningRay;
    }

    /// <summary>
    /// �u���b�N�Ƀ_���[�W��^����
    /// </summary>
    /// <param name="transform">�_���[�W��^����u���b�N</param>
    /// <returns>�_���[�W���ʂ�����</returns>
    private bool CauseDamageToBlock(Transform transform)
    {
        // [Block] �̎擾�����݂�
        if (transform.TryGetComponent(out Block block))
        {
            // �̌@�_���[�W���Z
            if (!block.AddMiningDamage(GetPower(), (int)(m_miningValue.itemDrop / 100.0f)))
                return false;

            // �̌@�񐔉��Z
            m_miningCount++;
            // �^�_���[�W���Z
            m_takenDamage += m_miningValue.power;

            // �̌@�G�t�F�N�g
            if (m_miningParticle)
            {
                Instantiate(m_miningParticle.gameObject, transform.position, Quaternion.identity);
            }

            // �u���b�N�ɓ���������_���[�W�����𔲂���
            return true;

        }

        return false;
    }

    // �L�͈͂̍̌@
    private void MiningOfRange(Vector2 center)
    {
        // �u���b�N�̎擾
        Collider2D[] blocks = Physics2D.OverlapCircleAll(center, m_miningValue.size / 2.0f, LayerMask.GetMask("Block"));

        foreach (Collider2D block in blocks)
        {
            // �_���[�W
            // �^�O�� Block
            if (block.transform.CompareTag("Block"))
            {
                // �u���b�N�Ƀ_���[�W��^����
                CauseDamageToBlock(block.transform);
                continue;
            }
            // �^�O�� Tool
            if (block.transform.CompareTag("Tool"))
            {
                // �}�E�X�J�[�\���Ɠ����O���b�h
                if (MyFunction.CheckSameGrid(block.transform.position, center))
                {
                    // �c�[���Ƀ_���[�W��^����
                    CauseDamageToBlock(block.transform);
                    break;
                }
            }
        }

    }

    // �̌@�͎Z�o
    private float GetPower()
    {
        // �̌@��
        float power = m_miningValue.power;

        // 0 ~ 100%
        float rand = Random.Range(0, 100);

        // �o�ڂ��N���e�B�J������菬����
        if (rand <= m_miningValue.critical)
        {
            // �_���[�W�{����������
            power *= m_miningValue.criticalDamage / 100.0f;
        }

        // �̌@�͂�Ԃ�
        return power;
    }

    // �ۂ̂��̃T�C�Y�ݒ�
    private void SetCircularSawScale()
    {
        // �X�P�[��
        float scale = Mathf.Max(m_miningValue.size, 1.0f);

        // �ۂ̂��̃T�C�Y�ݒ�
        m_circularSaw.transform.localScale = Vector3.one * scale;

    }

}
