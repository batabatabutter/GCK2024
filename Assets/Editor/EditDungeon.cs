using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class EditDungeon : EditorWindow
{
	// �t�@�C���̊K�w
	private string m_path = "Assets/DungeonData/";

	// �t�@�C����
    private string m_fileName = "";

	// �n�ʃ^�C���}�b�v��
	private string m_baseTilemapName = "Base";
	// �u���b�N�^�C���}�b�v��
	private string m_blockTilemapName = "Block";

	//// �n�ʃ^�C���̕ۑ���
	//private string m_baseTileName = "1";
	//// ��Ճu���b�N�̕ۑ���
	//private string m_bedrockBlockName = "2";
	//// �j�̐����͈̓u���b�N
	//private string m_coreBlockName = "3";

	//// �n�ʃ^�C���̃X�v���C�g�̃p�X
	//private string m_baseTileSpritePath = "Assets/MapChip/a2_0.asset";
	//// �j��s�\�u���b�N�^�C���̃X�v���C�g�̃p�X
	//private string m_bedrockBlockSpritePath = "Assets/MapChip/a5_21.asset";
	//// �j�̐����͈̓u���b�N�̃^�C���̃X�v���C�g�p�X
	//private string m_coreBlockSpritePath = "Assets/MapChip/a5_84.asset";

	// �󔒃^�C����
	private string m_nullTileName = "0";

	// �z��ɂ���p�̃^�C���̏��
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

	// �X�N���[���̒l
	private Vector2 m_scrollValue = new();



	[MenuItem("GameObject/Dungeon")]
    static private void Open()
    {
		// �E�B���h�E���擾
		GetWindow<EditDungeon>();

	}

	private void OnGUI()
	{
		// �X�N���[���J�n
		m_scrollValue = EditorGUILayout.BeginScrollView(m_scrollValue);

		// �t�@�C���̊K�w
		GUILayout.Label("�t�@�C���̊K�w");
		m_path = GUILayout.TextField(m_path);

		// �t�@�C����
		GUILayout.Label("�t�@�C����");
		m_fileName = EditorGUILayout.TextField(m_fileName);

		// �Ԃ�������
		GUILayout.Space(10);

		// �n�ʃ^�C���}�b�v��
		GUILayout.Label("�n�ʃ^�C���}�b�v��");
		m_baseTilemapName = GUILayout.TextField(m_baseTilemapName);
		// �u���b�N�^�C���}�b�v��
		GUILayout.Label("�u���b�N�^�C���}�b�v��");
		m_blockTilemapName = GUILayout.TextField(m_blockTilemapName);

		// �Ԃ�������
		GUILayout.Space(10);

		//// �n�ʃ^�C���̕ۑ���
		//GUILayout.Label("�n�ʃ^�C���̕ۑ���");
		//m_baseTileName = GUILayout.TextField(m_baseTileName);
		//// �n�ʃ^�C���̃p�X
		//GUILayout.Label("�n�ʃ^�C���̃p�X");
		//m_baseTileSpritePath = GUILayout.TextField(m_baseTileSpritePath);

		//// �Ԃ�������
		//GUILayout.Space(10);

		//// ��Ճu���b�N�̕ۑ���
		//GUILayout.Label("��Ճu���b�N�̕ۑ���");
		//m_bedrockBlockName = GUILayout.TextField(m_bedrockBlockName);
		//// ��Ճu���b�N�̃^�C���̃p�X
		//GUILayout.Label("��Ճu���b�N�̃^�C���̃p�X");
		//m_bedrockBlockSpritePath = GUILayout.TextField(m_bedrockBlockSpritePath);

		//// �Ԃ�������
		//GUILayout.Space(10);

		//// �j�u���b�N�̕ۑ���
		//GUILayout.Label("�j�u���b�N�̕ۑ���");
		//m_coreBlockName = GUILayout.TextField(m_coreBlockName);
		//// �j�u���b�N�̃^�C���̃p�X
		//GUILayout.Label("�j�u���b�N�̃^�C���̃p�X");
		//m_coreBlockSpritePath = GUILayout.TextField(m_coreBlockSpritePath);

		GUILayout.Label("�^�C���̕ۑ����ƃp�X");
		// �Q�l : https://bta25fun.wixsite.com/modev/post/2019/12/19/%E3%82%A8%E3%83%87%E3%82%A3%E3%82%BF%E6%8B%A1%E5%BC%B5%E3%81%A7%E9%85%8D%E5%88%97%E5%A4%89%E6%95%B0%E3%81%AE%E5%85%A5%E5%8A%9B%E6%AC%84%E3%82%92%E5%87%BA%E3%81%99%E3%81%AB%E3%81%AF
		ScriptableObject target = this;
		SerializedObject so = new(target);
		SerializedProperty stringsProperty = so.FindProperty("m_tiles");
		EditorGUILayout.PropertyField(stringsProperty, true);
		so.ApplyModifiedProperties();

		// �Ԃ�������
		GUILayout.Space(10);

		// �󔒃^�C����
		GUILayout.Label("�󔒃u���b�N��");
		m_nullTileName = GUILayout.TextField(m_nullTileName);

		// �Ԃ�������
		GUILayout.Space(10);

		// �ۑ�
		if (GUILayout.Button("�ۑ�"))
		{
			Save();
		}

		// �Ԃ�������
		GUILayout.Space(100);

		// �R�����g
		GUILayout.Label("�n�ʃ^�C���̏�Ƀu���b�N�����������");
		GUILayout.Label("CSV�t�@�C���㉺���]���Ă邯�ǎd�l�ł�");
		GUILayout.Label("�t�@�C���p�X�̓v���W�F�N�g����R�s�[�ł����");
		GUILayout.Label("�h���b�O�ŃX�v���C�g��ݒ�ł���悤�ɂ���");

		// �X�N���[���I��
		EditorGUILayout.EndScrollView();


	}

	// �^�C���}�b�v�̏����t�@�C���ɏ����o��
	private void Save()
	{
		// �S�Ẵ^�C���}�b�v���擾����
		Tilemap[] tileMaps = FindObjectsOfType(typeof(Tilemap)) as Tilemap[];

		// �^�C���}�b�v���Ȃ���Ώ������Ȃ�
		if (tileMaps.Length == 0)
		{
			Debug.Log("�^�C���}�b�v�Ȃ���");
			return;
		}

		// �t�@�C�������ݒ肳��ĂȂ���Ώ������Ȃ�
		if (m_fileName == "")
		{
			Debug.Log("�t�@�C������ݒ肵�Ă�");
			return;
		}

		// �n�ʃ^�C��
		Tilemap baseTile = null;
		// �u���b�N�^�C��
		Tilemap blockTile = null;

		foreach (Tilemap tilemap in tileMaps)
		{
			// �n�ʃ^�C�����擾
			if (tilemap.name == m_baseTilemapName)
			{
				baseTile = tilemap;
			}
			// �u���b�N�^�C�����擾
			if (tilemap.name == m_blockTilemapName)
			{
				blockTile = tilemap;
			}
		}

		// �}�b�v�𕶎��z��ɕϊ�
		string[] str = CreateMap(baseTile, blockTile);

		// �����񂪑��݂��Ȃ�
		if (str.Length <= 0)
		{
			Debug.Log("�ۑ�������̂��Ȃ���");
			return;
		}

		// �t�@�C���̃p�X
		string path = m_path + m_fileName + ".csv";

		// �������݃t�@�C��
		StreamWriter stream = new(path, false);

		// ��������t�@�C���ɒǉ�
		for (int i = 0; i < str.Length; i++)
		{
			stream.WriteLine(str[i]);
		}

		// �t�@�C�������
		stream.Close();

		Debug.Log("�ۑ������I");

	}

	private string[] CreateMap(Tilemap baseTile, Tilemap blockTile)
	{
		// �^�C���}�b�v���ǂ������Ȃ�
		if (baseTile == null && blockTile == null)
		{
			Debug.Log("�^�C���}�b�v���Ȃ���");
			return new string[0];
		}

		// �^�C���̃X�v���C�g��ݒ肷��
		SetTileSprite();

		// �}�b�v�̃T�C�Y�擾
		Vector3Int size = baseTile.cellBounds.size;
		// �}�b�v�̊J�n�ʒu�擾
		Vector3Int position = baseTile.cellBounds.position;

		//// �n�ʃ^�C���̃X�v���C�g�擾
		//Sprite baseSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_baseTileSpritePath).sprite;
		//// ��Ճ^�C���̃X�v���C�g�擾
		//Sprite bedrockSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_bedrockBlockSpritePath).sprite;
		//// �j�^�C���̃X�v���C�g�擾
		//Sprite coreSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_coreBlockSpritePath).sprite;

		// �߂�l�p�̋�̕����z��
		string[] ret = new string[size.y];

		for (int y = 0; y < size.y; y++)
		{
			// 1�s���̃f�[�^
			string[] row = new string[size.x];

			for (int x = 0; x < size.x; x++)
			{
				// �^�C���̈ʒu
				Vector3Int pos = position + new Vector3Int(x, y, baseTile.cellBounds.position.z);

				if (baseTile)
				{
					// �^�C���̃X�v���C�g
					Sprite bs = GetSprite(baseTile, pos);

					// �X�v���C�g������΃^�C�������擾
					if (bs)
					{
						row[x] = GetTileName(bs);
					}
					// �X�v���C�g���Ȃ���� 0
					else
					{
						row[x] = "0";
					}
				}

				if (blockTile)
				{
					// �ǂ̃X�v���C�g
					Sprite ws = GetSprite(blockTile, pos);

					// �X�v���C�g������΃^�C�������擾
					if (ws)
					{
						row[x] = GetTileName(ws);
					}
				}

			}

			// �J���}��؂�ɂ����������ǉ�
			ret[y] = string.Join(",", row);

		}

		// �J���}��؂�ɂ��������z��
		return ret;
	}

	// �X�v���C�g���擾
	private Sprite GetSprite(Tilemap tile, Vector3Int pos)
	{
		// �^�C�����Ȃ�
		if (tile == null)
		{
			return null;
		}

		return tile.GetSprite(pos);
	}

	// �^�C�������擾
	private string GetTileName(Sprite sprite)
	{
		for (int i = 0; i < m_tiles.Length; i++)
		{
			// �^�C���̃X�v���C�g���r
			if (m_tiles[i].tileSprite == sprite)
			{
				// �^�C������Ԃ�
				return m_tiles[i].tileName;
			}
		}

		// �󔒃^�C������Ԃ�
		return m_nullTileName;
	}

	private void SetTileSprite()
	{
		for (int i = 0; i < m_tiles.Length; i++)
		{
			// �X�v���C�g������΃p�X��ݒ肷��
			if (m_tiles[i].tileSprite)
			{
				//m_tiles[i].tilePath = m_tiles[i].tileSprite.
				System.IO.Path.GetFullPath(m_tiles[i].tileSprite.name);
			}
			// �X�v���C�g���Ȃ��ꍇ�̓p�X�̃X�v���C�g��ݒ肷��
			else
			{
				m_tiles[i].tileSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_tiles[i].tilePath).sprite;
			}
		}
	}

}
