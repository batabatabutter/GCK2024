using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteReadCSV : MonoBehaviour
{
	// ****************************** �������� ****************************** //
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

		// CSV�f�[�^�̏�������
		MyFunction.Writer(path, str);
	}


	// ****************************** �ǂݍ��� ****************************** //
	static public List<List<string>> ReadCSV(TextAsset csvData)
	{
		// �e�L�X�g�A�Z�b�g����e�L�X�g�����o��
		string data = csvData.text;

		return ReadCSV(data);
	}
	static public List<List<string>> ReadCSV(string csvData)
	{
		List<List<string>> list = new();

		// 1�s�����o��
		string[] lines = csvData.Split('\n');

		// �e�s�̃f�[�^�����X�g�ɓ���Ă���
		foreach (string line in lines)
		{
			// �����񂪂Ȃ�
			if (line == "")
				break;

			// ���s���폜������������J���}��؂�Ŕz��Ɋi�[
			string[] texts = line.Remove(line.Length - 1).Split(",");

			// �P�s���̃��X�g
			List<string> li = new();

			// �ꕶ���Â��X�g�ɒǉ�
			foreach (string text in texts)
			{
				li.Add(text);
			}

			// ���X�g�ɒǉ�
			list.Add(li);
		}

		return list;
	}

	static public List<List<TEnum>> ReadCSV<TEnum>(TextAsset csvData)
	{
		// �e�L�X�g�A�Z�b�g����e�L�X�g�����o��
		string data = csvData.text;

		return ReadCSV<TEnum>(data);
	}
	static public List<List<TEnum>> ReadCSV<TEnum>(string csvData)
	{
		List<List<TEnum>> list = new();

		// 1�s�����o��
		string[] lines = csvData.Split('\n');

		// �e�s�̃f�[�^�����X�g�ɓ���Ă���
		foreach (string line in lines)
		{
			// �����񂪂Ȃ�
			if (line == "")
				break;

			// ���s���폜������������J���}��؂�Ŕz��Ɋi�[
			string[] texts = line.Remove(line.Length - 1).Split(",");

			// �P�s���̃��X�g
			List<TEnum> li = new();

			// �ꕶ���Â��X�g�ɒǉ�
			foreach (string text in texts)
			{
				li.Add((TEnum)Enum.Parse(typeof(TEnum), text));
			}

			// ���X�g�ɒǉ�
			list.Add(li);
		}

		return list;
	}

}
