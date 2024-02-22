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
		// guid����A�Z�b�g�̃p�X���擾���܂�
		string assetPath = AssetDatabase.GUIDToAssetPath(guid);
		if (string.IsNullOrEmpty(assetPath))
		{
			return;
		}

		// Packages�t�H���_�͏��O
		if (assetPath.StartsWith("Packages"))
		{
			return;
		}

		// �A�C�R�����ŏ������Ă���ꍇ�ɂ̂ݗL��������
		if (selectionRect.height >= 20f)
		{
			return;
		}

		// �{�^�����\���ł��镝���\���m�ۂł��Ă��Ȃ���Ε\�����Ȃ�
		if (selectionRect.width < 200f)
		{
			return;
		}

		// �{�^���̕\���͈͂�ݒ肵�܂�
		Vector2 buttonSize = new Vector2(80f, selectionRect.size.y);
		Rect buttonRect = selectionRect;
		buttonRect.x = buttonRect.xMax - buttonSize.x;
		buttonRect.size = buttonSize;

		// �{�^����z�u���܂�
		if (GUI.Button(buttonRect, "Copy Path"))
		{
			// �{�^���������ꂽ�Ƃ��Ƀp�X���N���b�v�{�[�h�ɐݒ肵�܂�
			EditorGUIUtility.systemCopyBuffer = assetPath;
		}
	}
}