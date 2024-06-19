using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteLoadCSV : MonoBehaviour
{
	static public void WriteCSV<T>(List<List<T>> lists, string fileName)
	{
		// �������ݗp�̋�̕����z��
		List<string> str = new();

		foreach(List<T> y in lists)
		{
			// 1�s���̃f�[�^
			List<string> row = new();
			// �f�[�^�ݒ�
			foreach(T x in y)
			{
				row.Add(x.ToString());
			}
			// �J���}��؂�ɂ����������ǉ�
			str.Add(string.Join(",", row));
		}

		// �t�@�C���̃p�X
		string path = Application.dataPath + "/" + fileName;

		// �������݃t�@�C��
		StreamWriter stream = new(path, false);

		// ��������t�@�C���ɒǉ�
		for (int i = 0; i < str.Count; i++)
		{
			stream.WriteLine(str[i]);
		}

		// �t�@�C�������
		stream.Close();
		
	}

	static public List<List<T>> LoadCSV<T>(TextAsset csvData)
	{
		List<List<T>> list = new();

		// �e�L�X�g�A�Z�b�g����e�L�X�g�����o��
		string data = csvData.text;
		// 1�s�����o��
		string[] lines = data.Split('\n');

		// �e�s�̃f�[�^�����X�g�ɓ���Ă���
		foreach (string line in lines)
		{
			string[] texts = line.Split(",");

			// �P�s���̃��X�g
			List<T> li = new();

			// �ꕶ���Â��X�g�ɒǉ�
			foreach (string text in texts)
			{
				li.Add((T)Enum.Parse(typeof(T), text));
			}

			// ���X�g�ɒǉ�
			list.Add(li);
		}

		return list;
	}

}
