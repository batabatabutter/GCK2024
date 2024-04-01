using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneUICanvas : MonoBehaviour
{
    //  プレイシーンマネージャー
    private PlaySceneManager m_manager;

    //  プレイシーンマネージャー設定
    public void SetPlayScenemManager(PlaySceneManager manager) { m_manager = manager; }
    //  プレイシーンマネージャー取得
    public PlaySceneManager GetPlaySceneManager() { return m_manager; }
}
