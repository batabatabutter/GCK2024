using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchBlock : MonoBehaviour
{
    [Header("�T�[�`�Ώ�")]
    [SerializeField, CustomEnum(typeof(BlockData.BlockType))] private string m_searchBlockType;
    private BlockData.BlockType m_blockType;

	// �T�[�`�Ώۃu���b�N�̃Q�[���I�u�W�F�N�g�I�u�W�F�N�g
	private readonly Dictionary<BlockData.BlockType, List<Transform>> m_searchBlocks = new();
    // �^�[�Q�b�g�̃u���b�N�̃Q�[���I�u�W�F�N�g
    private List<Transform> m_targetBlocks = new();

    [Header("�T�[�`�͈�(���a)")]
    [SerializeField] private float m_searchRange;

    [Header("�T�[�`��")]
    [SerializeField] private float m_hitCount = 1;
    [Header("�S�I��")]
    [SerializeField] private bool m_searchAll = false;

    [Header("�^�[�Q�b�g�̈ʒu�������}�[�J�[")]
    [SerializeField] private SearchMarker m_markerObject = null;

    [Header("�}�[�J�[�̗L������(�b)")]
    [SerializeField] private float m_markerLifeTime = 1.0f;
    [Header("�}�[�J�[�̕\���͈�(���a)")]
    [SerializeField] private float m_markerMaxScale = 50.0f;

    [Header("�J�n���ɃT�[�`")]
    [SerializeField] private bool m_awake = false;


	private void Awake()
	{
        m_blockType = SerializeUtil.Restore<BlockData.BlockType>(m_searchBlockType);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SearchOne(m_blockType);
        }
    }

    // �ł��߂��u���b�N
    public void SearchOne(BlockData.BlockType type)
    {
        // �^�[�Q�b�g����ɂ���
        m_targetBlocks.Clear();

        // �T�[�`�Ώۂ��Ȃ���ΕԂ�
        if (m_searchBlocks.Count == 0)
        {
			Debug.Log("�T�[�`�Ώۂ��Ȃ���");
			return;
        }

        // �߂��u���b�N
        List<Transform> nearBlock = m_searchBlocks[type];
        // �߂����Ƀ\�[�g
        nearBlock.Sort((lhs, rhs) => Vector2.Distance(transform.position, lhs.position).CompareTo(Vector2.Distance(transform.position, rhs.position)));

        // �ł��߂��u���b�N���T�[�`�ΏۂƂ��Ēǉ�
        m_targetBlocks.Add(nearBlock[0]);

        // �}�[�J�[�̍쐬
		CreateSearchMarker();

	}

	// �͈͓��̂��ׂẴu���b�N
	public void SearchAll(BlockData.BlockType type)
    {
        // �^�[�Q�b�g����ɂ���
        m_targetBlocks.Clear();

		// �T�[�`�Ώۂ��Ȃ���ΕԂ�
		if (m_searchBlocks.Count == 0)
		{
			Debug.Log("�T�[�`�Ώۂ��Ȃ���");
			return;
		}

        foreach (Transform block in m_searchBlocks[type])
        {
            // �v���C���[����̋���
            float distance = Vector2.Distance(transform.position, block.position);

            // ���ׂĂ��T�[�`�͈�
            if (m_searchAll)
            {
                m_targetBlocks.Add(block);
                continue;
            }

            // �T�[�`�͈͂��߂�
            if (distance <= m_searchRange)
            {
                m_targetBlocks.Add(block);
            }
        }

		// �}�[�J�[�̍쐬
		CreateSearchMarker();

	}

	// �C���X�y�N�^�[�Őݒ肳�ꂽ���̃u���b�N
	public void SearchCount(BlockData.BlockType type, int count)
    {
		// �^�[�Q�b�g����ɂ���
		m_targetBlocks.Clear();

		// �T�[�`�Ώۂ��Ȃ���ΕԂ�
		if (m_searchBlocks.Count == 0)
		{
            Debug.Log("�T�[�`�Ώۂ��Ȃ���");
			return;
		}

        // �R�s�[�n��
        m_targetBlocks = new(m_searchBlocks[type]);

        // �T�[�`�͈͊O�͍폜
        m_targetBlocks.RemoveAll(b => Vector2.Distance(transform.position, b.transform.position) > m_searchRange);

        // �����̋߂����Ƀ\�[�g
        m_targetBlocks.Sort((lhs, rhs) => Vector2.Distance(transform.position, lhs.transform.position).CompareTo((int)Vector2.Distance(transform.position, rhs.transform.position)));

		// �T�[�`�ΏۊO���폜
        while (m_targetBlocks.Count > m_hitCount)
        {
            m_targetBlocks.RemoveAt(m_targetBlocks.Count - 1);
        }

		// �}�[�J�[�̍쐬
		CreateSearchMarker();

	}

	// �u���b�N�̐ݒ�
	public void SetSearchBlocks(List<Block> blocks)
    {
        foreach (Block block in blocks)
        {
            //  null�Ȃ�X�L�b�v
            if (!block) continue;

            if (block.BlockData == null)
                continue;

            // �u���b�N�̎�ގ擾
            BlockData.BlockType type = block.BlockData.Type;

            // �L�[�����݂��Ȃ�
            if(!m_searchBlocks.ContainsKey(type))
            {
                m_searchBlocks[type] = new();
            }

            // �u���b�N�̒ǉ�
            m_searchBlocks[type].Add(block.transform);
        }

		// �J�n�Ɠ����ɃT�[�`����
		if (m_awake)
		{
			SearchOne(BlockData.BlockType.CORE);
		}

	}



	private void CreateSearchMarker()
    {
        // �}�[�J�[���Ȃ�
        if (m_markerObject == null)
        {
            Debug.Log("�T�[�` : �}�[�J�[���Ȃ���");
            return;
        }
        // ���ׂẴ^�[�Q�b�g�̈ʒu�Ƀ}�[�J�[�𐶐�����
        foreach (Transform target in m_targetBlocks)
        {
            // �}�[�J�[�𐶐�
            SearchMarker marker = Instantiate(m_markerObject, target.position, Quaternion.identity);
            // �������Ԑݒ�
            marker.LifeTime = m_markerLifeTime;
            // �ő�T�C�Y�ݒ�
            marker.MaxScale = m_markerMaxScale;
            // �v���C���[
            marker.Player = transform;
		}
    }



    // �}�[�J�[�̐�������
    public float MarkerLifeTime
    {
        set { m_markerLifeTime = value; }
    }

    // �}�[�J�[�̕\���͈�
    public float MarkerMaxScale
    {
        set { m_markerMaxScale = value; }
    }


}
