using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneUICanvas : MonoBehaviour
{
    //  �v���C�V�[���}�l�[�W���[
    private PlaySceneManager m_manager;

    //  �v���C�V�[���}�l�[�W���[�ݒ�
    public void SetPlayScenemManager(PlaySceneManager manager) { m_manager = manager; }
    //  �v���C�V�[���}�l�[�W���[�擾
    public PlaySceneManager GetPlaySceneManager() { return m_manager; }
}
