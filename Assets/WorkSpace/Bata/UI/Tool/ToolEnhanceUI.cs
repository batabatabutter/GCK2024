using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static PlayerTool;
using static UnityEngine.GridBrushBase;

public class ToolEnhanceUI : MonoBehaviour
{
    [Header("プレハブ")]
    [SerializeField] GameObject m_toolFrameObj;
    [SerializeField] Vector2 m_toolOffset = Vector2.zero;

    //  ツールデータベース
    [Header("ツールのデータベース")]
    [SerializeField] private ToolDataBase m_toolDataBase;

    //  生成したUI格納
    private List<ToolEnhanceFrame> m_toolFrames = new List<ToolEnhanceFrame>();

    //  シーンマネージャー
    private PlaySceneManager m_playSceneManager;
    //  プレイヤー
    private GameObject m_player;
    private PlayerTool m_plTool;

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
        //  プレイシーンマネージャー設定
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        // ツール取得
        foreach (Tool tool in m_plTool.ToolScripts.Values)
        {
            //  掘る奴だけ取得
            if (tool.GetType() == typeof(ToolMining))
            {
                var frame = Instantiate(m_toolFrameObj, transform).GetComponent<ToolEnhanceFrame>();
                frame.transform.localScale = Vector3.zero;
                frame.ToolMining = (ToolMining)tool;
                frame.ToolIcon.sprite = m_toolDataBase.toolDic[((ToolMining)tool).ToolType].Icon;
                m_toolFrames.Add(frame);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        m_toolFrames.Sort((a, b) => (int)a.Bar.transform.localScale.x - (int)b.Bar.transform.localScale.x);

        int count = 0;

        foreach (var frame in m_toolFrames)
        {
            if (frame.Bar.transform.localScale.x > 0.0f)
            {
                frame.transform.localPosition = new Vector2(0.0f, -m_toolOffset.y * count);
                count++;
            }
        }
    }

    //  プレイシーン設定
    public void SetPlaySceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

        //  プレイシーンマネージャーが無かったら格納しない
        if (m_playSceneManager == null)
            Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:" + this);
        else
        {
            //  プレイヤー格納
            m_player = m_playSceneManager.GetPlayer();
            m_plTool = m_player.GetComponent<PlayerTool>();
        }
        //  プレイヤーが見つからなかったらデバッグ状態に
        if (m_player == null) m_debug = true;
    }
}
