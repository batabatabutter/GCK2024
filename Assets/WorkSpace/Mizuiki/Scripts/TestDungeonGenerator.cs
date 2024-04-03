using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class TestDungeonGenerator : MonoBehaviour
{
	[Header("��������_���W�����̃f�[�^")]
	[SerializeField] private Object m_dungeonData = null;

	[Header("�_���W�����̃T�C�Y")]
	[SerializeField] private int m_dungeonSizeX;
	[SerializeField] private int m_dungeonSizeY;

	[Header("�����u���b�N")]
	[SerializeField] private GameObject m_block = null;
	[SerializeField] private GameObject m_blockBedrock = null;
	[SerializeField] private GameObject m_blockCore = null;

	[Header("���̕ϓN���Ȃ��u���b�N")]
	[SerializeField] private string m_blockNameNormal = "1";
	[Header("�j��s�\�u���b�N")]
	[SerializeField] private string m_blockNameBedrock = "2";
	[Header("�_���W�����̊j")]
	[SerializeField] private string m_blockNameCore = "3";

	[SerializeField] GameObject m_player = null;

	// Start is called before the first frame update
	void Start()
	{
		// �v���C���[���ݒ肳��Ă���ΐ���
		if (m_player != null)
		{
			Instantiate(m_player);
		}

		// SCV�ǂݍ���
		GenerateSCV();

		// �_���W�����炵���_���W��������
		//GenerateRoom();

	}

	// Update is called once per frame
	void Update()
	{

	}

	// �_���W�����̐���
	private void Generate(List<List<string>> mapList)
	{
		// �ǂ݂������f�[�^�����ƂɃ_���W��������������
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				// 0 �̏ꍇ�͉����������Ȃ�
				if (mapList[y][x] == "0" || mapList[y][x] == "")
					continue;

				// �������W
				Vector3 pos = new(x, y, 0.0f);

				// �u���b�N�̐���
				GameObject block = null;

				// ���ʂ̃u���b�N
				if (mapList[y][x] == m_blockNameNormal)
				{
					block = Instantiate(m_block, pos, Quaternion.identity);
				}
				// �j��s�\�u���b�N�ɂ���
				else if (mapList[y][x] == m_blockNameBedrock)
				{
					// ��Ճu���b�N����
					block = Instantiate(m_blockBedrock, pos, Quaternion.identity);
				}
				// �j�ɂ���
				else if (mapList[y][x] == m_blockNameCore)
				{
					// �j�u���b�N����
					block = Instantiate(m_blockCore, pos, Quaternion.identity);
				}

				// �����Ή�
				block.AddComponent<ChangeBrightness>();

			}
		}

	}

	// CSV�ǂݍ��݂̐���
	private void GenerateSCV()
	{
		// �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
		if (!m_dungeonData)
			return;

		// �}�b�v�̃��X�g
		List<List<string>> mapList = new ();

		// ���s��؂�œǂݏo��
		foreach (string line in m_dungeonData.ToString().Split("\n"))
		{
			// �s�����݂��Ȃ���΃��[�v�𔲂���
			if (line == "")
				break;

			string lin = line.Remove(line.Length - 1);

			List<string> list = new ();

			// �J���}��؂�œǂݏo��
			foreach (string line2 in lin.Split(","))
			{
				list.Add(line2);
			}

			mapList.Add(list);
		}

		// �}�b�v�̐���
		Generate(mapList);

	}


	struct Room
	{
		int topLeftX;   // ������WX
		int topLeftY;   // ������WY
		int width;		// ��
		int height;		// ����

	}

	// ���������_���W��������
	private void GenerateRoom()
	{
		// �}�b�v
		List<List<string>> mapList = new ();

		// �}�b�v�̏�����
		for (int y = 0; y < m_dungeonSizeY; y++)
		{
			mapList.Add(new List<string>());

			for (int x = 0; x < m_dungeonSizeX; x++)
			{
				// �ʏ�u���b�N�Ŗ��߂�
				mapList[y].Add(m_blockNameNormal);
			}
		}

		// �����̐�����(5 ~ 10)
		int generateRoomCount = Random.Range(5, 10);



	}

}
