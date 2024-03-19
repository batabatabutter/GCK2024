using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUI : MonoBehaviour
{
    //  プレイヤー
    [Header("プレイヤー情報")]
    [SerializeField] Player m_player;

    //  ツールUIのプレハブ
    [Header("ツールUIのプレハブ")]
    [SerializeField] GameObject m_toolIcon;

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //  プレイヤーが見つからなかったらデバッグ状態に
        if (m_player == null) m_debug = true;
    }
}
