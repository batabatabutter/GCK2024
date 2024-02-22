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
	private readonly string m_path = "Assets/DungeonData/";

	// �t�@�C����
    private string m_fileName = "";

	// �n�ʃ^�C���}�b�v��
	private string m_baseTilemapName = "Base";
	// �u���b�N�^�C���}�b�v��
	private string m_blockTilemapName = "Block";

	// �n�ʃ^�C���̕ۑ���
	private string m_baseTileName = "1";
	// ��Ճu���b�N�̕ۑ���
	private string m_bedrockBlockName = "2";
	// �j�̐����͈̓u���b�N
	private string m_coreBlockName = "3";

	// �n�ʃ^�C���̃X�v���C�g�̃p�X
	private string m_baseTileSpritePath = "Assets/MapChip/a2_0.asset";
	// �j��s�\�u���b�N�^�C���̃X�v���C�g�̃p�X
	private string m_bedrockBlockSpritePath = "Assets/MapChip/a5_21.asset";
	// �j�̐����͈̓u���b�N�̃^�C���̃X�v���C�g�p�X
	private string m_coreBlockSpritePath = "Assets/MapChip/a5_84.asset";


	[MenuItem("GameObject/Dungeon")]
    static private void Open()
    {
		// �E�B���h�E���擾
		GetWindow<EditDungeon>();
	}

	private void OnGUI()
	{
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

		// �n�ʃ^�C���̕ۑ���
		GUILayout.Label("�n�ʃ^�C���̕ۑ���");
		m_baseTileName = GUILayout.TextField(m_baseTileName);
		// �n�ʃ^�C���̃p�X
		GUILayout.Label("�n�ʃ^�C���̃p�X");
		m_baseTileSpritePath = GUILayout.TextField(m_baseTileSpritePath);

		// �Ԃ�������
		GUILayout.Space(10);

		// ��Ճu���b�N�̕ۑ���
		GUILayout.Label("��Ճu���b�N�̕ۑ���");
		m_bedrockBlockName = GUILayout.TextField(m_bedrockBlockName);
		// ��Ճu���b�N�̃^�C���̃p�X
		GUILayout.Label("��Ճu���b�N�̃^�C���̃p�X");
		m_bedrockBlockSpritePath = GUILayout.TextField(m_bedrockBlockSpritePath);

		// �Ԃ�������
		GUILayout.Space(10);

		// �j�u���b�N�̕ۑ���
		GUILayout.Label("�j�u���b�N�̕ۑ���");
		m_coreBlockName = GUILayout.TextField(m_coreBlockName);
		// �j�u���b�N�̃^�C���̃p�X
		GUILayout.Label("�j�u���b�N�̃^�C���̃p�X");
		m_coreBlockSpritePath = GUILayout.TextField(m_coreBlockSpritePath);


		// �Ԃ�������
		GUILayout.Space(10);

		//// �_���W�����̃T�C�Y
		//GUILayout.Label("�_���W�����T�C�Y");
		//GUILayout.TextField("1 * 1");

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
		// �}�b�v�̃T�C�Y�擾
		Vector3Int size = baseTile.cellBounds.size;
		// �}�b�v�̊J�n�ʒu�擾
		Vector3Int position = baseTile.cellBounds.position;

		// �n�ʃ^�C���̃X�v���C�g�擾
		Sprite baseSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_baseTileSpritePath).sprite;
		// ��Ճ^�C���̃X�v���C�g�擾
		Sprite bedrockSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_bedrockBlockSpritePath).sprite;
		// �j�^�C���̃X�v���C�g�擾
		Sprite coreSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_coreBlockSpritePath).sprite;

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

				// �^�C���̃X�v���C�g
				Sprite bs = baseTile.GetSprite(pos);

				// �X�v���C�g������� 1
				if (bs == baseSprite)
				{
					row[x] = "1";
				}
				// �X�v���C�g���Ȃ���� 0
				else
				{
					row[x] = "0";
				}

				// �ǂ̃X�v���C�g
				Sprite ws = blockTile.GetSprite(pos);

				// �󂹂Ȃ��ǂ̃X�v���C�g�ł���� 2
				if (ws == bedrockSprite)
				{
					row[x] = "2";
				}

				// �j�̃X�v���C�g�ł���� 3
				if (ws == coreSprite)
				{
					row[x] = "3";
				}

			}

			// �J���}��؂�ɂ����������ǉ�
			ret[y] = string.Join(",", row);

		}

		// �J���}��؂�ɂ��������z��
		return ret;
	}

}