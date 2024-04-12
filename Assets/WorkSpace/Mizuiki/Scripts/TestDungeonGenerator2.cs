using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeonGenerator2 : TestDungeonGeneratorBase
{
	class Room
	{
		public int topLeftX;   // ������WX
		public int topLeftY;   // ������WY
		public int width;      // ��
		public int height;     // ����
		public List<int> aisleNum = new();   //MapGene��aisle�̉��Ԗڂ��g����

	}
	struct Aisle
	{
		public int startPoint; //�������̎n�_�@�ォ��
		public int endPoint;   //�������̏I�_�@�����E
		public int line;       //�������̏ꏊ
		public int direction;  //����
	};

	enum DIRECTION
	{   //����
		VERTICAL,       //�c
		HORIZON         //��
	};

	[SerializeField] private int MINIMUM_SIZE = 5;       //���[���̍Œ�T�C�Y
	[SerializeField] private int AISLE_WIDTH = 3;        //�ʘH��

	[SerializeField] private Vector2Int m_roomCount = new();

	// ����
	List<Room> room = new();
	// �ʘH
	List<Aisle> aisle = new();

	// �}�b�v
	List<List<string>> mapList = new();

	// ���������_���W��������
	public override List<List<string>> GenerateDungeon(Vector2Int size)
	{
		// �}�b�v�̏�����
		for (int y = 0; y < size.y; y++)
		{
			mapList.Add(new List<string>());

			for (int x = 0; x < size.x; x++)
			{
				// �ʏ�u���b�N�Ŗ��߂�
				mapList[y].Add("1");
			}
		}

		// �����̐�����(5 ~ 10)
		int generateRoomCount = Random.Range(m_roomCount.x, m_roomCount.y);

		Room tempRoom = new()
		{
			topLeftX = 1,
			topLeftY = 1,
			width = size.x - 2,
			height = size.y - 2,
		};
		room.Add(tempRoom);

		while (generateRoomCount > 0)
		{
			generateRoomCount -= 1;
			//�����ł���T�C�Y�̕������������邩�@13�ȏ�
			int roomNum = 0;        //�K��T�C�Y�ȏ�̕����̐�
			List<int> passRoom = new();   //�K��T�C�Y�ȏ�̕����ԍ�������
			for (int e = 0; e < room.Count; e++)
			{
				if (room[e].width >= MINIMUM_SIZE * 2 + AISLE_WIDTH
					|| room[e].height >= MINIMUM_SIZE * 2 + AISLE_WIDTH)
				{
					roomNum++;
					passRoom.Add(e);
				}
			}
			//�������镔�������߂�
			roomNum = Random.Range(0, roomNum);
			//�c�����������߂�
			int direction;  //direction=0 �c�@direction=1 ��
			if (room[passRoom[roomNum]].width >= (MINIMUM_SIZE * 2 + AISLE_WIDTH)
				&& room[passRoom[roomNum]].height >= (MINIMUM_SIZE * 2 + AISLE_WIDTH))
			{
				direction = Random.Range(0, 1);
			}
			else if (room[passRoom[roomNum]].width >= (MINIMUM_SIZE * 2 + AISLE_WIDTH))
			{
				direction = (int)DIRECTION.VERTICAL;
			}
			else
			{
				direction = (int)DIRECTION.HORIZON;
			}
			//�������郉�C�������A�ʘH�����
			RoomDivide(passRoom[roomNum], MakeDivideLine(passRoom[roomNum], direction), direction);

			if (generateRoomCount != 0)
			{   //�K�萔�܂Ő����������𔻒�
				//MakeMap();	// �ċA
				continue;
			}
		}

		RoomDig();
		AisleDig();

		return mapList;
	}

	void RoomDivide(int roomNum, int line, int direction)
	{   //�����𕪊�����
		Room room1 = new(), room2 = new();
		Aisle tempAisle;
		if (direction == (int)DIRECTION.VERTICAL)
		{   //�c����
			room1.topLeftY = room2.topLeftY = room[roomNum].topLeftY;
			room1.topLeftX = room[roomNum].topLeftX;
			room2.topLeftX = room1.topLeftX + line + 1;
			room1.height = room2.height = room[roomNum].height;
			room1.width = line - 2;
			room2.width = room[roomNum].width - line - 1;
			//�������̏�������ꏊ
			tempAisle.startPoint = room[roomNum].topLeftY - 1;
			tempAisle.endPoint = room[roomNum].topLeftY + room[roomNum].height;
			tempAisle.line = room[roomNum].topLeftX + line - 1;
			tempAisle.direction = (int)DIRECTION.VERTICAL;
			//�����O�̕����̎������������ꂼ��ɓn��
			for (int e = 0; e < room[roomNum].aisleNum.Count; e++)
			{
				if (aisle[room[roomNum].aisleNum[e]].direction == (int)DIRECTION.VERTICAL)
				{   //�c�̐��������ꍇ
					if (aisle[room[roomNum].aisleNum[e]].line < room[roomNum].topLeftX)
					{   //�����������̕�����荶�ɂ���ꍇ
						room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);     // �����ɗאڂ��Ă�ʘH�̑}��
					}
					else
					{   //�E�̏ꍇ
						room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
					}
				}
				else
				{   //���̐��̏ꍇ
					room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);
					room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
				}
			}
		}
		else
		{   //������
			room1.topLeftX = room2.topLeftX = room[roomNum].topLeftX;
			room1.topLeftY = room[roomNum].topLeftY;
			room2.topLeftY = room1.topLeftY + line + 1;
			room1.width = room2.width = room[roomNum].width;
			room1.height = line - 2;
			room2.height = room[roomNum].height - line - 1;
			//�������̏�������ꏊ
			tempAisle.startPoint = room[roomNum].topLeftX - 1;
			tempAisle.endPoint = room[roomNum].topLeftX + room[roomNum].width;
			tempAisle.line = room[roomNum].topLeftY + line - 1;
			tempAisle.direction = (int)DIRECTION.HORIZON;
			//�����O�̕����̎������������ꂼ��ɓn��
			for (int e = 0; e < room[roomNum].aisleNum.Count; e++)
			{
				if (aisle[room[roomNum].aisleNum[e]].direction == (int)DIRECTION.HORIZON)
				{   //���̐��������ꍇ
					if (aisle[room[roomNum].aisleNum[e]].line < room[roomNum].topLeftY)
					{   //�����������̕�������ɂ���ꍇ
						room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);
					}
					else
					{   //���̏ꍇ
						room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
					}
				}
				else
				{   //�c�̐��̏ꍇ
					room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);
					room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
				}
			}
		}
		//��������ʘH�Ƃ��ĐV����aisle�ɒǉ�����
		aisle.Add(tempAisle);
		room1.aisleNum.Add(aisle.Count - 1);
		room2.aisleNum.Add(aisle.Count - 1);
		//���������������폜���A�V����������room�ɓ����
		room.Remove(room[roomNum]);
		room.Add(room1);
		room.Add(room2);
	}

	int MakeDivideLine(int roomNum, int direction)
	{   //�����̕�������ꏊ��T��	direction=0 �c�@direction=1 ��
		int divideLine;
		if (direction == (int)DIRECTION.VERTICAL)
		{
			divideLine = room[roomNum].width;
		}
		else
		{
			divideLine = room[roomNum].height;
		}

		divideLine -= (MINIMUM_SIZE * 2 + 2);
		divideLine = Random.Range(0, divideLine + MINIMUM_SIZE + 2);    //rand��+1 �ʘH��+1

		return divideLine;
	}

	void RoomDig()
	{       //�}�b�v�ɕ�������������
		for (int e = 0; e < (int)room.Count; e++)
		{
			for (int f = room[e].topLeftX; f < room[e].topLeftX + room[e].width; f++)
			{
				for (int g = room[e].topLeftY; g < room[e].topLeftY + room[e].height; g++)
				{
					mapList[f][g] = " ";
				}
			}
		}
	}

	void AisleDig()
	{   //�}�b�v�ɒʘH����������
		int tempRand;
		for (int e = 0; e < room.Count; e++)
		{   //�����̐������s��
			for (int f = 0; f < room[e].aisleNum.Count; f++)
			{   //�����̎��ʘH�̐�
				if (aisle[room[e].aisleNum[f]].direction == (int)DIRECTION.VERTICAL)
				{   //�c���̎�(�������牡�����Ɍ@��)
					tempRand = Random.Range(0, room[e].height + room[e].topLeftY); //�ʘH�����y���W�����߂�
					if (aisle[room[e].aisleNum[f]].line < room[e].topLeftX)
					{   //��
						for (int g = 0; g < 2; g++)
						{
							mapList[room[e].topLeftX - g - 1][tempRand] = " ";
						}
					}
					else
					{   //�E
						for (int g = 0; g < 2; g++)
						{
							mapList[room[e].topLeftX + room[e].width + g][tempRand] = " ";
						}
					}
				}
				else
				{   //�����̎�(��������c�����Ɍ@��)
					tempRand = Random.Range(0, room[e].width + room[e].topLeftX);  //�ʘH�����x���W�����߂�
					if (aisle[room[e].aisleNum[f]].line < room[e].topLeftY)
					{   //��
						for (int g = 0; g < 2; g++)
						{
							mapList[tempRand][room[e].topLeftY - g - 1] = " ";
						}
					}
					else
					{   //��
						for (int g = 0; g < 2; g++)
						{
							mapList[tempRand][room[e].topLeftY + room[e].height + g] = " ";
						}
					}
				}
			}
		}
		for (int e = 0; e < aisle.Count; e++)
		{
			//�ʘH���m���Ȃ���
			List<int> temp = new();
			//�ʘH�����ꂩ���Ă���ꏊ��T��
			if (aisle[e].direction == (int)DIRECTION.VERTICAL)
			{
				for (int f = aisle[e].startPoint; f < aisle[e].endPoint; f++)
				{
					if (mapList[aisle[e].line][f] == " ")
					{
						temp.Add(f);
					}
				}
			}
			else
			{
				for (int f = aisle[e].startPoint; f < aisle[e].endPoint; f++)
				{
					if (mapList[f][aisle[e].line] == " ")
					{
						temp.Add(f);
					}
				}
			}
			//�Ȃ�
			if (temp.Count % 2 == 0)
			{   //�ʘH�̐��������Ȃ�
				while (temp.Count > 0)
				{
					for (int f = temp[0] + 1; f < temp[1]; f++)
					{
						if (aisle[e].direction == (int)DIRECTION.VERTICAL) //�c�̎�
						{
							mapList[aisle[e].line][f] = " ";
						}
						else
						{
							mapList[f][aisle[e].line] = " ";
						}
					}
					temp.Remove(temp[0]);
					temp.Remove(temp[0]);
				}
			}
			else
			{   //��̎�
				if (temp.Count != 1)
				{   //�ʘH����̎��͂Ȃ����Ă��邽�߂Ƃ΂�
					while (temp.Count > 3)
					{
						for (int f = temp[0] + 1; f < temp[1]; f++)
						{
							if (aisle[e].direction == (int)DIRECTION.VERTICAL) //�c�̎�
							{
								mapList[aisle[e].line][f] = " ";
							}
							else
							{
								mapList[f][aisle[e].line] = " ";
							}
						}
						temp.Remove(temp[0]);
						temp.Remove(temp[0]);
					}
					//�c��3����������
					for (int f = temp[0] + 1; f < temp[2]; f++)
					{
						if (aisle[e].direction == (int)DIRECTION.VERTICAL) //�c�̎�
						{
							mapList[aisle[e].line][f] = " ";
						}
						else
						{
							mapList[f][aisle[e].line] = " ";
						}
					}
				}
			}
		}
	}
}
