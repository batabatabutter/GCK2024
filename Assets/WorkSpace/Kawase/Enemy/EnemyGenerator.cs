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

    [Header("�G�l�~�[�}�l�[�W���[")]
    [SerializeField] private EnemyManager m_enemyManager = null;

    [Header("�v���C���[���S�œG�������͈�"),Min(0)]
    [SerializeField] float m_spawnRadius;

    [Header("�h���I�u�W�F�N�g�̃��C���[")]
    [SerializeField] LayerMask m_detectionLayer;  

    [Header("�v���C���[")]
    [SerializeField] private Transform m_player;

	//�o������G�̃��X�g
	readonly List<Enemy.Type> m_spawnList = new();

    //����
    private float m_spawnTime;
    //�e
    private GameObject m_parent;

    //�E�F�[�u
    int m_wave = 0;

    //���
    //WaveState m_waveState;

    //�o����
    int m_spawnEnemyNum;

    //�X�|�[���^�C�}�[
    float m_timer;

    //�E�F�[�u�}�l�[�W���[
    //WaveManager m_waveManager;

    [Header("�_���W�����A�^�b�J�[")]
    [SerializeField] private DungeonAttacker m_dungeonAttacker = null;


    // Start is called before the first frame update
    void Start()
    {
        int stageNum = 0;
		if (m_playSceneManager)
        {
			// �X�e�[�W�ԍ�
			stageNum = m_playSceneManager.StageNum;
			// �v���C���[�擾
			if (m_player == null)
			{
				m_player = m_playSceneManager.Player.transform;
			}

		}

        // �_���W�����A�^�b�J�[�擾
        if (m_dungeonAttacker == null)
        {
            m_dungeonAttacker = GetComponent<DungeonAttacker>();
        }

        //�E�F�[�u���Ƃ̏��
        DungeonData.DungeonWave dungeonData = m_dungeonDataBase.dungeonDatas[stageNum].DungeonWaves[m_wave];
        //�o�����̂����Ă�
        m_spawnEnemyNum = dungeonData.generateEnemyNum;

        //�X�|�[���Ԋu
        m_spawnTime = dungeonData.geterateEnemyInterval;

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

        //�e
        m_parent = new GameObject("Enemies");

    }

    // Update is called once per frame
    void Update()
    {
        // �A�^�b�J�[���Ȃ��ꍇ�̓X�|�[�������Ȃ�
        if (m_dungeonAttacker == null)
        {
            return;
        }

        if (m_timer < 0)
        {
            //�X�|�[��
            Spawn(m_spawnList[Random.Range(0, m_spawnList.Count)]);

            m_spawnEnemyNum--;

            m_timer = m_spawnTime;
        }
        else
        {
            // �U����Ԃ̏ꍇ�̓^�C�}�[���Z
            if (m_dungeonAttacker.Active)
            {
                m_timer -= Time.deltaTime;

            }
        }

        if(m_spawnEnemyNum <= 0)
        {
            // ���̓G�𐶐�
            Start();

        }

    }

    public void Spawn()
    {
        // �^�C�v���w�肵�ăX�|�[��
        Spawn(GetSpawnType());
    }
	public void Spawn(Enemy.Type type)
	{
        Spawn(type, m_spawnRadius);
	}
    public void Spawn(Transform block)
    {
        Spawn(GetSpawnType(), block);
    }
    public void Spawn(float radius)
    {
        Spawn(GetSpawnType(), radius);
    }
	public void Spawn(Enemy.Type type, float radius)
    {
        // �w�背�C���[�̃I�u�W�F�N�g�����m
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_player.position, radius, m_detectionLayer);
        // �R���C�_�[�z������X�g�ɂ���
		List<Collider2D> collidersList = new(colliders);

		// �����ɍ���Ȃ��v�f���폜
		collidersList.RemoveAll(collider => !ShouldKeepCollider(collider));

        if (collidersList.Count <= 0)
		{
			Debug.Log("�h���u���b�N���Ȃ�");
            return;
		}

		// ���m�����I�u�W�F�N�g���烉���_���Ɉ�I��
		Collider2D randomCollider = collidersList[Random.Range(0, collidersList.Count)];
        // �u���b�N�ɓG��߈˂�����
        Spawn(type, randomCollider.transform);

    }
    public void Spawn(Enemy.Type type, Vector3 position)
    {
        // position �̈ʒu�ɂ���u���b�N�擾
        Collider2D collider = Physics2D.OverlapCircle(MyFunction.RoundHalfUp(position), 0, m_detectionLayer);

        // �u���b�N�Ȃ�ĂȂ�������
        if (collider == null)
        {
			Debug.Log("�h���u���b�N���Ȃ�");
			return;
        }
        // �߈˂ł��Ȃ���
        if (!ShouldKeepCollider(collider))
        {
			Debug.Log("�h���u���b�N���Ȃ�");
			return;
		}

		// �u���b�N�ɓG��߈˂�����
		Spawn(type, collider.transform);
    }
    public void Spawn(Enemy.Type type, Transform block)
    {
		// �O�̂��ߗ�O�𐶂܂Ȃ����`�F�b�N
		if (!CheckEnableEnemy())
		{
			Debug.Log("�G�������ł��Ȃ���B�����͈͂��������Ă�");
			return;
		}
		// �G�̎�ނ��͈͊O�������ꍇ�͐������Ȃ���
		if (type == Enemy.Type.OverID)
		{
			Spawn(block);
			return;
		}

        // �G�l�~�[�̏��擾
        EnemyData enemyData = m_enemyDataBase.enemyDatas[(int)type];

		switch (enemyData.system)
		{
			case Enemy.System.Dwell:
				// �u���b�N�߈ˌ^

				// �I�����ꂽ�I�u�W�F�N�g�ɑ΂��鏈�����s��
				if (block != null)
				{
                    // �X�|�[���ʒu
					Vector3 spawnPos = block.position;
                    // �G�𐶐�
					GameObject enemy = Instantiate(enemyData.prefab, spawnPos, Quaternion.identity, m_parent.transform);
                    // �G�l�~�[�}�l�[�W���[�ɒǉ�
                    m_enemyManager.AddEnemy(enemy.GetComponent<Enemy>());
                    // �h���u���b�N�̓o�^
					enemy.GetComponent<EnemyDwell>().DwellBlock = block.gameObject;
					// �u���b�N��߈ˍς݂ɂ��� == �߈˕s�\��Ԃɂ���
					block.GetComponent<Block>().CanPossess = false;
                    // �X�|�[������炷
                    AudioManager.Instance.PlaySE(enemyData.GenerateSE);
				}
				else
				{
					Debug.Log("�h���u���b�N���Ȃ�");
				}
				break;

			case Enemy.System.Mob:
			// �����^(��ŏ����Ǝv��)



			default:
				break;
		}

	}



    private Enemy.Type GetSpawnType()
    {
        return m_spawnList[Random.Range(0, m_spawnList.Count)];
	}

	// ���X�g�̒��ɐ����͈͂̓G�����邩�m�F
	private bool CheckEnableEnemy()
    {
        foreach (Enemy.Type type in m_spawnList)
        {
            if (type != Enemy.Type.OverID)
            {
                // �͈͓�
                return true;
            }
        }
        // �͈͓��̓G�͂��Ȃ�����
        return false;
    }

    // �R���C�_�[��ێ����邩�ǂ����𔻒肷�郁�\�b�h
    private bool ShouldKeepCollider(Collider2D collider)
    {
        // �w�肳�ꂽ�����Ɋ�Â��ăR���C�_�[��ێ����邩�ǂ����𔻒�
        Block block = collider.gameObject.GetComponent<Block>();

        // Block�X�N���v�g���A�^�b�`����Ă��Ȃ��ꍇ�͕ێ����Ȃ�
        if (block == null)
            return false;

		// �߈ˍς݂̏ꍇ�͕ێ����Ȃ�
		if (block.CanPossess == false)
			return false;

		// BlockData��null�łȂ����Ƃ��m�F
		if (block.BlockData != null)
        {
            return block.BlockData.Type < BlockData.BlockType.SPECIAL;
        }
        // �u���b�N�f�[�^���Ȃ�������ێ����Ȃ�
        return false;
    }
}
