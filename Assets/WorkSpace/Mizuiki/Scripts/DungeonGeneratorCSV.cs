using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorCSV : DungeonGeneratorBase
{
	// �}�b�v
	private readonly List<List<string>> m_mapList = new();

	public override List<List<string>> GenerateDungeon(DungeonData dungeonData)
	{
		// �}�b�v�̏�����
		for (int y = 0; y < dungeonData.Size.y; y++)
		{
			List<string> list = new();
			for (int x = 0; x < dungeonData.Size.x; x++)
			{
				list.Add("1");
			}
			m_mapList.Add(list);
		}

		// �f�[�^���L���X�g
		DungeonDataCSV data = dungeonData as DungeonDataCSV;

		// �L���X�g�ł��Ȃ�
		if (data == null)
		{
			return m_mapList;
		}

		Vector2Int size = data.Size;

		//�P�O���P�O�̃��X�g�Ǘ��p
		List<List<List<string>>> mapListManager = new();

		//�f�[�^�x�[�X���烊�X�g�̎擾
		List<TextAsset> dungeonCSV = data.DungeonCSV;

		for (int i = 0; i < dungeonCSV.Count; i++)
		{
			// �t�@�C�����Ȃ���΃}�b�v�ǂݍ��݂̏��������Ȃ�
			if (dungeonCSV[i] == null)
			{
				Debug.Log("CSV file is not assigned");
				return new();

			}

			// �t�@�C�������s��؂�Ŕz��Ɋi�[
			string[] lines = dungeonCSV[i].text.Split('\n');
			// �ǂ݂������f�[�^�i�[�p���X�g
			List<List<string>> list = new();
			// �t�@�C���̓��e��1�s������
			foreach (string line in lines)
			{
				// �����񂪂Ȃ�
				if (line == "")
					break;

				// ��������J���}��؂�Ŕz��Ɋi�[
				string[] values = line.Split(',');
				// �e�s�̃f�[�^���i�[���郊�X�g
				List<string> rowData = new();
				// �e��̒l����������
				foreach (string value in values)
				{
					// �f�[�^�����X�g�ɒǉ�
					rowData.Add(value);
				}

				// �s�̃f�[�^��CSV�f�[�^�ɒǉ�
				list.Add(rowData);
			}
			//�R�����ɓ����
			mapListManager.Add(list);
		}

		//�u���b�N����
		for (int y = 0; y < size.y / 10; y++)
		{
			for (int x = 0; x < size.x / 10; x++)
			{
				int random = Random.Range(0, dungeonCSV.Count);
				Make10_10Block(mapListManager[random], x, y);

			}
		}

		return m_mapList;
	}

	void Make10_10Block(List<List<string>> mapList, int originX, int originY)
	{
		// �ǂ݂������f�[�^�����ƂɃ_���W��������������
		for (int y = 0; y < mapList.Count; y++)
		{
			for (int x = 0; x < mapList[y].Count; x++)
			{
				//// 0 �̏ꍇ�͉����������Ȃ�
				//if (mapList[y][x] == "0" || mapList[y][x] == "")
				//	continue;

				// �������W
				Vector2Int pos = new((originX * 10) + x, (originY * 10) + y);
				// �}�b�v�ǉ�
				m_mapList[pos.y][pos.x] = mapList[y][x];
			}
		}
	}

}
