using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUI : MonoBehaviour
{
    //  シーンマネージャー
    [Header("プレイシーンマネージャー")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    //  プレイヤー
    private GameObject m_player;

    //  ツールUIのプレハブ
    [Header("ツールUIのプレハブ")]
    [SerializeField] private List<Sprite> m_toolGraph;
    [SerializeField] private GameObject m_toolFrame;
    [SerializeField] private Vector2 m_offset;

    //  HP格納
    private List<GameObject> m_toolObjects = new List<GameObject>();

    [Header("ツールのデータベース")]
    [SerializeField] private ToolDataBase m_data;

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
        //  プレイシーンマネージャーが無かったら格納しない
        if (m_playSceneManager == null)
            Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:ToolUI");
        else
        {
            //  プレイヤー格納
            m_player = m_playSceneManager.GetPlayer();
        }
        //  プレイヤーが見つからなかったらデバッグ状態に
        if (m_player == null) m_debug = true;

        //  UI生成
        //  生成位置
        Vector2 size = m_toolFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < (int)ToolData.ToolType.OVER; i++)
        {
            //  座標
            pos = new Vector3((size.x + m_offset.x)* i, 0.0f) + transform.position;
            //  UI生成
            GameObject frame = Instantiate(m_toolFrame, pos, Quaternion.identity, transform);
            //  画像設定
            frame.GetComponent<ToolFrame>().SetImage(m_toolGraph[i]);

            m_toolObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  ツール更新
        for (int i = 0; i < (int)ToolData.ToolType.OVER; i++)
        {
            //  ツール数設定
            m_toolObjects[i].GetComponent<ToolFrame>().SetIsSelected(false);
            //  ツール作成可能数設定
            m_toolObjects[i].GetComponent<ToolFrame>().SetNum(GetToolUseNum((ToolData.ToolType)i));
        }

        //  ツール選択状態参照
        m_toolObjects[0].GetComponent<ToolFrame>().SetIsSelected(true); 
    }

    //  ツール作成可能数取得
    public int GetToolUseNum(ToolData.ToolType toolType)
    {
        //  可能数
        int num = 0;

        //  アイテム数取得
        for (int i = 0; i < m_data.tool[(int)toolType].itemMaterials.Count; i++)
        {

        }

        return num;
    }
}
