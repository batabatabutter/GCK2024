using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameCanvas : CheckCanvas
{
	public override void Decision()
	{
		// �Z�[�u
		SaveDataReadWrite.m_instance.Save();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
            Application.Quit();//�Q�[���v���C�I��
#endif


	}


}
