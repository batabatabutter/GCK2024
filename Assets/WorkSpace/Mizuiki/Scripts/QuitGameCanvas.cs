using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameCanvas : CheckCanvas
{
	public override void Decision()
	{
		// セーブ
		SaveDataReadWrite.m_instance.Save();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
            Application.Quit();//ゲームプレイ終了
#endif


	}


}
