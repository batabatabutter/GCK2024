using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveManager;

public class EnemyGenerator : MonoBehaviour
{
    [Header("�_���W�����f�[�^�x�[�X")]
    [SerializeField] DungeonDataBase m_dungeonDataBase;

    [Header("�G�l�~�[�f�[�^�x�[�X")]
    [SerializeField] EnemyDataBase m_enemyDataBase;

    [Header("�v���C�V�[���}�l�[�W���[")]
    [SerializeField] PlaySceneManager m_playSceneManager;

    [Header("�v���C���[���S�œG�������͈�"),Min(0)]
    [SerializeField] float m_spawnradius;

    [Header("���m���郌�C���[")]
    [SerializeField] LayerMask detectionLayer;  

    //�o������G�̃��X�g
    List<Enemy.Type> m_spawnList = new List<Enemy.Type>();

    //�v���C���[
    private GameObject m_player;

    //����
    private float m_spawnTime;
    //�e
    private GameObject m_parent;

    //�E�F�[�u
    int m_wave;

    //���
    WaveManager.WaveState m_waveState;

    //�o����
    int m_spawnEnemyNum;

    //�X�|�[���^�C�}�[
    float m_timer;

    //�E�F�[�u�}�l�[�W���[
    WaveManager m_waveManager;

    // Start is called before the first frame update
    void Start()
    {
        if(m_waveManager == null)
        {
            m_waveManager = GetComponent<WaveManager>();
        }

        //�X�e�[�W�ԍ�
        int stageNum = m_playSceneManager.StageNum;
        //���݂̃E�F�[�u���̎擾
        m_wave = m_waveManager.WaveNum;
        //�E�F�[�u���Ƃ̏��
        DungeonData.DungeonWave dungeonData = m_dungeonDataBase.dungeonDatas[stageNum].DungeonWaves[m_wave];
        //�o�����̂����Ă�
        m_spawnEnemyNum = dungeonData.generateEnemyNum;

        //�X�|�[���Ԋu
        m_spawnTime = dungeonData.dungeonATKCoolTime;

        //�^�C�}�[�̐ݒ�
        m_timer = m_spawnTime;

        //������
        m_spawnList.Clear();

        //�X�e�[�W�̏o���G�z��
        List<Enemy.Type> enemyTypeList = dungeonData.generateEnemyType;


        for (int i = 0; i < enemyTypeList.Count; i++)
        {
            //�f�[�^�x�[�X�ɂȂ����̂͂��������O
            if((int)enemyTypeList[i] >= m_enemyDataBase.enemyDatas.Count)
            {
                continue;
            }
            //�ԍ��ɍ��킹���v���n�u���擾
            Enemy.Type type = enemyTypeList[i];
            // ���X�g���ɓ����v�f���Ȃ����m�F
            if (!m_spawnList.Contains(type))
            {
                // �����v�f���Ȃ��ꍇ�ɗv�f��ǉ�
                m_spawnList.Add(type);
            }
        }

        if(m_player == null)
        {
            //�v���C���[�悱��
            m_player = m_playSceneManager.GetPlayer();
        }
        //if(m_player == null)
        {
            //�e
            m_parent = new GameObject("Enemies");
        }

    }

    // Update is called once per frame
    void Update()
    {



        if (m_timer < 0)
        {
            //�X�|�[��
            Spawn(m_spawnList[Random.Range(0, m_spawnList.Count)]);

            m_spawnEnemyNum--;

            m_timer = m_spawnTime;
        }
        else
        {
            if(m_waveManager.waveState == WaveState.Attack)
            {
                m_timer -= Time.deltaTime;

            }
        }

        if(m_spawnEnemyNum <= 0)
        {
            m_waveManager.waveState = WaveState.Break;
            //�E�F�[�u�������Ȃ��ꍇ������
            if ( m_waveManager.WaveNum < m_waveManager.WAVE_MAX_NUM - 1)
            {

                m_waveManager.WaveNum++;

            }
            Start();

        }

    }

    public void Spawn(Enemy.Type type)
    {
        switch (m_enemyDataBase.enemyDatas[(int)type].system)
        {
            case Enemy.System.Dwell:

                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_player.transform.position, m_spawnradius, detectionLayer);

                List<Collider2D> collidersList = new List<Collider2D>(colliders);

                // �����ɍ���Ȃ��v�f���폜
                collidersList.RemoveAll(collider => !ShouldKeepCollider(collider));

                if (collidersList.Count > 0)
                {
                    // ���m�����I�u�W�F�N�g���烉���_���Ɉ�I��
                    Collider2D randomCollider = collidersList[Random.Range(0, collidersList.Count)];

                    // �I�����ꂽ�I�u�W�F�N�g�ɑ΂��鏈�����s��

                    Vector3 spawnPos = randomCollider.transform.position;

                    GameObject enemy = Instantiate(m_enemyDataBase.enemyDatas[(int)type].prefab, spawnPos, Quaternion.identity, m_parent.transform);

                    enemy.GetComponent<EnemyDwell>().DwellBlock = randomCollider.gameObject;

                }
                else
                {
                    Debug.Log("�h���u���b�N���Ȃ�");
                }

                    break;
            default:
                break;
        }

    }

    // �R���C�_�[��ێ����邩�ǂ����𔻒肷�郁�\�b�h
    bool ShouldKeepCollider(Collider2D collider)
    {
        // �w�肳�ꂽ�����Ɋ�Â��ăR���C�_�[��ێ����邩�ǂ����𔻒�
        Block block = collider.gameObject.GetComponent<Block>();
        if (block != null && block.BlockData != null) // BlockData��null�łȂ����Ƃ��m�F
        {
            return block.BlockData.Type < BlockData.BlockType.SPECIAL;
        }
        // Block�X�N���v�g���A�^�b�`����Ă��Ȃ��ꍇ�͕ێ����Ȃ�
        return false;
    }
}
