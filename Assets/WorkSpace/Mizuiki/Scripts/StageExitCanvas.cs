using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExitCanvas : CheckCanvas
{
	public override void Decision()
	{
		base.Decision();

		// セーブ
		SaveDataReadWrite.m_instance.Save();

		// シーン切り替え
		SceneManager.LoadScene("StageSelectScene");
	}


}
