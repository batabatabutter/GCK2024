using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    //  プレイヤー
    private GameObject m_player;
    //  コア
    private GameObject m_core;
    private List<GameObject> m_cores;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //  プレイヤーの設定
    public void SetPlayer(GameObject player) { m_player = player; }
    //  プレイヤーの取得
    public GameObject GetPlayer() { return m_player; }

    //  コアの設定
    public void SetCore(GameObject core) { m_core = core; }
    //  コア達の取得
    public GameObject GetCore() { return m_core; }
    //  コアの追加
    public void AddCore(GameObject core) { m_cores.Add(core); }
    //  コア達の取得
    public List<GameObject> GetCores() { return m_cores; }
}
