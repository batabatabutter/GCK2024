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
		// ���x��
		GUILayout.Label("���x��");

		// �e�L�X�g�{�b�N�X
		text = GUILayout.TextArea(text, GUILayout.Height(100));

		if (GUILayout.Button("�^�C���}�b�v"))
		{
			Tilemap[] gameObjects = FindObjectsOfType(typeof(Tilemap)) as Tilemap[];

			foreach (var obj in gameObjects)
			{
				//Debug.Log(obj.cellBounds);
			}

			// �^�C���}�b�v�͈̔͂��擾
			BoundsInt bounds = gameObjects[0].cellBounds;
			Vector3Int position = bounds.position;	// �n�_
			Vector3Int size = bounds.size;			// �n�_����̃T�C�Y

			// �w�肵���ʒu�ɂ���^�C���̃X�v���C�g�擾
			Sprite sprite = gameObjects[0].GetSprite(bounds.position);
			Debug.Log(sprite);

		}

		if (GUILayout.Button("CSV"))
		{
			// ��������CSV�t�@�C�������
			StreamWriter stream = new StreamWriter("TextCSV.csv");

			// �������ޕ�����
			string[] s = { "1", "2", "3" };
			// �J���}��؂�ɂ���
			string str = string.Join(",", s);

			// CSV��������
			stream.WriteLine(str);

			// �������ݏI��
			stream.Close();

		}
	}

}