using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class PlayerMining : MonoBehaviour
{
    [Header("���C���[�}�X�N")]
    [SerializeField] private LayerMask m_layerMask;

    [Header("�̌@�͈�(���a)")]
    [SerializeField] private float m_miningRange = 2.0f;
    [Header("�̌@�͈͔{��")]
    [SerializeField] private float m_miningRangeRate = 1.0f;

    [Header("�̌@��")]
    [SerializeField] private float m_miningPower = 1.0f;
    [Header("�̌@�͔{��")]
    [SerializeField] private float m_miningPowerRate = 1.0f;

    [Header("�̌@���x(/s)")]
    [SerializeField] private float m_miningSpeed = 1.0f;
    private float m_miningCoolTime = 0.0f;
    [Header("�̌@���x�{��")]
    [SerializeField] private float m_miningSpeedRate = 1.0f;

    [Header("�N���e�B�J����(%)")]
    [SerializeField] private float m_criticalRate = 0.0f;
    [Header("�N���e�B�J���_���[�W(%)")]
    [SerializeField] private float m_criticalDamageRate = 2.0f;

    // �̌@��
    private int m_miningCount = 0;
    // �u���b�N�̔j��
    private int m_brokenCount = 0;


    [Header("�f�o�b�O�\��")]
    [SerializeField] private GameObject m_debugMiningRange;
    [SerializeField] private GameObject m_debugMiningPoint;

    // Start is called before the first frame update
    void Start()
    {
        // �̌@�͈͂̐ݒ�
        m_debugMiningRange.transform.localScale = new Vector3(m_miningRange * 2.0f, m_miningRange * 2.0f, m_miningRange * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
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
        RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_miningRange * m_miningRangeRate, m_layerMask);
        // �����������̂�����Γ��������ʒu���̌@�|�C���g
        if (rayCast)
        {
            m_debugMiningPoint.transform.position = rayCast.point;
        }
        // �����������̂��Ȃ����
        else
        {
            // �̌@�|�C���g
            m_debugMiningPoint.transform.position = playerPos + (playerToMouse * m_miningRange * m_miningRangeRate);
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
        RaycastHit2D[] rayCasts = Physics2D.RaycastAll(playerPos, playerToMouse, m_miningRange * m_miningRangeRate, m_layerMask);
        foreach (RaycastHit2D rayCast in rayCasts)
        {
            // �^�O�� Block
            if (rayCast.transform.CompareTag("Block"))
            {
                // �u���b�N�Ƀ_���[�W��^����
                if (CauseDamageToBlock(rayCast.transform))
                {
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
                    CauseDamageToBlock(rayCast.transform);
                    break;
				}

			}
		}

		// �N�[���^�C���ݒ�
		m_miningCoolTime = 1.0f / (m_miningSpeed * m_miningSpeedRate);

	}



    // �u���b�N�Ƀ_���[�W��^����
    private bool CauseDamageToBlock(Transform transform)
    {
		// [Block] �̎擾�����݂�
		if (transform.TryGetComponent(out Block block))
		{
			// �̌@�_���[�W���Z
			if (block.AddMiningDamage(GetPower()))
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
        float power = m_miningPower * m_miningPowerRate;

        // 0 ~ 100%
        float rand = Random.Range(0, 100);

        // �o�ڂ��N���e�B�J������菬����
        if (rand <= m_criticalRate)
        {
            // �_���[�W�{����������
            power *= m_criticalDamageRate;
        }

        // �̌@�͂�Ԃ�
        return power;
    }


    // �̌@���x
    public float MiningSpeed
    {
        get { return m_miningSpeed; }
        set { m_miningSpeed = value; }
    }
    // �̌@���x�̔{��
    public float MiningSpeedRate
    {
        get { return m_miningSpeedRate; }
        set { m_miningSpeedRate = value; }
    }

    // �̌@��
    public float MiningPower
    {
        get { return m_miningPower; }
        set { m_miningPower = value; }
    }

    // �N���e�B�J����
    public float CriticalRate
    {
        get { return m_criticalRate; }
        set { m_criticalRate = value; }
    }

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

}
