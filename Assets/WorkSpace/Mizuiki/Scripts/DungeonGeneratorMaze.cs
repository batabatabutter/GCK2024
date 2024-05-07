using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DungeonGeneratorMaze : DungeonGeneratorBase
{
	[Header("�����̎��")]
	[SerializeField] private DungeonDataMaze.MazeType m_mazeType;

	[Header("�ʘH�̕�")]
	[SerializeField, Min(1)] private int m_pathWidth = 1;
	[SerializeField, Min(1)] private int m_pathHeight = 1;

	// �}�b�v�̃T�C�Y
	private Vector2Int m_mapSize;

	// ���H�s��
	readonly List<List<string>> m_mazeList = new();
	// �}�b�v���X�g
	readonly List<List<string>> m_mapList = new();



	// �f�[�^�����Ƃɐ���
	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
	{
		// �f�[�^��ϊ�
		DungeonDataMaze data = dungeonData as DungeonDataMaze;
		// �f�[�^������ΐݒ肷��
		if (data)
		{
			SetDungeonData(data);
		}
		// �T�C�Y�����ƂɃ_���W��������
		return GenerateDungeon(dungeonData.Size);
	}

	// �T�C�Y�����Ƃɐ���
	public override List<List<string>> GenerateDungeon(Vector2Int size)
	{
		// �^�C���}�b�v������
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

		// �}�b�v�̃T�C�Y�擾
		m_mapSize.x = size.x / m_pathWidth;
		m_mapSize.y = size.y / m_pathHeight;

		// �����T�C�Y�̏ꍇ�͊�ɂ���
		if (m_mapSize.x % 2 == 0)
		{
			m_mapSize.x--;
		}
		if (m_mapSize.y % 2 == 0)
		{
			m_mapSize.y--;
		}

		// �T�C�Y��5�����̏ꍇ�͕Ԃ�
		if (m_mapSize.x < 5 || m_mapSize.y < 5)
			return m_mapList;

		// �����@������
		switch (m_mazeType)
		{
			case DungeonDataMaze.MazeType.Bar:
				// �_�|���@
				CreateBar();
				break;

			case DungeonDataMaze.MazeType.Wall:
				// �ǐL�΂��@
				CreateWall();
				break;

			case DungeonDataMaze.MazeType.Dig:
				// ���@��@
				CreateDig();
				break;
		}

		// ���H�̏������ƂɃ}�b�v�����
		CreateMazeMap();

		// ���������}�b�v����Ԃ�
		return m_mapList;
	}

	// �_���W�����f�[�^�̐ݒ�
	private void SetDungeonData(DungeonDataMaze dungeonData)
	{
		// �����^�C�v
		m_mazeType = dungeonData.Type;
		// �ʘH�̕�
		m_pathWidth = dungeonData.PathWidth;
	}

	// �_�|���@
	private void CreateBar()
	{
		// �O����ǂɂ���
		for (int y = 0; y < m_mapSize.y; y++)
		{
			List<string> maze = new();
			for (int x = 0; x < m_mapSize.x; x++)
			{
				// �O��
				if (x == 0 || y == 0 ||
					x == m_mapSize.x - 1 || y == m_mapSize.y - 1)
				{
					maze.Add("1");
				}
				// ����
				else
				{
					maze.Add("0");
				}
			}
			m_mazeList.Add(maze);
		}

		// �_�X�̕ǂ�����Ă�������L�΂�
		for (int y = 2; y < m_mapSize.y - 1; y += 2)
		{
			for (int x = 2; x < m_mapSize.x - 1; x += 2)
			{
				// �_(��)�𗧂Ă�
				m_mazeList[y][x] = "1";

				// �O�̂��߂̖������[�v���p
				int count = 0;

				// �|����܂ŌJ��Ԃ�
				while (true)
				{
					// 1�s�ڂ̏ꍇ�͏�ɂ��|����
					int direction;
					if (y == 2)
					{
						direction = Random.Range(0, 4);
					}
					else
					{
						direction = Random.Range(0, 3);
					}

					// �_��|������
					Vector2Int bar = new (x, y);
					switch (direction)
					{
						case 0:
							// ��
							bar.y++;
							break;

						case 1:
							// �E
							bar.x++;
							break;

						case 2:
							// ��
							bar.y--;
							break;

						case 3:
							// ��
							bar.x--;
							break;
					}

					// �|���������ǂ���Ȃ�
					if (m_mazeList[bar.y][bar.x] != "1")
					{
						// �w������ɓ|���Ĕ�����
						m_mazeList[bar.y][bar.x] = "1";
						break;
					}

					// 100���񃋁[�v�����狭���I��
					count++;
					if (count > 1000000)
						break;
				}
			}
		}
	}

	// �ǐL�΂��@
	private void CreateWall()
	{

	}

	// ���@��@
	private void CreateDig()
	{

	}

	// ���H�����ƂɃ}�b�v�����
	private void CreateMazeMap()
	{
		int height = 0;

		for (int y = 0; y < m_mapList.Count; y++)
		{
			if (y % m_pathHeight == 0 && y != 0)
			{
				height++;
			}
			if (height >= m_mapSize.y)
			{
				break;
			}


			int width = 0;

			for (int x = 0; x < m_mapList[y].Count; x++)
			{
				if (x % m_pathWidth == 0 && x != 0)
				{
					width++;
				}
				if (width >= m_mapSize.x)
				{
					break;
				}

				m_mapList[y][x] = m_mazeList[height][width];
			}
		}
	}

}
