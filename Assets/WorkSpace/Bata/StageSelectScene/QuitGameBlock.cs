using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuitGameBlock : Block
{
	[Header("ゲーム終了キャンバス")]
	[SerializeField] private QuitGameCanvas m_canvas = null;

	public override bool AddMiningDamage(float power, int dropCount = 1)
	{
		// キャンバスを開く
		m_canvas.SetEnabled(true);

		return false;
	}

	//public override void DropItem(int stageID = 1)
	//{
	//    #if UNITY_EDITOR
	//        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
	//    #else
	//        Application.Quit();//ゲームプレイ終了
	//    #endif
	//}
}
