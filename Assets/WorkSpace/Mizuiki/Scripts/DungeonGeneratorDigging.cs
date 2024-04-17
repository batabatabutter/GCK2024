using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �Q�l : https://qiita.com/gis/items/253cb6af577ec11e79cf
/// </summary>
public class DungeonGeneratorDigging : DungeonGeneratorBase
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

    // ����(�ʘH�ɂ��g�p)
    struct Room
    {
        public Vector2Int pos;    // �����̍��W
        public Vector2Int size;   // �����̃T�C�Y

        public Tile tile;
    }
    [Header("�����̃T�C�Y�̍ŏ��l�ƍő�l")]
    [SerializeField] private MyFunction.MinMax m_roomRange;

    [Header("�ʘH�̒����͈̔�")]
    [SerializeField] private MyFunction.MinMax m_pathRange;

    [Header("�����̍ő吔"), Min(0)]
    [SerializeField] private int m_maxCount = 100;

    [Header("�����̐�����")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_roomGenerateRate = 0.5f;

    [Header("�ʘH�����܂��Ă��銄��")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_pathFillRate = 0.5f;

    [Header("�����ɍz�΂̉򂪂ł���m��")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_roomOreChunkRate = 0.0f;
    [Header("�����̍z�ΐ�����")]
    [SerializeField] private MyFunction.MinMax m_roomOreChunkCount;

	// �^�C���}�b�v
	readonly List<List<Tile>> m_tiles = new();
    readonly List<List<string>> m_mapList = new();

    // �����̔z��
    private readonly List<Room> m_rooms = new();
    // �ʘH�̐����\�ʒu
    private readonly List<Room> m_root = new();

    // �}�b�v�̃T�C�Y
    private Vector2Int m_mapSize;

    // �_���W�����f�[�^�̐ݒ�
    public void SetDungeonData(DungeonDataDigging data)
    {
		// �����̃T�C�Y
		m_roomRange = data.RoomRange;
		// �ʘH�̒���
		m_pathRange = data.PathRange;
		// �����̍ő吔
		m_maxCount = data.MaxCount;
		// �����̐�����
		m_roomGenerateRate = data.RoomGenerateRate;
		// �ʘH�̖��ߗ���
		m_pathFillRate = data.PathFillRate;

	}

	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
    {
		// �}�b�v�̃T�C�Y�擾
		m_mapSize = dungeonData.Size;

		// �^�C���}�b�v������
		for (int y = 0; y < m_mapSize.y; y++)
        {
            List<Tile> tiles = new();
            for (int x = 0; x < m_mapSize.x; x++)
            {
                // ��̒ǉ�
                tiles.Add(Tile.BLOCK);
            }
            // �s�̒ǉ�
            m_tiles.Add(tiles);
        }

        // �f�[�^�̃L���X�g
		DungeonDataDigging data = dungeonData as DungeonDataDigging;

        // �f�[�^�̐ݒ�
        if (data)
        {
            SetDungeonData(data);
        }

        // �ŏ��̕����̍��W
        Vector2Int firstRoomPos = new(Random.Range(0, m_mapSize.x - m_roomRange.max), Random.Range(0, m_mapSize.y - m_roomRange.max));

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
            m_mapList.Add(list);
        }

        // �z�ΐ���
        CreateOre();

        return m_mapList;
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

    // �z�΂̐���
    private void CreateOre()
    {
        // �܂��͐������镔�������߂�
		foreach (Room room in m_rooms)
		{
            // �����擾
            float rand = Random.Range(0.0f, 1.0f);

            // �z�΂𐶐����Ȃ�
            if (rand > m_roomOreChunkRate)
                continue;

            // �������̌���
            int count = Random.Range(m_roomOreChunkCount.min, m_roomOreChunkCount.max + 1);
            // �z�΂̉�𐶐�
            for (int i = 0; i < count; i++)
            {
                // �����ʒu����
                Vector2Int pos = new(Random.Range(room.pos.x, room.pos.x + room.size.x), Random.Range(room.pos.y, room.pos.y + room.size.y));

                CreateOre(pos);
            }
		}
	}
    private void CreateOre(Vector2Int pos)
    {


        m_mapList[pos.y][pos.x] = "2";
        
    }

}
