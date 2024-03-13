using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestDungeonGenerator : MonoBehaviour
{
	[Header("��������_���W�����̃p�X")]
	[SerializeField] private string m_dungeonPath = "Assets/DungeonData/Dungeon.csv";

	[Header("�_���W�����̃T�C�Y")]
	[SerializeField] private int m_dungeonSizeX;
	[SerializeField] private int m_dungeonSizeY;

	[Header("�����u���b�N")]
	[SerializeField] private GameObject m_block = null;

	//[Header("���̕ϓN���Ȃ��u���b�N")]
	//[SerializeField] private string m_blockNormal = "1";
	[Header("�j��s�\�u���b�N")]
	[SerializeField] private string m_blockDontBroken = "2";
	[Header("�_���W�����̊j")]
	[SerializeField] private string m_blockCore = "3";

	[SerializeField] GameObject m_player = null;

	// Start is called before the first frame update
	void Start()
	{
		if (m_player != null)
		{
			Instantiate<GameObject>(m_player);
		}

		// �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
		if (!File.Exists(m_dungeonPath))
			return;

		// �}�b�v�̃��X�g
		List<List<string>> mapList = new List<List<string>>();

		// �t�@�C���ǂݍ���
		StreamReader streamReader = new StreamReader(m_dungeonPath);

		// ���s��؂�œǂݏo��
		foreach (string line in streamReader.ReadToEnd().Split("\n"))
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

		// �t�@�C�������
		streamReader.Close();

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
				GameObject block = Instantiate<GameObject>(m_block, pos, Quaternion.identity);
				// �u���b�N�X�N���v�g������
				block.AddComponent<Block>();

				// �j��s�\�u���b�N�ɂ���
				if (mapList[y][x] == m_blockDontBroken)
				{
					// ������₷���悤�ɂƂ肠�����F��ς���
					block.GetComponent<SpriteRenderer>().color = Color.gray;
					// �j��s�\�ɂ���
					block.GetComponent<Block>().DontBroken = true;
				}

				// �j�ɂ���
				if (mapList[y][x] == m_blockCore)
				{
					// ������₷���悤�ɂƂ肠�����F��ς���
					block.GetComponent<SpriteRenderer>().color = Color.red;
					// �_���W�����X�N���v�g������
					block.AddComponent<Dungeon>();
				}


			}
		}


	}

	// Update is called once per frame
	void Update()
	{

	}
}
