using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		if (m_player != null)
		{
			Instantiate(m_player);
		}

		// �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
		if (!m_dungeonData)
			return;

		// �}�b�v�̃��X�g
		List<List<string>> mapList = new List<List<string>>();

		//// �t�@�C���ǂݍ���
		//StreamReader streamReader = new StreamReader(m_dungeonData.ToString());

		// ���s��؂�œǂݏo��
		foreach (string line in /*streamReader.ReadToEnd()*/m_dungeonData.ToString().Split("\n"))
		{
			// �s�����݂��Ȃ���΃��[�v�𔲂���
			if (line == "")
				break;

			string lin = line.Remove(line.Length - 1);

			List<string> list = new List<string>();

			// �J���}��؂�œǂݏo��
			foreach (string line2 in lin.Split(","))
			{
				list.Add(line2);
			}

			mapList.Add(list);
		}

		//// �t�@�C�������
		//streamReader.Close();

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

	// Update is called once per frame
	void Update()
	{

	}
}
