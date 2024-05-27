using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DungeonGeneratorMaze : DungeonGeneratorBase
{
	[Header("�����̎��")]
	[SerializeField] private DungeonDataMaze.MazeType m_mazeType;

	[Header("�ǂ̕�")]
	[SerializeField, Min(1)] private int m_wallWidth = 1;
	[SerializeField, Min(1)] private int m_wallHeight = 1;

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
		m_mapSize.x = size.x / ((m_pathWidth + m_wallWidth) / 2);
		m_mapSize.y = size.y / ((m_pathHeight + m_wallHeight) / 2);

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
		// �ǂ̕�
		m_wallWidth = dungeonData.WallWidth;
		m_wallHeight = dungeonData.WallHeight;
		// �ʘH�̕�
		m_pathWidth = dungeonData.PathWidth;
		m_pathHeight = dungeonData.PathHeight;
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
		// �ǂ�L�΂����n�_
		List<Vector2Int> startPos = new();

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
					// �ǂ�L�΂����n�_�ǉ�
					if (x % 2 == 0 && y % 2 == 0)
					{
						startPos.Add(new(x, y));
					}
				}
			}
			m_mazeList.Add(maze);
		}

		// �O�̂��߂̖������[�v���p
		int count = 0;
		// �ǂ�L�΂����n�_���Ȃ��Ȃ�܂Ń��[�v
		while (startPos.Count > 0)
		{
			// �����_���ȃC���f�b�N�X���擾
			int index = Random.Range(0, startPos.Count);
			// �J�n�n�_���擾
			Vector2Int start = startPos[index];
			// ���n�_����폜
			startPos.RemoveAt(index);

			// ���łɕǂɂȂ��Ă���
			if (m_mazeList[start.y][start.x] == "1")
				continue;

			// �g�����̕ǂ̏��
			Stack<Vector2Int> extendWall = new();

			// �ǂ�L�΂��Ă���
			ExtendWall(start, extendWall);

			count++;
			if (count > 1000000)
				break;
		}
;

	}
	// �ǂ�L�΂�
	private void ExtendWall(Vector2Int start, Stack<Vector2Int> extendWall)
	{
		List<MyFunction.Direction> direction = new();
		// ��
		if (m_mazeList[start.y + 1][start.x] == "0" &&          // ��}�X�オ�ʘH
			!extendWall.Contains(start + new Vector2Int(0, 2)))  // ��}�X�オ���ݐL�΂��Ă���ǂł͂Ȃ�
		{
			direction.Add(MyFunction.Direction.UP);
		}
		// �E
		if (m_mazeList[start.y][start.x + 1] == "0" &&          // ��}�X�E���ʘH
			!extendWall.Contains(start + new Vector2Int(2, 0))) // ��}�X�E�����ݐL�΂��Ă���ǂł͂Ȃ�
		{
			direction.Add(MyFunction.Direction.RIGHT);
		}
		// ��
		if (m_mazeList[start.y - 1][start.x] == "0" &&          // ��}�X�����ʘH
			!extendWall.Contains(start + new Vector2Int(0, -2)))// ��}�X�������ݐL�΂��Ă���ǂł͂Ȃ�
		{
			direction.Add(MyFunction.Direction.DOWN);
		}
		// ��
		if (m_mazeList[start.y][start.x - 1] == "0" &&          // ��}�X�����ʘH
			!extendWall.Contains(start + new Vector2Int(-2, 0)))// ��}�X�������ݐL�΂��Ă���ǂł͂Ȃ�
		{
			direction.Add(MyFunction.Direction.LEFT);
		}

		// �l�����̂����ǂꂩ�ɂ͐L�΂���
		if (direction.Count > 0)
		{
			// ���ݒn�_��ǂɂ���
			m_mazeList[start.y][start.x] = "1";
			// �g�����̕ǂɒǉ�
			extendWall.Push(start);

			// �L�΂����悪�ʘH
			bool path = false;
			// �����_���ɐL�΂����������߂�
			int dir = Random.Range(0, direction.Count);
			// �L�΂�
			switch (direction[dir])
			{
				case MyFunction.Direction.UP:		// ��
					// ��}�X�オ�ʘH
					path = m_mazeList[start.y + 2][start.x] == "0";
					// ��}�X���ǂɂ���
					m_mazeList[++start.y][start.x] = "1";
					// ��}�X���ǂɂ���
					m_mazeList[++start.y][start.x] = "1";
					extendWall.Push(start);
					break;

				case MyFunction.Direction.RIGHT:	// �E
					// ��}�X�E���ʘH
					path = m_mazeList[start.y][start.x + 2] == "0";
					// ��}�X�E��ǂɂ���
					m_mazeList[start.y][++start.x] = "1";
					// ��}�X�E��ǂɂ���
					m_mazeList[start.y][++start.x] = "1";
					extendWall.Push(start);
					break;

				case MyFunction.Direction.DOWN:     // ��
					// ��}�X�����ʘH
					path = m_mazeList[start.y - 2][start.x] == "0";
					// ��}�X����ǂɂ���
					m_mazeList[--start.y][start.x] = "1";
					// ��}�X����ǂɂ���
					m_mazeList[--start.y][start.x] = "1";
					extendWall.Push(start);
					break;

				case MyFunction.Direction.LEFT:		// ��
					// ��}�X�����ʘH
					path = m_mazeList[start.y][start.x - 2] == "0";
					// ��}�X����ǂɂ���
					m_mazeList[start.y][--start.x] = "1";
					// ��}�X����ǂɂ���
					m_mazeList[start.y][--start.x] = "1";
					extendWall.Push(start);
					break;
			}
			// �L�΂����悪�ʘH
			if (path)
			{
				// ����ɕǂ�L�΂�
				ExtendWall(start, extendWall);
			}
		}
		// �ǂ��ɂ��L�΂��Ȃ�
		else
		{
			// �߂��ĐL�΂��������ĊJ����
			Vector2Int back = extendWall.Pop();
			ExtendWall(back, extendWall);
		}
	}

	// ���@��@
	private void CreateDig()
	{

	}

	// ���H�����ƂɃ}�b�v�����
	private void CreateMazeMap()
	{
		// �����u���b�N��
		Vector2Int count = Vector2Int.zero;
		// �[�����̕ǂ̕�
		Vector2Int edgeWall = new()
		{
			x = m_mapList[0].Count / m_mazeList[0].Count,
			y = m_mapList.Count / m_mazeList.Count,
		};

		for (int y = 0; y < m_mazeList.Count; y++)
		{
			int height;
			// y �������̏ꍇ�͕ǂ̕�
			if (y % 2 == 0)
			{
				height = m_wallHeight;
			}
			// �����̏ꍇ�͒ʘH�̕�
			else
			{
				height = m_pathHeight;
			}

			// �[����
			if (y == 0)
			{
				height = edgeWall.y;
			}

			// x �̃J�E���g������
			count.x = 0;

			for (int x = 0; x < m_mazeList[y].Count; x++)
			{
				int width;
				// x �������̏ꍇ�͕ǂ̕��Ő���
				if (x % 2 == 0)
				{
					width = m_wallWidth;
				}
				// ��̏ꍇ�͒ʘH�̕��Ő���
				else
				{
					width = m_pathHeight;
				}

				// �[����
				if (x == 0)
				{
					width = edgeWall.x;
				}

				// �u���b�N�̏�񐶐�
				CreateBlocks(count, width, height, m_mazeList[y][x]);
				// �J�E���g���Z
				count.x += width;
			}
			// �J�E���g���Z
			count.y += height;
		}


		//int height = 0;
		//// �}�b�v�̍s
		//for (int y = 0; y < m_mapList.Count; y++)
		//{
		//	// �ŏ��̍s�͂͂����āA�ʘH���̔{���̂Ƃ��ɍ��������Z
		//	if (y % m_pathHeight == 0 && y != 0)
		//	{
		//		height++;
		//	}
		//	// ���H�̃T�C�Y�O�ɂȂ����烋�[�v���I������
		//	if (height >= m_mapSize.y)
		//	{
		//		break;
		//	}
		//	// ���̐錾
		//	int width = 0;
		//	// �}�b�v�̗�
		//	for (int x = 0; x < m_mapList[y].Count; x++)
		//	{
		//		// �ŏ��̗���͂����āA�ʘH�̕��̔{���̂Ƃ��ɕ������Z
		//		if (x % m_pathWidth == 0 && x != 0)
		//		{
		//			width++;
		//		}
		//		// ���H�̃T�C�Y�O�ɂȂ����烋�[�v�𔲂���
		//		if (width >= m_mapSize.x)
		//		{
		//			break;
		//		}
		//		// �}�b�v�ɖ��H�̏�����
		//		m_mapList[y][x] = m_mazeList[height][width];
		//	}
		//}
	}

	// ���H�̈�O���b�h���̃u���b�N�𐶐�����
	private void CreateBlocks(Vector2Int start, int width, int height, string maze)
	{
		for (int y = 0; y < height; y++)
		{
			// �z��͈͊O
			if (start.y + y >= m_mapList.Count)
				break;

			for (int x = 0; x < width; x++)
			{
				// �z��͈͊O
				if (start.x + x >= m_mapList[start.y + y].Count)
					break;

				// ���H�̏�������
				m_mapList[start.y + y][start.x + x] = maze;
			}
		}
	}

}
