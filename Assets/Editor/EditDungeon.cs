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

	// ���n�^�C���}�b�v��
	private string m_baseTilemapName = "Base";
	// �u���b�N�^�C���}�b�v��
	private string m_blockTilemapName = "Block";

	// ���n�^�C���̃X�v���C�g�̃p�X
	private string m_baseTileSpritePath = "Assets/MapChip/a2_0.asset";
	// �j��s�\�u���b�N�^�C���̃X�v���C�g�̃p�X
	private string m_dontBrokenSpritePath = "Assets/MapChip/a5_21.asset";


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

		// ���n�^�C���}�b�v��
		GUILayout.Label("���n�^�C���}�b�v��");
		m_baseTilemapName = GUILayout.TextField(m_baseTilemapName);
		// �u���b�N�^�C���}�b�v��
		GUILayout.Label("�u���b�N�^�C���}�b�v��");
		m_blockTilemapName = GUILayout.TextField(m_blockTilemapName);

		// �Ԃ�������
		GUILayout.Space(10);

		// ���n�^�C���̃p�X
		GUILayout.Label("���n�^�C���̃p�X");
		m_baseTileSpritePath = GUILayout.TextField(m_baseTileSpritePath);
		// �j��s�\�ȃu���b�N�̃^�C���̃p�X
		GUILayout.Label("�j��s�\�ȃu���b�N�̃^�C���̃p�X");
		m_dontBrokenSpritePath = GUILayout.TextField(m_dontBrokenSpritePath);

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

		// ���n�^�C��
		Tilemap baseTile = null;
		// �u���b�N�^�C��
		Tilemap blockTile = null;

		foreach (Tilemap tilemap in tileMaps)
		{
			// ���n�^�C�����擾
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

	}

	private string[] CreateMap(Tilemap baseTile, Tilemap blockTile)
	{
		// �}�b�v�̃T�C�Y�擾
		Vector3Int size = baseTile.cellBounds.size;
		// �}�b�v�̊J�n�ʒu�擾
		Vector3Int position = baseTile.cellBounds.position;

		// ���n�^�C���̃X�v���C�g�擾
		Sprite baseSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_baseTileSpritePath).sprite;
		// �j��s�\�^�C���̃X�v���C�g�擾
		Sprite dontBrokenSprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(m_dontBrokenSpritePath).sprite;

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

				// �󂹂Ȃ��ǂ̃X�v���C�g
				Sprite ws = blockTile.GetSprite(pos);
				// �ݒ肳�ꂽ�X�v���C�g�ł���� 2
				if (ws == dontBrokenSprite)
				{
					row[x] = "2";
				}

			}

			// �J���}��؂�ɂ����������ǉ�
			ret[y] = string.Join(",", row);

		}

		// �J���}��؂�ɂ��������z��
		return ret;
	}

}
