using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUI : MonoBehaviour
{
    //  ツールUIのプレハブ
    [Header("ツールUIのプレハブ")]
    [SerializeField] private GameObject m_toolFrame;
    [SerializeField] private Vector2 m_offset;

    //  ツールデータベース
    [Header("ツールのデータベース")]
    [SerializeField] private ToolDataBase m_data;

    //  HP格納
    private List<GameObject> m_toolObjects = new List<GameObject>();

    //  シーンマネージャー
    private PlaySceneManager m_playSceneManager;
    //  プレイヤー
    private GameObject m_player;

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
        //  プレイシーンマネージャー設定
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

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
            frame.GetComponent<ToolFrame>().SetImage(m_data.tool[i].sprite);

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
            m_toolObjects[i].GetComponent<ToolFrame>().SetNum(GetToolUseNum(i));
        }

        //  ツール選択状態参照
        m_toolObjects[(int)m_player.GetComponent<PlayerAction>().ToolType].
            GetComponent<ToolFrame>().SetIsSelected(true); 
    }

    //  ツール作成可能数取得
    public int GetToolUseNum(int toolType)
    {
        //  可能数
        int num = int.MaxValue;

        //  アイテム数取得
        for (int i = 0; i < m_data.tool[toolType].itemMaterials.Count; i++)
        {
            //  アイテム
            ItemData.Type type = m_data.tool[toolType].itemMaterials[i].type;
            int count = m_data.tool[toolType].itemMaterials[i].count;

            //  所持アイテム数から作成可能数を割り出す
            num = Mathf.Min(num,
                m_player.transform.Find("Item").gameObject.
                GetComponent<PlayerItem>().Items[type] / count);
        }

        return num;
    }

    //  プレイシーン設定
    public void SetPlaySceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

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
    }
}
