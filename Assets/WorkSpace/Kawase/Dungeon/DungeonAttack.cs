using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DungeonAttack;
using static WaveManager;

public class DungeonAttack : MonoBehaviour
{
    //�v���C���[�̉��ɏo��n�C���C�g�̒Ⴓ
    const float HEIGLIGHT_HEIGHT = 0.5f;



    enum Direction
    {
        UP,
        RIGHT,
        DWON,
        LEFT
    }

    public enum AttackPattern
    {
        FallRock,
        RollRock,
        Bank,

        OverID

    }

    [Header("�_���W�����f�[�^�x�[�X")]
    [SerializeField] DungeonDataBase m_dungeonDataBase;

    [Header("--------------------------------------------")]

    [Header("core�̍U�����Ă��鋗��")]
    [SerializeField] float m_attackLength = 20;

    [Header("--------------------------------------------")]

    [Header("�V�[���}�l�[�W���[")]
    [SerializeField] PlaySceneManager m_sceneManager;
    [Header("--------------------------------------------")]

    [Header("����")]
    [SerializeField] GameObject m_fallRock;
    [Header("�n�C���C�g")]
    [SerializeField] GameObject m_fallRockHighlight;
    [Header("���΂̐������鍂��")]
    public float heightRock = 3.0f;
    [Header("�R�E�Q�L����")]
    [SerializeField] float m_fallTime = 4.0f;

    [Header("--------------------------------------------")]
    [Header("�]�����")]
    [SerializeField] GameObject m_rollRock;
    [Header("���̃n�C���C�g")]
    [SerializeField] GameObject m_rollRockhighlight;
    [Header("���̃n�C���C�g���o�����鋗��")]
    [Header("�ォ��")]
    [SerializeField] float m_up;
    [Header("�E����")]
    [SerializeField] float m_right;
    [Header("������")]
    [SerializeField] float m_down;
    [Header("������")]
    [SerializeField] float m_left;
    [Header("�R�E�Q�L����")]
    [SerializeField] float m_rollTime = 7.0f;

    [Header("--------------------------------------------")]
    [Header("�y��")]
    [SerializeField] GameObject m_bank;
    [Header("�y��n�C���C�g")]
    [SerializeField] GameObject m_bankHighlight;
    [Header("�R�E�Q�L����")]
    [SerializeField] float m_bankTime = 4.0f;

    //"�U���Ԋu"
    float m_attackCoolTime;

    //�N���[�^�C���L��
    float m_keepCoolTime;

    GameObject m_target;
    GameObject m_core;

    //�U���̑I��p
    bool m_isFall = false;
    bool m_isRoll = false;
    bool m_isBank = false;

    //�E�F�[�u
    int m_wave;

    //�E�F�[�u�}�l�[�W���[
    WaveManager m_waveManager;


    // Start is called before the first frame update
    void Start()
    {
        if (m_waveManager == null)
        {
            m_waveManager = GetComponent<WaveManager>();
        }
        //�E�F�[�u���̎擾
        m_wave = m_waveManager.WaveNum;
        //�^�Q�ƃR�A
        m_target = m_sceneManager.GetPlayer();
        m_core = m_sceneManager.GetCore();
        //�U���Ԋu
        m_keepCoolTime =
            m_dungeonDataBase.dungeonDatas[GetComponent<DungeonGenerator>().GetStageNum()].DungeonWaves[m_wave].geterateEnemyInterval;

        //�^�C�}�[�Z�b�g
        m_attackCoolTime = m_keepCoolTime;

        //�A�^�b�N�p�^�[���̎擾
        List<AttackPattern> attackPatterns = m_dungeonDataBase.
            dungeonDatas[GetComponent<DungeonGenerator>().GetStageNum()].
            DungeonWaves[m_wave].dungeonATKPattern;

        //bool�l�̂����Ă�
        SetAtkIs(attackPatterns);

    }

    // Update is called once per frame
    void Update()
    {
        //�E�F�[�u�ω��œ��e�X�V
        if(m_wave != GetComponent<WaveManager>().WaveNum)
        {
            Start();
        }


        int ratio = 1;

        //core�ƃv���C���[���߂��ƃR�E�Q�L��2�{
        if(m_core != null) 
        {
            if (Vector2.Distance(m_core.transform.position, m_target.transform.position) < m_attackLength)
            {
                ratio = 2;

            }
        }

        //�A�^�b�N��ԂŃ_���W�����U��
        if (m_waveManager.waveState == WaveState.Attack)
        {        
            //�N�[���^�C������
            m_attackCoolTime -= Time.deltaTime * ratio;
        }

        if (m_attackCoolTime < 0.0f)
        {
            //�R�E�Q�L�������ǂ���
            bool checkAttacked = false;
            //�������[�v�悯
            int outNum = 10;

            //�R�E�Q�L�ɍ��킹�����ԉ��Z
            float attackAddTime = 0.0f;

            //�R�E�Q�L���Ȃ������ꍇ�Ē��I
            while (!checkAttacked)
            {

                //�}�W�b�N�i���o�[�ōs�����Ē�����s
                int r = Random.Range(0, 3);

                if (m_isFall && r == 0)
                {
                    FallrockAttack();
                    checkAttacked = true;

                    attackAddTime = m_fallTime;
                }

                if (m_isRoll && r == 1)
                {
                    RockRollingAttack();
                    checkAttacked = true;

                    attackAddTime = m_rollTime;

                }
                if (m_isBank && r == 2)
                {
                    BankAttack();
                    checkAttacked = true;
                    attackAddTime = m_bankTime;

                }

                outNum--;
                //�S�ẴR�E�Q�L��off���Ƃ����ɂ͂��邩��
                if (outNum < 0)
                    break;
            }
            //�N�[���^�C�����Z�b�g
            m_attackCoolTime = m_keepCoolTime + attackAddTime;
        }
    }

    private void FallrockAttack()
    {
        Vector3 rockfallPos = new (m_target.transform.position.x, m_target.transform.position.y + heightRock, 0);
        Vector3 highlightPos = new (m_target.transform.position.x, m_target.transform.position.y - HEIGLIGHT_HEIGHT, 0);

        Instantiate(m_fallRock, rockfallPos, Quaternion.identity);
        Instantiate(m_fallRockHighlight, highlightPos, Quaternion.identity);
    }

    private void RockRollingAttack()
    {
        Vector3 rollingPos;

        float rollingRotation;

        //4�ʂ肾����O�`�R
        int rand = Random.Range(0, 4);

        if(rand == (int)Direction.UP)
        {
            rollingPos = new Vector3(m_target.transform.position.x,m_target.transform.position.y + m_up, 0);

            rollingRotation = 180;

        }
        else if(rand == (int)Direction.RIGHT)
        {
            rollingPos = new Vector3(m_target.transform.position.x + m_right, m_target.transform.position.y, 0);
            rollingRotation = 90;

        }
        else if (rand == (int)Direction.DWON)
        {
            rollingPos = new Vector3(m_target.transform.position.x, m_target.transform.position.y - m_down, 0);
            rollingRotation = 0;

        }
        else
        {
            rollingPos = new Vector3(m_target.transform.position.x - m_left, m_target.transform.position.y, 0);
            rollingRotation = 270;

        }


        Instantiate(m_rollRockhighlight, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
        Instantiate(m_rollRock, rollingPos, Quaternion.Euler(0, 0, rollingRotation));

    }
    private void BankAttack()
    {
        Vector3 pos = m_target.transform.position;

        pos.x -= 1;
        pos.y += 1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 1)
                    continue;
                    

                Instantiate(m_bank, new Vector3(pos.x + j,pos.y - i,0), Quaternion.identity);
                Instantiate(m_bankHighlight, new Vector3(pos.x + j, pos.y - i, 0), Quaternion.identity);
            }

        }
    }

    private void SetAtkIs(List<AttackPattern> attackPatterns)
    {
        m_isFall = false;
        m_isRoll = false;
        m_isBank = false;
        //�U���̐ݒ�
        for (int i = 0; i < attackPatterns.Count; i++)
        {
            switch (attackPatterns[i])
            {
                case AttackPattern.FallRock:
                    m_isFall = true;
                    break;
                case AttackPattern.RollRock:
                    m_isRoll = true;
                    break;
                case AttackPattern.Bank:
                    m_isBank = true;
                    break;
                default:
                    break;
            }
        }

    }
}
