using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    const int MAX_RECHANGE = 100;

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


    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W�ԍ�
        int stageNum = m_playSceneManager.StageNum;
        //�X�e�[�W�̏o���G�z��
        List<Enemy.Type> enemyTypeList = m_dungeonDataBase.dungeonDatas[stageNum].enemy;

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

        //�v���C���[�悱��
        m_player = m_playSceneManager.GetPlayer();
        //�X�|�[���Ԋu�悱��
        m_spawnTime = m_dungeonDataBase.dungeonDatas[stageNum].enemySpawnTime;

        //�e
        m_parent = new GameObject("Enemies");

    }

    // Update is called once per frame
    void Update()
    {
        //�X�e�[�W�ԍ�
        int stageNum = m_playSceneManager.StageNum;

        if (m_spawnTime < 0)
        {
            //�X�|�[��
            Spawn(m_spawnList[Random.Range(0, m_spawnList.Count)]);


            m_spawnTime = m_dungeonDataBase.dungeonDatas[stageNum].enemySpawnTime;
        }
        else
        {
            m_spawnTime -= Time.deltaTime;
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
