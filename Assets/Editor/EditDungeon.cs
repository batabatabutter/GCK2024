using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class EditDungeon : EditorWindow
{
	// ファイルの階層
	private string m_path = "Assets/DungeonData/";

	// ファイル名
    private string m_fileName = "";

	// 地面タイルマップ名
	private string m_baseTilemapName = "Base";
	// ブロックタイルマップ名
	private string m_blockTilemapName = "Block";

	//// 地面タイルの保存名
	//private string m_baseTileName = "1";
	//// 岩盤ブロックの保存名
	//private string m_bedrockBlockName = "2";
	//// 核の生成範囲ブロック
	//private string m_coreBlockName = "3";

	//// 地面タイルのスプライトのパス
	//private string m_baseTileSpritePath = "Assets/MapChip/a2_0.asset";
	//// 破壊不可能ブロックタイルのスプライトのパス
	//private string m_bedrockBlockSpritePath = "Assets/MapChip/a5_21.asset";
	//// 核の生成範囲ブロックのタイルのスプライトパス
	//private string m_coreBlockSpritePath = "Assets/MapChip/a5_84.asset";

	// 空白タイル名
	private string m_nullTileName = "0";

	// 配列にする用のタイルの情報
	[System.Serializable]
	public class MapTileSprite
	{
		public string	tileName = "0";
		public Sprite	tileSprite = null;
		public string	tilePath = "";

		public MapTileSprite(string name, string path)
		{
			tileName = name;
			tilePath = path;
		}
	}

	public MapTileSprite[] m_tiles = new MapTileSprite[]
	{
		new("1", "Assets/MapChip/a2_0.asset"),
		new ("2", "Assets/MapChip/a5_21.asset"),
		new ("3", "Assets/MapChip/a5_84.asset"),
	};

	// スクロールの値
	private Vector2 m_scrollValue = new();



	[MenuItem("GameObject/Dungeon")]
    static private void Open()
    {
		// ウィンドウを取得
		GetWindow<EditDungeon>();

	}

	private void OnGUI()
	{
		// スクロール開始
		m_scrollValue = EditorGUILayout.BeginScrollView(m_scrollValue);

		// ファイルの階層
		GUILayout.Label("ファイルの階層");
		m_path = GUILayout.TextField(m_path);

		// ファイル名
		GUILayout.Label("ファイル名");
		m_fileName = EditorGUILayout.TextField(m_fileName);

		// 間をあける
		GUILayout.Space(10);

		// 地面タイルマップ名
		GUILayout.Label("地面タイルマップ名");
		m_baseTilemapName = GUILayout.TextField(m_baseTilemapName);
		// ブロックタイルマップ名
		GUILayout.Label("ブロックタイルマップ名");
		m_blockTilemapName = GUILayout.TextField(m_blockTilemapName);

		// 間をあける
		GUILayout.Space(10);

		//// 地面タイルの保存名
		//GUILayout.Label("地面タイルの保存名");
		//m_baseTileName = GUILayout.TextField(m_baseTileName);
		//// 地面タイルのパス
		//GUILayout.Label("地面タイルのパス");
		//m_baseTileSpritePath = GUILayout.TextField(m_baseTileSpritePath);

		//// 間をあける
		//GUILayout.Space(10);

		//// 岩盤ブロックの保存名
		//GUILayout.Label("岩盤ブロックの保存名");
		//m_bedrockBlockName = GUILayout.TextField(m_bedrockBlockName);
		//// 岩盤ブロックのタイルのパス
		//GUILayout.Label("岩盤ブロックのタイルのパス");
		//m_bedrockBlockSpritePath = GUILayout.TextField(m_bedrockBlockSpritePath);

		//// 間をあける
		//GUILayout.Space(10);

		//// 核ブロックの保存名
		//GUILayout.Label("核ブロックの保存名");
		//m_coreBlockName = GUILayout.TextField(m_coreBlockName);
		//// 核ブロックのタイルのパス
		//GUILayout.Label("核ブロックのタイルのパス");
		//m_coreBlockSpritePath = GUILayout.TextField(m_coreBlockSpritePath);

		GUILayout.Label("タイルの保存名とパス");
		// 参考 : https://bta25fun.wixsite.com/modev/post/2019/12/19/%E3%82%A8%E3%83%87%E3%82%A3%E3%82%BF%E6%8B%A1%E5%BC%B5%E3%81%A7%E9%85%8D%E5%88%97%E5%A4%89%E6%95%B0%E3%81%AE%E5%85%A5%E5%8A%9B%E6%AC%84%E3%82%92%E5%87%BA%E3%81%99%E3%81%AB%E3%81%AF
		ScriptableObject target = this;
		SerializedObject so = new(target);
		SerializedProperty stringsProperty = so.FindProperty("m_tiles");
		EditorGUILayout.PropertyField(stringsProperty, true);
		so.ApplyModifiedProperties();

		// 間をあける
		GUILayout.Space(10);

		// 空白タイル名
		GUILayout.Label("空白ブロック名");
		m_nullTileName = GUILayout.TextField(m_nullTileName);

		// 間をあける
		GUILayout.Space(10);

		// 保存
		if (GUILayout.Button("保存"))
		{
			Save();
		}

		// 間をあける
		GUILayout.Space(100);

		// コメント
		GUILayout.Label("地面タイルの上にブロックが生成される");
		GUILayout.Label("CSVファイル上下反転してるけど仕様です");
		GUILayout.Label("ファイルパスはプロジェクトからコピーできるよ");
		GUILayout.Label("ドラッグでスプライトを設定できるようにした");

		// スクロール終了
		EditorGUILayout.EndScrollView();


	}

	// タイルマップの情報をファイルに書き出す
	private void Save()
	{
		// 全てのタイルマップを取得する
		Tilemap[] tileMaps = FindObjectsOfType(typeof(Tilemap)) as Tilemap[];

		// タイルマップがなければ処理しない
		if (tileMaps.Length == 0)
		{
			Debug.Log("タイルマップないよ");
			return;
		}

		// ファイル名が設定されてなければ処理しない
		if (m_fileName == "")
		{
			Debug.Log("ファイル名を設定してね");
			return;
		}

		// 地面タイル
		Tilemap baseTile = null;
		// ブロックタイル
		Tilemap blockTile = null;

		foreach (Tilemap tilemap in tileMaps)
		{
			// 地面タイルを取得
			if (tilemap.name == m_baseTilemapName)
			{
				baseTile = tilemap;
			}
			// ブロックタイルを取得
			if (tilemap.name == m_blockTilemapName)
			{
				blockTile = tilemap;
			}
		}

		// マップを文字配列に変換
		string[] str = CreateMap(baseTile, blockTile);

		// 文字列が存在しない
		if (str.Length <= 0)
		{
			Debug.Log("保存するものがないよ");
			return;
		}

		// ファイルのパス
		string path = m_path + m_fileName + ".csv";

		// 書き込みファイル
		StreamWriter stream = new(path, false);

		// 文字列をファイルに追加
		for (int i = 0; i < str.Length; i++)
		{
			stream.WriteLine(str[i]);
		}

		// ファイルを閉じる
		stream.Close();

		Debug.Log("保存完了！");

	}

	private string[] CreateMap(Tilemap baseTile, Tilemap blockTile)
	{
		// タイルマップがどっちもない
		if (baseTile == null && blockTile == null)
		{
			Debug.Log("タイルマップがないよ");
			return new string[0];
		}

		// タイルのスプライトを設定する
		SetTileSprite();

		// マップのサイズ取得
		Vector3Int size = baseTile.cellBounds.size;
		// マップの開始位置取得
		Vector3Int position = baseTile.cellBounds.position;

		//// 地面タイルのスプライト取得
		//Sprite baseSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_baseTileSpritePath).sprite;
		//// 岩盤タイルのスプライト取得
		//Sprite bedrockSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_bedrockBlockSpritePath).sprite;
		//// 核タイルのスプライト取得
		//Sprite coreSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_coreBlockSpritePath).sprite;

		// 戻り値用の空の文字配列
		string[] ret = new string[size.y];

		for (int y = 0; y < size.y; y++)
		{
			// 1行分のデータ
			string[] row = new string[size.x];

			for (int x = 0; x < size.x; x++)
			{
				// タイルの位置
				Vector3Int pos = position + new Vector3Int(x, y, baseTile.cellBounds.position.z);

				if (baseTile)
				{
					// タイルのスプライト
					Sprite bs = GetSprite(baseTile, pos);

					// スプライトがあればタイル名を取得
					if (bs)
					{
						row[x] = GetTileName(bs);
					}
					// スプライトがなければ 0
					else
					{
						row[x] = "0";
					}
				}

				if (blockTile)
				{
					// 壁のスプライト
					Sprite ws = GetSprite(blockTile, pos);

					// スプライトがあればタイル名を取得
					if (ws)
					{
						row[x] = GetTileName(ws);
					}
				}

			}

			// カンマ区切りにした文字列を追加
			ret[y] = string.Join(",", row);

		}

		// カンマ区切りにした文字配列
		return ret;
	}

	// スプライトを取得
	private Sprite GetSprite(Tilemap tile, Vector3Int pos)
	{
		// タイルがない
		if (tile == null)
		{
			return null;
		}

		return tile.GetSprite(pos);
	}

	// タイル名を取得
	private string GetTileName(Sprite sprite)
	{
		for (int i = 0; i < m_tiles.Length; i++)
		{
			// タイルのスプライトを比較
			if (m_tiles[i].tileSprite == sprite)
			{
				// タイル名を返す
				return m_tiles[i].tileName;
			}
		}

		// 空白タイル名を返す
		return m_nullTileName;
	}

	private void SetTileSprite()
	{
		for (int i = 0; i < m_tiles.Length; i++)
		{
			// スプライトがあればパスを設定する
			if (m_tiles[i].tileSprite)
			{
				//m_tiles[i].tilePath = m_tiles[i].tileSprite.
				System.IO.Path.GetFullPath(m_tiles[i].tileSprite.name);
			}
			// スプライトがない場合はパスのスプライトを設定する
			else
			{
				m_tiles[i].tileSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_tiles[i].tilePath).sprite;
			}
		}
	}

}
