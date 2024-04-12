using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeonGenerator2 : TestDungeonGeneratorBase
{
	class Room
	{
		public int topLeftX;   // 左上座標X
		public int topLeftY;   // 左上座標Y
		public int width;      // 幅
		public int height;     // 高さ
		public List<int> aisleNum = new();   //MapGeneのaisleの何番目を使うか

	}
	struct Aisle
	{
		public int startPoint; //分割線の始点　上か左
		public int endPoint;   //分割線の終点　下か右
		public int line;       //分割線の場所
		public int direction;  //方向
	};

	enum DIRECTION
	{   //方向
		VERTICAL,       //縦
		HORIZON         //横
	};

	[SerializeField] private int MINIMUM_SIZE = 5;       //ルームの最低サイズ
	[SerializeField] private int AISLE_WIDTH = 3;        //通路幅

	[SerializeField] private Vector2Int m_roomCount = new();

	// 部屋
	List<Room> room = new();
	// 通路
	List<Aisle> aisle = new();

	// マップ
	List<List<string>> mapList = new();

	// 部屋分けダンジョン生成
	public override List<List<string>> GenerateDungeon(Vector2Int size)
	{
		// マップの初期化
		for (int y = 0; y < size.y; y++)
		{
			mapList.Add(new List<string>());

			for (int x = 0; x < size.x; x++)
			{
				// 通常ブロックで埋める
				mapList[y].Add("1");
			}
		}

		// 部屋の生成数(5 ~ 10)
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
			//分割できるサイズの部屋がいくつあるか　13以上
			int roomNum = 0;        //規定サイズ以上の部屋の数
			List<int> passRoom = new();   //規定サイズ以上の部屋番号を入れる
			for (int e = 0; e < room.Count; e++)
			{
				if (room[e].width >= MINIMUM_SIZE * 2 + AISLE_WIDTH
					|| room[e].height >= MINIMUM_SIZE * 2 + AISLE_WIDTH)
				{
					roomNum++;
					passRoom.Add(e);
				}
			}
			//分割する部屋を決める
			roomNum = Random.Range(0, roomNum);
			//縦か横かを決める
			int direction;  //direction=0 縦　direction=1 横
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
			//分割するラインを作り、通路を作る
			RoomDivide(passRoom[roomNum], MakeDivideLine(passRoom[roomNum], direction), direction);

			if (generateRoomCount != 0)
			{   //規定数まで生成したかを判定
				//MakeMap();	// 再帰
				continue;
			}
		}

		RoomDig();
		AisleDig();

		return mapList;
	}

	void RoomDivide(int roomNum, int line, int direction)
	{   //部屋を分割する
		Room room1 = new(), room2 = new();
		Aisle tempAisle;
		if (direction == (int)DIRECTION.VERTICAL)
		{   //縦分割
			room1.topLeftY = room2.topLeftY = room[roomNum].topLeftY;
			room1.topLeftX = room[roomNum].topLeftX;
			room2.topLeftX = room1.topLeftX + line + 1;
			room1.height = room2.height = room[roomNum].height;
			room1.width = line - 2;
			room2.width = room[roomNum].width - line - 1;
			//分割線の情報を入れる場所
			tempAisle.startPoint = room[roomNum].topLeftY - 1;
			tempAisle.endPoint = room[roomNum].topLeftY + room[roomNum].height;
			tempAisle.line = room[roomNum].topLeftX + line - 1;
			tempAisle.direction = (int)DIRECTION.VERTICAL;
			//分割前の部屋の持つ分割線をそれぞれに渡す
			for (int e = 0; e < room[roomNum].aisleNum.Count; e++)
			{
				if (aisle[room[roomNum].aisleNum[e]].direction == (int)DIRECTION.VERTICAL)
				{   //縦の線だった場合
					if (aisle[room[roomNum].aisleNum[e]].line < room[roomNum].topLeftX)
					{   //分割線が元の部屋より左にある場合
						room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);     // 部屋に隣接してる通路の挿入
					}
					else
					{   //右の場合
						room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
					}
				}
				else
				{   //横の線の場合
					room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);
					room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
				}
			}
		}
		else
		{   //横分割
			room1.topLeftX = room2.topLeftX = room[roomNum].topLeftX;
			room1.topLeftY = room[roomNum].topLeftY;
			room2.topLeftY = room1.topLeftY + line + 1;
			room1.width = room2.width = room[roomNum].width;
			room1.height = line - 2;
			room2.height = room[roomNum].height - line - 1;
			//分割線の情報を入れる場所
			tempAisle.startPoint = room[roomNum].topLeftX - 1;
			tempAisle.endPoint = room[roomNum].topLeftX + room[roomNum].width;
			tempAisle.line = room[roomNum].topLeftY + line - 1;
			tempAisle.direction = (int)DIRECTION.HORIZON;
			//分割前の部屋の持つ分割線をそれぞれに渡す
			for (int e = 0; e < room[roomNum].aisleNum.Count; e++)
			{
				if (aisle[room[roomNum].aisleNum[e]].direction == (int)DIRECTION.HORIZON)
				{   //横の線だった場合
					if (aisle[room[roomNum].aisleNum[e]].line < room[roomNum].topLeftY)
					{   //分割線が元の部屋より上にある場合
						room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);
					}
					else
					{   //下の場合
						room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
					}
				}
				else
				{   //縦の線の場合
					room1.aisleNum.Insert(room1.aisleNum.Count, room[roomNum].aisleNum[e]);
					room2.aisleNum.Insert(room2.aisleNum.Count, room[roomNum].aisleNum[e]);
				}
			}
		}
		//分割線を通路として新たにaisleに追加する
		aisle.Add(tempAisle);
		room1.aisleNum.Add(aisle.Count - 1);
		room2.aisleNum.Add(aisle.Count - 1);
		//分割した部屋を削除し、新しい部屋をroomに入れる
		room.Remove(room[roomNum]);
		room.Add(room1);
		room.Add(room2);
	}

	int MakeDivideLine(int roomNum, int direction)
	{   //部屋の分割する場所を探す	direction=0 縦　direction=1 横
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
		divideLine = Random.Range(0, divideLine + MINIMUM_SIZE + 2);    //randの+1 通路の+1

		return divideLine;
	}

	void RoomDig()
	{       //マップに部屋を書き込む
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
	{   //マップに通路を書き込む
		int tempRand;
		for (int e = 0; e < room.Count; e++)
		{   //部屋の数だけ行う
			for (int f = 0; f < room[e].aisleNum.Count; f++)
			{   //部屋の持つ通路の数
				if (aisle[room[e].aisleNum[f]].direction == (int)DIRECTION.VERTICAL)
				{   //縦線の時(部屋から横向きに掘る)
					tempRand = Random.Range(0, room[e].height + room[e].topLeftY); //通路を作るy座標を決める
					if (aisle[room[e].aisleNum[f]].line < room[e].topLeftX)
					{   //左
						for (int g = 0; g < 2; g++)
						{
							mapList[room[e].topLeftX - g - 1][tempRand] = " ";
						}
					}
					else
					{   //右
						for (int g = 0; g < 2; g++)
						{
							mapList[room[e].topLeftX + room[e].width + g][tempRand] = " ";
						}
					}
				}
				else
				{   //横線の時(部屋から縦向きに掘る)
					tempRand = Random.Range(0, room[e].width + room[e].topLeftX);  //通路を作るx座標を決める
					if (aisle[room[e].aisleNum[f]].line < room[e].topLeftY)
					{   //上
						for (int g = 0; g < 2; g++)
						{
							mapList[tempRand][room[e].topLeftY - g - 1] = " ";
						}
					}
					else
					{   //下
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
			//通路同士をつなげる
			List<int> temp = new();
			//通路が作られかけている場所を探す
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
			//つなぐ
			if (temp.Count % 2 == 0)
			{   //通路の数が偶数なら
				while (temp.Count > 0)
				{
					for (int f = temp[0] + 1; f < temp[1]; f++)
					{
						if (aisle[e].direction == (int)DIRECTION.VERTICAL) //縦の時
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
			{   //奇数の時
				if (temp.Count != 1)
				{   //通路が一つの時はつながっているためとばす
					while (temp.Count > 3)
					{
						for (int f = temp[0] + 1; f < temp[1]; f++)
						{
							if (aisle[e].direction == (int)DIRECTION.VERTICAL) //縦の時
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
					//残り3つをくっつける
					for (int f = temp[0] + 1; f < temp[2]; f++)
					{
						if (aisle[e].direction == (int)DIRECTION.VERTICAL) //縦の時
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
