using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteLoadCSV : MonoBehaviour
{
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

		// 書き込みファイル
		StreamWriter stream = new(path, false);

		// 文字列をファイルに追加
		for (int i = 0; i < str.Count; i++)
		{
			stream.WriteLine(str[i]);
		}

		// ファイルを閉じる
		stream.Close();
		
	}

	static public List<List<T>> LoadCSV<T>(TextAsset csvData)
	{
		List<List<T>> list = new();

		// テキストアセットからテキストを取り出す
		string data = csvData.text;
		// 1行ずつ取り出す
		string[] lines = data.Split('\n');

		// 各行のデータをリストに入れていく
		foreach (string line in lines)
		{
			string[] texts = line.Split(",");

			// １行分のリスト
			List<T> li = new();

			// 一文字づつリストに追加
			foreach (string text in texts)
			{
				li.Add((T)Enum.Parse(typeof(T), text));
			}

			// リストに追加
			list.Add(li);
		}

		return list;
	}

}
