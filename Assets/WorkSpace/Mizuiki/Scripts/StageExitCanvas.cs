using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExitCanvas : CheckCanvas
{
	public override void Decision()
	{
		base.Decision();

		// �Z�[�u
		SaveDataReadWrite.m_instance.Save();

		// �V�[���؂�ւ�
		SceneManager.LoadScene("StageSelectScene");
	}


}
