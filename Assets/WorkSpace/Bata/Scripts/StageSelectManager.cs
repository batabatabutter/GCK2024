using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
	[Header("�X�e�[�W�ԍ�")]
	[SerializeField] StageNumScriptableObject m_stageNumObj = null;

    [Header("�v���C���[")]
    [SerializeField] private GameObject m_player = null;

    [Header("�X�e�[�W�Z���N�g�u���b�N")]
    //[SerializeField] private GameObject m_stageSelectBlock;
    [SerializeField] private StageEntryBlock m_stageEntryBlock = null;

    [Header("�X�e�[�W����L�����o�X")]
    [SerializeField] private StageEntryCanvas m_stageEntryCanvas = null;

    [Header("�X�e�[�W���")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase = null;

    [Header("�u���b�N�Ԃ̌���")]
    [SerializeField] private float m_offset;
    [Header("�u���b�N�̒��S")]
    [SerializeField] private Vector3 m_blockCenter = Vector3.zero;

    [Header("�n��")]
    [SerializeField] private GameObject m_groundObj = null;

    [Header("�{�[�_�[�I�u�W�F�N�g")]
    [SerializeField] private GameObject m_borderObj = null;

    [Header("�ړ��͈�")]
    [SerializeField] private Vector3 m_minPos;
    [SerializeField] private Vector3 m_maxPos;
    [Header("�I�t�Z�b�g")]
    [SerializeField] private float m_borderOffset;

    // Start is called before the first frame update
    void Start()
    {
        // �n�ʂ̐���
        GenerateGrounds();

        // �g�̐���
        GenerateFrames();

        // �I�����̐���
        GenerateSelectors();

    }

    public void ChangeScene(int stageNum)
    {
		// �X�e�[�W�ԍ��ݒ�
		m_stageNumObj.stageNum = stageNum;
		// �����ݒ�
		SaveDataReadWrite.m_instance.MiningType = m_player.GetComponent<PlayerMining>().CircularSaw.MiningType;
		// �Z�[�u
		SaveDataReadWrite.m_instance.Save();
		// �V�[���ǂݍ���
		SceneManager.LoadScene("PlayScene");

	}




    // �n�ʂ̐���
    private void GenerateGrounds()
    {
		//  �n�ʂ̐e�쐬
		GameObject parent = new("Grounds");
		parent.transform.parent = transform;
		for (int x = (int)(m_minPos.x - m_borderOffset); x < (int)(m_maxPos.x + m_borderOffset); x++)
		{
			for (int y = (int)(m_minPos.y - m_borderOffset); y < (int)(m_maxPos.y + m_borderOffset); y++)
			{
				//  ����
				var gr = Instantiate(m_groundObj, parent.transform);
				gr.transform.position = new Vector3(x, y, 0);
			}
		}
	}

    // �g�̐���
    private void GenerateFrames()
    {
		//  �͈͘g�̐e�쐬
		GameObject parent = new("Borders");
		parent.transform.parent = transform;
		for (int x = (int)m_minPos.x; x < (int)m_maxPos.x; x++)
		{
			//  ����
			GameObject br = Instantiate(m_borderObj, parent.transform);
			br.transform.position = new Vector3(x, m_minPos.y, 0);
			br = Instantiate(m_borderObj, parent.transform);
			br.transform.position = new Vector3(x, m_maxPos.y, 0);
		}
		for (int y = (int)m_minPos.y; y < (int)m_maxPos.y; y++)
		{
			//  ����
			GameObject br = Instantiate(m_borderObj, parent.transform);
			br.transform.position = new Vector3(m_minPos.x, y, 0);
			br = Instantiate(m_borderObj, parent.transform);
			br.transform.position = new Vector3(m_maxPos.x, y, 0);
		}

	}

	// �I�����̐���
	private void GenerateSelectors()
	{
		//  �I�����ڂ̐e�쐬
		GameObject parent = new("StageSelectBlock");
		parent.transform.parent = transform;
		// ����̊p�x
		float degree = 360.0f / m_dungeonDataBase.dungeonDatas.Count;
		//  �_���W���������u���b�N����
		for (int i = 0; i < m_dungeonDataBase.dungeonDatas.Count; i++)
		{
			// �I��������
			var block = Instantiate(m_stageEntryBlock, parent.transform);

			// ����
			StageEntryBlock entryBlock = block.GetComponent<StageEntryBlock>();
			// �X�e�[�W�ԍ��ݒ�
			entryBlock.StageNum = i;
			// �L�����o�X�ݒ�
			entryBlock.StageEntryCanvas = m_stageEntryCanvas;

			// �R�A�̃X�v���C�g�ݒ�
			block.GetComponent<SpriteRenderer>().sprite = m_dungeonDataBase.dungeonDatas[i].CoreSprite;

			// �p�x(�E���ɂ��邽�߂Ƀ}�C�i�X������)
			float angle = -degree * i * Mathf.Deg2Rad;
			// �z�u�ꏊ
			Vector3 current = Vector3.up * m_offset;
			Vector3 pos = Vector3.zero;
			// �X�e�[�W���̊p�x��]
			pos.x = (current.x * Mathf.Cos(angle)) - (current.y * Mathf.Sin(angle));
			pos.y = (current.x * Mathf.Sin(angle)) + (current.y * Mathf.Cos(angle));
			// ���S���W���ړ�
			pos += m_blockCenter;
			// ���W�ݒ�
			block.transform.position = MyFunction.RoundHalfUp(pos);
		}

	}
}
