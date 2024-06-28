using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteReadCSV : MonoBehaviour
{
	// ****************************** 書き込み ****************************** //
	static public void WriteCSV<T>(List<List<T>> lists, string fileName)
	{
		// 書き込み用の空の文字配列
		List<string> str = new();

		foreach(List<T> y in lists)
		{
			// 1行分のデータ
			List<string> row = new();
			// データ設定
			foreach(T x in y)
			{
				row.Add(x.ToString());
			}
			// カンマ区切りにした文字列を追加
			str.Add(string.Join(",", row));
		}

		// ファイルのパス
		string path = Application.dataPath + "/" + fileName;

		// CSVデータの書き込み
		MyFunction.Writer(path, str);
	}


	// ****************************** 読み込み ****************************** //
	static public List<List<string>> ReadCSV(TextAsset csvData)
	{
		// テキストアセットからテキストを取り出す
		string data = csvData.text;

		return ReadCSV(data);
	}
	static public List<List<string>> ReadCSV(string csvData)
	{
		List<List<string>> list = new();

		// 1行ずつ取り出す
		string[] lines = csvData.Split('\n');

		// 各行のデータをリストに入れていく
		foreach (string line in lines)
		{
			// 文字列がない
			if (line == "")
				break;

			// 改行を削除した文字列をカンマ区切りで配列に格納
			string[] texts = line.Remove(line.Length - 1).Split(",");

			// １行分のリスト
			List<string> li = new();

			// 一文字づつリストに追加
			foreach (string text in texts)
			{
				li.Add(text);
			}

			// リストに追加
			list.Add(li);
		}

		return list;
	}

	static public List<List<TEnum>> ReadCSV<TEnum>(TextAsset csvData)
	{
		// テキストアセットからテキストを取り出す
		string data = csvData.text;

		return ReadCSV<TEnum>(data);
	}
	static public List<List<TEnum>> ReadCSV<TEnum>(string csvData)
	{
		List<List<TEnum>> list = new();

		// 1行ずつ取り出す
		string[] lines = csvData.Split('\n');

		// 各行のデータをリストに入れていく
		foreach (string line in lines)
		{
			// 文字列がない
			if (line == "")
				break;

			// 改行を削除した文字列をカンマ区切りで配列に格納
			string[] texts = line.Remove(line.Length - 1).Split(",");

			// １行分のリスト
			List<TEnum> li = new();

			// 一文字づつリストに追加
			foreach (string text in texts)
			{
				li.Add((TEnum)Enum.Parse(typeof(TEnum), text));
			}

			// リストに追加
			list.Add(li);
		}

		return list;
	}

}
