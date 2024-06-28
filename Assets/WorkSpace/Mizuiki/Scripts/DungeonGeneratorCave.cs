using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorCave : DungeonGeneratorBase
{
	[Header("�󓴂̐�������"), Range(0.0f, 1.0f)]
	[SerializeField] private float m_cavity = 0.5f;

	[Header("�m�C�Y�̃X�P�[��"), Min(0.0f)]
	[SerializeField] private float m_noiseScale = 1.0f;


	// �}�b�v�̃T�C�Y
	private Vector2Int m_mapSize;

	// �}�b�v���X�g
	readonly List<List<string>> m_mapList = new();


	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
	{
		// �f�[�^�̃L���X�g
		DungeonDataCave data = dungeonData as DungeonDataCave;

		// �f�[�^�̐ݒ�
		if (data)
		{
			SetDungeonData(data);
		}

		return GenerateDungeon(dungeonData.Size);

	}
	public override List<List<string>> GenerateDungeon(Vector2Int size)
	{
		// �}�b�v�̃T�C�Y�擾
		m_mapSize = size;

		// �}�b�v������
		for (int y = 0; y < size.y; y++)
		{
			List<string> list = new();
			for (int x = 0; x < size.x; x++)
			{
				// ��̒ǉ�
				list.Add("1");
			}
			// �s�̒ǉ�
			m_mapList.Add(list);
		}

		// �󓴂̐���
		CreateCave();

		// ���������}�b�v��Ԃ�
		return m_mapList;
	}

	public void SetDungeonData(DungeonDataCave data)
    {
		// �󓴗�
		m_cavity = data.Cavity;
	}

	
	private void CreateCave()
	{
		// �����擾
		float random = Random.value;

		for (int y = 0; y < m_mapSize.y; y++)
		{
			for (int x = 0; x < m_mapSize.x; x++)
			{
				// �m�C�Y�擾
				float noise = MyFunction.GetNoise(new Vector2(x, y) * m_noiseScale, random);

				// �m�C�Y�̒l�����������ȉ�
				if (noise <= m_cavity)
				{
					m_mapList[y][x] = "0";
				}
			}
		}
	}

}
