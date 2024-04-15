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
    [SerializeField] private ToolDataBase m_toolDataBase;

    //  UI表示ツール数
    [Header("UIに必要な数")]
    [SerializeField] private int m_graphToolNum;
    [SerializeField, Range(0.0f, 1.0f)] private float m_graphScaleDeg;

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

        //  中心の番号
        int centerNum = m_graphToolNum / 2;

        float totalWidth = 0.0f;

        //  スライダー式
        for (int i = 0; i < m_graphToolNum; i++)
        {
            //  座標
            pos = new Vector3(totalWidth, 0.0f) + transform.position;
            //  UI生成
            GameObject frame = Instantiate(m_toolFrame, pos, Quaternion.identity, transform);

            //  大きさカウント
            if (i != centerNum)
                totalWidth += m_toolFrame.GetComponent<RectTransform>().sizeDelta.x * m_graphScaleDeg + m_offset.x;
            else
                totalWidth += m_toolFrame.GetComponent<RectTransform>().sizeDelta.x + m_offset.x;

            //  追加
            m_toolObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  スライダー式
        //  レアフラグ
        bool isRare = m_player.GetComponent<PlayerTool>().IsRareTool;
        //  ツールの数
        ToolData.ToolType playerToolType;
        int centerNum = m_toolObjects.Count / 2;

        //  ID
        int minID = 0;
        int maxID = 0;
        if (isRare)
        {
            minID = (int)ToolData.ToolType.RARE + 1;
            maxID = m_toolDataBase.toolRareData.Count + (int)ToolData.ToolType.RARE;
            playerToolType = m_player.GetComponent<PlayerTool>().ToolTypeRare;
        }
        else
        {
            minID = 0;
            maxID = m_toolDataBase.toolNormalData.Count - 1;
            playerToolType = m_player.GetComponent<PlayerTool>().ToolType;
        }

        //  ツールを番号に変換
        int playerToolNum = (int)playerToolType;

        //  デバッグ
        if (m_debug) Debug.Log("現在のツール:" + playerToolType);

        for (int i = 0; i < m_toolObjects.Count; i++)
        {
            //  対応ツール
            int thisToolID = playerToolNum - (centerNum - i);
            //  オーバーしてたら修正
            if (thisToolID < minID) thisToolID = (maxID + 1) - minID + thisToolID;
            else if (thisToolID > maxID) thisToolID = thisToolID - (maxID + 1) + minID;
            //  タイプに変換
            ToolData.ToolType thisToolType = (ToolData.ToolType)thisToolID;

            //  大きさ変更
            m_toolObjects[i].transform.localScale = Vector3.one * m_graphScaleDeg;

            //  ツール数設定
            m_toolObjects[i].GetComponent<ToolFrame>().SetIsSelected(false);
            //  ツール画像設定
            m_toolObjects[i].GetComponent<ToolFrame>().SetImage(m_toolDataBase.toolDic[thisToolType].Icon);
            //  ツール作成可能数設定
            //if (!isRare)
                m_toolObjects[i].GetComponent<ToolFrame>().SetNum(GetToolUseNum(thisToolType));

            //  クールタイムがあるなら0.0～1.0に補間
            if (m_toolDataBase.toolDic[thisToolType].RecastTime > 0)
            {
                if (!isRare)
                    m_toolObjects[i].GetComponent<ToolFrame>()
                .GetRecastImage().fillAmount =
                m_player.GetComponent<PlayerAction>()
                .GetToolRecast(thisToolType) /
                m_toolDataBase.toolDic[thisToolType].RecastTime;
            }
            else
            {
                m_toolObjects[i].GetComponent<ToolFrame>()
                    .GetRecastImage().fillAmount = 0.0f;
            }
        }

        //  ツール選択状態参照
        m_toolObjects[centerNum].GetComponent<ToolFrame>().SetIsSelected(true);
        m_toolObjects[centerNum].transform.localScale = Vector3.one;
    }

    //  ツール作成可能数取得
    private int GetToolUseNum(ToolData.ToolType toolType)
    {
        //  可能数
        int num = int.MaxValue;

        //  アイテム数取得
        for (int i = 0; i < m_toolDataBase.toolDic[toolType].ItemMaterials.Length; i++)
        {
            //  アイテム
            ItemData.ItemType type = m_toolDataBase.toolDic[toolType].ItemMaterials[i].type;
            int count = m_toolDataBase.toolDic[toolType].ItemMaterials[i].count;

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
