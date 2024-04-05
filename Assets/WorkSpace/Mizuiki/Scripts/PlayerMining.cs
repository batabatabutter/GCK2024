using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class PlayerMining : MonoBehaviour
{
    // �̌@�Ɏg�p����l
    [System.Serializable]
    public struct MiningValue
    {
		[Header("�̌@�͈�(���a)")]
		public float range;
		[Header("�̌@��")]
		public float power;
		[Header("�̌@���x(/s)")]
		public float speed;
		[Header("�N���e�B�J����(%)")]
		public float critical;
        [Header("�N���e�B�J���_���[�W")]
        public float criticalDamage;
		[Header("�A�C�e���h���b�v��")]
		public float itemDrop;

        public static MiningValue operator+ (MiningValue left, MiningValue right)
        {
            MiningValue val = new()
            {
                range = left.range + right.range,
                power = left.power + right.power,
                speed = left.speed + right.speed,
                critical = left.critical + right.criticalDamage,
                criticalDamage = left.criticalDamage + right.criticalDamage,
                itemDrop = left.itemDrop + right.itemDrop,
            };
            return val;
        }
        public static MiningValue operator* (MiningValue left, MiningValue right)
        {
			MiningValue val = new()
			{
				range = left.range * right.range,
				power = left.power * right.power,
				speed = left.speed * right.speed,
				critical = left.critical * right.critical,
                criticalDamage = left.criticalDamage * right.criticalDamage,
				itemDrop = left.itemDrop * right.itemDrop
			};
			return val;
        }
    }

    [Header("���C���[�}�X�N")]
    [SerializeField] private LayerMask m_layerMask;

    [Header("��b�l")]
    [SerializeField] private MiningValue m_miningValueBase;
    [Header("�{��")]
    [SerializeField] private MiningValue m_miningValueRate;
    [Header("�����l")]
    [SerializeField] private MiningValue m_miningValueBoost;

    // �ŏI�I�ȍ̌@�l
    private MiningValue m_miningValue;

    //[Header("�̌@�͈�(���a)")]
    //[SerializeField] private float m_miningRange = 2.0f;
    //[Header("�̌@�͈͔{��")]
    //[SerializeField] private float m_miningRangeRate = 1.0f;

    //[Header("�̌@��")]
    //[SerializeField] private float m_miningPower = 1.0f;
    //[Header("�̌@�͔{��")]
    //[SerializeField] private float m_miningPowerRate = 1.0f;

    //[Header("�̌@���x(/s)")]
    //[SerializeField] private float m_miningSpeed = 1.0f;
    private float m_miningCoolTime = 0.0f;
    //[Header("�̌@���x�{��")]
    //[SerializeField] private float m_miningSpeedRate = 1.0f;

    //[Header("�N���e�B�J����(%)")]
    //[SerializeField] private float m_criticalRate = 0.0f;
    //[Header("�N���e�B�J���_���[�W(%)")]
    //[SerializeField] private float m_criticalDamageRate = 2.0f;

    //[Header("�A�C�e���h���b�v��")]
    //[SerializeField] private int m_itemDropCount = 1;
    //[Header("�A�C�e���h���b�v��")]
    //[SerializeField] private float m_itemDropRate = 1.0f;

    // �̌@��
    private int m_miningCount = 0;
    // �u���b�N�̔j��
    private int m_brokenCount = 0;
    // �^�����_���[�W
    private float m_takenDamage = 0.0f;


    [Header("�f�o�b�O�\��")]
    [SerializeField] private GameObject m_debugMiningRange;
    [SerializeField] private GameObject m_debugMiningPoint;

    // Start is called before the first frame update
    void Start()
    {
        // �̌@�l�̌v�Z
        m_miningValue = m_miningValueBase * m_miningValueRate * m_miningValueBoost;

        // �̌@�͈͂̐ݒ�
        m_debugMiningRange.transform.localScale = new Vector3(m_miningValue.range * 2.0f, m_miningValue.range * 2.0f, m_miningValue.range * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
		// �̌@�l�̌v�Z
		m_miningValue = m_miningValueBase * m_miningValueRate * m_miningValueBoost;

		// �̌@�N�[���^�C��
		if (m_miningCoolTime > 0.0f)
        {
            m_miningCoolTime -= Time.deltaTime;
        }

        // �v���C���[�̈ʒu
        Vector2 playerPos = transform.position;

        // �}�E�X�̈ʒu���擾
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // �v���C���[�̈ʒu����}�E�X�̈ʒu�ւ̃x�N�g��
        Vector2 playerToMouse = mousePos - playerPos;
        // �x�N�g�����K��
        playerToMouse.Normalize();

        // �v���C���[����̌@�����ւ�RayCast
        RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_miningValue.range, m_layerMask);
        // �����������̂�����Γ��������ʒu���̌@�|�C���g
        if (rayCast)
        {
            m_debugMiningPoint.transform.position = rayCast.point;
        }
        // �����������̂��Ȃ����
        else
        {
            // �̌@�|�C���g
            m_debugMiningPoint.transform.position = playerPos + (playerToMouse * m_miningValue.range);
        }



	}

    // �̌@����
    public void Mining()
    {
        // �̌@�N�[���^�C����
        if (m_miningCoolTime > 0.0f)
            return;

		// �v���C���[�̈ʒu
		Vector2 playerPos = transform.position;

		// �}�E�X�̈ʒu���擾
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// �v���C���[�̈ʒu����}�E�X�̈ʒu�ւ̃x�N�g��
		Vector2 playerToMouse = mousePos - playerPos;
		// �x�N�g�����K��
		playerToMouse.Normalize();

        // �v���C���[����̌@�����ւ�RayCast
        RaycastHit2D[] rayCasts = Physics2D.RaycastAll(playerPos, playerToMouse, m_miningValue.range, m_layerMask);
        foreach (RaycastHit2D rayCast in rayCasts)
        {
            // �^�O�� Block
            if (rayCast.transform.CompareTag("Block"))
            {
                // �u���b�N�Ƀ_���[�W��^����
                if (CauseDamageToBlock(rayCast.transform))
                {
                    // �^�_���[�W�ɉ��Z
                    m_takenDamage += m_miningValue.power;

                    // ��Ԏ�O�̃u���b�N�Ƀ_���[�W��^����
                    break;
                }

                continue;
            }

            // �^�O�� Tool
            if (rayCast.transform.CompareTag("Tool"))
            {
                // �}�E�X�J�[�\���Ɠ����O���b�h
                if (MyFunction.CheckSameGrid(rayCast.transform.position, mousePos))
				{
                    // �c�[���Ƀ_���[�W��^����
                    CauseDamageToBlock(rayCast.transform);
                    break;
				}

			}
		}

		// �N�[���^�C���ݒ�
		m_miningCoolTime = 1.0f / (m_miningValue.speed);

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
			if (block.AddMiningDamage(GetPower(), (int)(m_miningValue.itemDrop)))
			{
				// �j��񐔉��Z
				m_brokenCount++;
			}

			// �̌@�񐔉��Z
			m_miningCount++;

			// �u���b�N�ɓ���������_���[�W�����𔲂���
			return true;

		}

        return false;
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
            power *= m_miningValue.criticalDamage;
        }

        // �̌@�͂�Ԃ�
        return power;
    }


    // ��b�l
    public MiningValue MiningValueBase
    {
        get { return m_miningValueBase; }
        set { m_miningValueBase = value; }
    }
    // �{��
    public MiningValue MiningValueRate
    {
        get { return m_miningValueRate; }
        set { m_miningValueRate = value; }
    }
    // �����l
    public MiningValue MiningValueBoost
    {
        get { return m_miningValueBoost; }
        set { m_miningValueBoost = value; }
    }

    // �̌@��
    public float MiningPower
    {
        get { return m_miningValue.power; }
    }

    //// �̌@���x
    //public float MiningSpeed
    //{
    //    get { return m_miningValue.speed; }
    //    set { m_miningValue.speed = value; }
    //}
    //// �̌@���x�̔{��
    //public float MiningSpeedRate
    //{
    //    get { return m_miningSpeedRate; }
    //    set { m_miningSpeedRate = value; }
    //}

    //// �N���e�B�J����
    //public float CriticalRate
    //{
    //    get { return m_criticalRate; }
    //    set { m_criticalRate = value; }
    //}

    //// �A�C�e���h���b�v��
    //public float ItemDropRate
    //{
    //    set { m_itemDropRate = value; }
    //}

    // �̌@��
    public int MiningCount
    {
        get { return m_miningCount; }
        set { m_miningCount = value; }
    }
    // �j��
    public int BrokenCount
    {
        get { return m_brokenCount; }
    }
    // �^�����_���[�W
    public float TakenDamage
    {
        get { return m_takenDamage; }
    }

}
