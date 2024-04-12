using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �Q�l : https://qiita.com/gis/items/253cb6af577ec11e79cf
/// </summary>
public class TestDungeonGenerator1 : TestDungeonGeneratorBase
{
    // ����
    enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,

        OVER,
    }
    // �^�C��
    enum Tile
    {
        BLOCK,
        ROOM,
        PATH,
        WALL,

        OVER,
    }

    // �͈�
    [System.Serializable]
    struct RangeInt
    {
        [Min(0)]
        public int min;
        [Min(1)]
        public int max;
    }
    // ����(�ʘH�ɂ��g�p)
    struct Room
    {
        public Vector2Int pos;    // �����̍��W
        public Vector2Int size;   // �����̃T�C�Y

        public Tile tile;
    }
    [Header("�����̃T�C�Y�̍ŏ��l�ƍő�l")]
    [SerializeField] private RangeInt m_roomRange;

    [Header("���̒����͈̔�")]
    [SerializeField] private RangeInt m_pathRange;

    [Header("�����̍ő吔"), Min(0)]
    [SerializeField] private int m_maxCount = 100;

    [Header("�����̐�����")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_roomGenerateRate = 0.5f;

    [Header("�ʘH�����܂��Ă��銄��")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_pathFillRate = 0.5f;

    // �^�C���}�b�v
    List<List<Tile>> m_tiles = new();

    // �����̔z��
    private List<Room> m_rooms = new();
    // �ʘH�̐����\�ʒu
    private List<Room> m_root = new();

    // �}�b�v�̃T�C�Y
    private Vector2Int m_mapSize;

    // �_���W�����f�[�^�̐ݒ�
    public void SetDungeonData(DungeonData dungeonData)
    {
    }

	public override List<List<string>> GenerateDungeon(Vector2Int size)
    {
        // �^�C���}�b�v������
        for (int y = 0; y < size.y; y++)
        {
            // �s�̒ǉ�
            m_tiles.Add(new());
            for (int x = 0; x < size.x; x++)
            {
                // ��̒ǉ�
                m_tiles[y].Add(Tile.BLOCK);
            }
        }

        // �}�b�v�̃T�C�Y�擾
        m_mapSize = size;

        // �ŏ��̕����̍��W
        Vector2Int firstRoomPos = new(Random.Range(0, size.x - m_roomRange.max), Random.Range(0, size.y - m_roomRange.max));

        // �ŏ��̕��������
        CreateRoom(firstRoomPos, Direction.OVER);

        // �q���镔�������
        for (int i = 0; i < m_maxCount; i++)
        {
            // ����ȏ���Ȃ�
            if (!CreateSpace())
            {
                Debug.Log("����ȏ���Ȃ��� -> " + i.ToString() + " �񐶐�");
                break;
            }
        }

        // ���������^�C���}�b�v�����ƂɃu���b�N�̔z�u���s��
        List<List<string>> mapList = new();
        foreach (List<Tile> tiles in m_tiles)
        {
            List<string> list = new();
            foreach (Tile tile in tiles)
            {
                if (tile == Tile.BLOCK)
                {
                    list.Add("1");
                }
                else if (tile == Tile.WALL)
                {
                    list.Add("1");
                }
                else if (tile == Tile.PATH)
                {
                    // �ʘH�����܂��Ă���m��
                    float rand = Random.Range(0.0f, 1.0f);
                    if (rand <= m_pathFillRate)
                    {
                        // �ʘH�̖��ߗ���
                        list.Add("1");
					}
                    else
                    {
                        list.Add("0");
                    }
				}
                else
                {
                    list.Add("0");
                }
            }
            mapList.Add(list);
        }

        return mapList;
    }


    // �����̍쐬
    private bool CreateRoom(Vector2Int position, Direction direction)
    {
        // �����̍쐬
        Room room = new()
        {
            pos = position,
			size = new Vector2Int(Random.Range(m_roomRange.min, m_roomRange.max), Random.Range(m_roomRange.min, m_roomRange.max)),
            tile = Tile.ROOM,
		};

        // �����̔�яo������
        switch (direction)
        {
            case Direction.UP:
                room.pos.x -= room.size.x / 2;
                room.pos.y += 1;
                break;

            case Direction.DOWN:
                room.pos.x -= room.size.x / 2;
                room.pos.y -= room.size.y;
                break;

            case Direction.LEFT:
                room.pos.x -= room.size.x;
                room.pos.y -= room.size.y / 2;
                break;

            case Direction.RIGHT:
                room.pos.x += 1;
                room.pos.y -= room.size.y / 2;
                break;
        }

        // �ق��̕�����ʘH�ɂ��Ԃ��Ă��Ȃ���
        if (CheckCreate(room))
        {
			// �����̒ǉ�
			m_rooms.Add(room);

            // �����̎���ɒʘH�ƕ����𐶐��\�ɂ���
            SetWall(room, direction, Tile.ROOM);
            // �����̍쐬����
            return true;
        }
        // ����������Ȃ�����
        return false;
	}

    // ���̍쐬
    private bool CreatePath(Vector2Int pos, Direction direction)
    {
        Room path = new()
        {
            pos = pos,
            tile = Tile.PATH,
        };

        // ���E�ɐL�΂�
        if (Random.Range(0, 2) == 0)
        {
            // �L�΂�����
            path.size.x = Random.Range(m_pathRange.min, m_pathRange.max);
            path.size.y = 1;
            // �L�΂�����
            switch (direction)
            {
                case Direction.UP:
                    // �m���ō��ɂ��炷
                    if (Random.Range(0, 2) == 0)
                    {
                        path.pos.x -= path.size.x - 1;
                    }
                    // ��ɐL�΂�
                    path.pos.y += 1;
                    break;

                case Direction.DOWN:
					// �m���ō��ɂ��炷
					if (Random.Range(0, 2) == 0)
					{
						path.pos.x -= path.size.x - 1;
					}
                    // ���ɐL�΂�
                    path.pos.y -= 1;
					break;

                case Direction.LEFT:
                    // ���ɐL�΂�
                    path.pos.x -= path.size.x;
                    break;

                case Direction.RIGHT:
                    // �E�ɐL�΂�
                    path.pos.x += 1;
                    break;
            }

        }
        // �㉺�ɐL�΂�
        else
        {
            // �L�΂�����
            path.size.x = 1;
            path.size.y = Random.Range(m_pathRange.min, m_pathRange.max);
            // �L�΂�����
            switch (direction)
            {
                case Direction.UP:
                    // ��ɐL�΂�
                    path.pos.y += 1;
                    break;

                case Direction.DOWN:
                    // ���ɐL�΂�
                    path.pos.y -= path.size.y;
                    break;

                case Direction.LEFT:
                    // ���ɐL�΂�
                    path.pos.x -= 1;
                    // �m���ŏ�ɂ��炷
                    if (Random.Range(0, 2) == 0)
                    {
                        path.pos.y -= path.size.y - 1;
                    }
                    break;

                case Direction.RIGHT:
                    // �E�ɐL�΂�
                    path.pos.x += 1;
                    // �m���ŏ�ɂ��炷
                    if (Random.Range(0, 2) == 0)
                    {
                        path.pos.y -= path.size.y - 1;
                    }
                    break;
            }
        }
		// �ق��̕�����ʘH�ɂ��Ԃ��Ă��Ȃ���
		if (CheckCreate(path))
		{
			// �ʘH�̎���ɒʘH�ƕ����𐶐��\�ɂ���
			SetWall(path, direction, Tile.PATH);
			// �ʘH�̍쐬����
			return true;
		}
		// �ʘH������Ȃ�����
		return false;

	}

	// ���ӂ𐶐��\�ȕǂɂ���
	private void SetWall(Room room, Direction direction, Tile tile)
    {
		// �����̎���ɒʘH�ƕ����𐶐��\�ɂ���
		if (direction != Direction.UP)
		{
			// �����̏�ɒʘH���쐬�\
			Room root = new()
			{
				pos = new(room.pos.x, room.pos.y + room.size.y),
				size = new(room.size.x, 1),
				tile = tile,
			};
			// �t�����ɒǉ�
			m_root.Add(root);
		}
		if (direction != Direction.DOWN)
		{
			// �����̏�ɒʘH���쐬�\
			Room root = new()
			{
				pos = new(room.pos.x, room.pos.y - 1),
				size = new(room.size.x, 1),
				tile = tile,
			};
			// �t�����ɒǉ�
			m_root.Add(root);
		}
		if (direction != Direction.LEFT)
		{
			// �����̍��ɒʘH���쐬�\
			Room root = new()
			{
				pos = new(room.pos.x - 1, room.pos.y),
				size = new(1, room.size.y),
				tile = tile,
			};
			// �t�����ɒǉ�
			m_root.Add(root);
		}
		if (direction != Direction.RIGHT)
		{
			// �����̉E�ɒʘH���쐬�\
			Room root = new()
			{
				pos = new(room.pos.x + room.size.x, room.pos.y),
				size = new(1, room.size.y),
				tile = tile,
			};
			// �t�����ɒǉ�
			m_root.Add(root);
		}

	}


	// �_���W�������L����
	private bool CreateSpace()
    {
        // �������[�v�h�~�p�̃J�E���^
        int count = 0;
        while (true)
        {
            // �t�������Ȃ�
            if (m_root.Count <= 0)
                return false;

            // �L�΂��t��������
            int index = Random.Range(0, m_root.Count);
            // �L�΂��n�߂�ʒu����
            Vector2Int pos = new()
            {
                x = Random.Range(m_root[index].pos.x, m_root[index].pos.x + m_root[index].size.x - 1),
                y = Random.Range(m_root[index].pos.y, m_root[index].pos.y + m_root[index].size.y - 1),
            };
            // �L�΂�����
            for (Direction dir = Direction.UP; dir < Direction.OVER; dir++)
            {
                // ���̕����ɂ͐L�΂��Ȃ�
                if (!CreateSpace(pos, dir))
                {
                    continue;
                }
                // �t��������폜
                m_root.RemoveAt(index);
                // �_���W�������L������
                return true;
            }

            count++;
            if (count >= 1000000)
                return false;
        }
    }
    private bool CreateSpace(Vector2Int position, Direction direction)
    {
        // ��}�X�߂������W
        Vector2Int backPos = position;
        switch (direction)
        {
            case Direction.UP:      // ���ɐL�΂�
                backPos.y -= 1;
                break;

            case Direction.DOWN:    // ��ɐL�΂�
                backPos.y += 1;
                break;

            case Direction.LEFT:    // �E�ɐL�΂�
                backPos.x += 1;
                break;

            case Direction.RIGHT:   // ���ɐL�΂�
                backPos.x -= 1;
                break;
        }
        // �߂����悪�͈͊O
        if (backPos.x < 0 ||              // ��
            backPos.y < 0 ||              // ��
            backPos.x >= m_mapSize.x ||   // �E
            backPos.y >= m_mapSize.y)     // ��
        {
            // �L�΂��Ȃ���
            return false;
        }
        // ���Α����������ʘH�ȊO
        if (!(m_tiles[backPos.y][backPos.x] == Tile.ROOM || m_tiles[backPos.y][backPos.x] == Tile.PATH))
        {
            return false;
        }

        // �����̐����m��
        float rand = Random.Range(0.0f, 1.0f);

        // �m���ŕ������ʘH
        if (rand <= m_roomGenerateRate)
        {
            // ���������Ȃ�����
            if (!CreateRoom(position, direction))
            {
                return false;
            }
            // �����̈ʒu��ʘH�ɂ���
            m_tiles[position.y][position.x] = Tile.PATH;
            // �_���W�������L������
            return true;
        }
        else
        {
            // �������Ȃ�����
            if (!CreatePath(position, direction))
            {
                return false;
            }
            // �����̈ʒu��ʘH�ɂ���
            m_tiles[position.y][position.x] = Tile.PATH;
            // �_���W�������L������
            return true;
        }
    }

    // �������쐬�\���`�F�b�N
    private bool CheckCreate(Room room)
    {
        // �}�b�v�͈͊O
        if (room.pos.x <= 0 ||  // ��
			room.pos.y <= 0 ||  // ��
            room.pos.x + room.size.x >= m_mapSize.x ||  // �E
            room.pos.y + room.size.y >= m_mapSize.y)    // ��
        {
            return false;
        }
        // �쐬�\��n�����ׂău���b�N���m�F
        for (int y = room.pos.y; y < room.pos.y + room.size.y; y++)
        {
            for (int x = room.pos.x; x < room.pos.x + room.size.x; x++)
            {
				// �u���b�N����Ȃ��^�C��������
				if (m_tiles[y][x] != Tile.BLOCK)
                {
                    // ���������Ȃ�
                    return false;
                }
            }
        }
        // �����̎����ǂɂ���
        for (int y = room.pos.y - 1; y < room.pos.y + room.size.y + 1; y++)
        {
            for (int x = room.pos.x - 1; x < room.pos.x + room.size.x + 1; x++)
            {
                // �����̊O��
                if (x < room.pos.x ||   // ��
                    y < room.pos.y ||   // ��
                    x >= room.pos.x + room.size.x ||    // �E
                    y >= room.pos.y + room.size.y)      // ��
                {
                    // �ǃ^�C���ɂ���
                    m_tiles[y][x] = Tile.WALL;
                }
                else
                {
                    // �w��^�C���ɂ���(ROOM or PATH)
                    m_tiles[y][x] = room.tile;
                }
            }
        }
        // ����������
        return true;
    }

}