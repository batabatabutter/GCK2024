using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuitGameBlock : Block
{
	[Header("�Q�[���I���L�����o�X")]
	[SerializeField] private QuitGameCanvas m_canvas = null;

	public override bool AddMiningDamage(float power, int dropCount = 1)
	{
		// �L�����o�X���J��
		m_canvas.SetEnabled(true);

		return false;
	}

	//public override void DropItem(int stageID = 1)
	//{
	//    #if UNITY_EDITOR
	//        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
	//    #else
	//        Application.Quit();//�Q�[���v���C�I��
	//    #endif
	//}
}
