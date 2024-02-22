using System.IO;
using UnityEngine;
using UnityEditor;

// https://media.colorfulpalette.co.jp/n/nef215d75b5fc
public class PathGet
{
	[InitializeOnLoadMethod]
	private static void Initialize()
	{
		EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
	}

	private static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
	{
		// guidからアセットのパスを取得します
		string assetPath = AssetDatabase.GUIDToAssetPath(guid);
		if (string.IsNullOrEmpty(assetPath))
		{
			return;
		}

		// Packagesフォルダは除外
		if (assetPath.StartsWith("Packages"))
		{
			return;
		}

		// アイコンが最小化している場合にのみ有効化する
		if (selectionRect.height >= 20f)
		{
			return;
		}

		// ボタンが表示できる幅が十分確保できていなければ表示しない
		if (selectionRect.width < 200f)
		{
			return;
		}

		// ボタンの表示範囲を設定します
		Vector2 buttonSize = new Vector2(80f, selectionRect.size.y);
		Rect buttonRect = selectionRect;
		buttonRect.x = buttonRect.xMax - buttonSize.x;
		buttonRect.size = buttonSize;

		// ボタンを配置します
		if (GUI.Button(buttonRect, "Copy Path"))
		{
			// ボタンが押されたときにパスをクリップボードに設定します
			EditorGUIUtility.systemCopyBuffer = assetPath;
		}
	}
}