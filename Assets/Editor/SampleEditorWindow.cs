using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SampleEditorWindow : EditorWindow
{
	private string text = "";

	[MenuItem("GameObject/Sample")]
	static private void Open()
	{
		SampleEditorWindow window = GetWindow<SampleEditorWindow>();
	}

	private void OnGUI()
	{
		// ラベル
		GUILayout.Label("ラベル");

		// テキストボックス
		text = GUILayout.TextArea(text, GUILayout.Height(100));

		if (GUILayout.Button("タイルマップ"))
		{
			Tilemap[] gameObjects = FindObjectsOfType(typeof(Tilemap)) as Tilemap[];

			foreach (var obj in gameObjects)
			{
				//Debug.Log(obj.cellBounds);
			}

			// タイルマップの範囲を取得
			BoundsInt bounds = gameObjects[0].cellBounds;
			Vector3Int position = bounds.position;	// 始点
			Vector3Int size = bounds.size;			// 始点からのサイズ

			// 指定した位置にあるタイルのスプライト取得
			Sprite sprite = gameObjects[0].GetSprite(bounds.position);
			Debug.Log(sprite);

		}

		if (GUILayout.Button("CSV"))
		{
			// 書き込むCSVファイルを作る
			StreamWriter stream = new StreamWriter("TextCSV.csv");

			// 書き込む文字列
			string[] s = { "1", "2", "3" };
			// カンマ区切りにする
			string str = string.Join(",", s);

			// CSV書き込み
			stream.WriteLine(str);

			// 書き込み終了
			stream.Close();

		}
	}

}