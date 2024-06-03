using System.Collections.Generic;
using System.Linq;
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
    private PlayerTool m_plTool;
    private PlayerItem m_plItem;

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
                totalWidth += size.x * m_graphScaleDeg + m_offset.x;
            else
                totalWidth += size.x + m_offset.x;

            //  追加
            m_toolObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  スライダー式
        //  ツールの数
        ToolData.ToolType playerToolType;
        int centerNum = m_toolObjects.Count / 2;

        //  ID
        List<ToolData.ToolType> useToolTypes = new List<ToolData.ToolType>(m_plTool.Tools.Keys);
        //int minID = 0;
        //int maxID = 0;
        //  レアツールなら
        if (m_plTool.IsRareTool)
        {
            //minID = (int)ToolData.ToolType.RARE + 1;
            //maxID = m_toolDataBase.toolRareData.Count + (int)ToolData.ToolType.RARE;
            playerToolType = m_plTool.ToolTypeRare;
        }
        else
        {
            //minID = 0;
            //maxID = m_toolDataBase.toolNormalData.Count - 1;
            playerToolType = m_plTool.ToolType;
        }

        //  ツールを番号に変換
        //int playerToolNum = (int)playerToolType;

        //  現在の番号取得
        int nowNum = 0;
        for (int i = 0; i < useToolTypes.Count; i++) 
        {
            if (playerToolType == useToolTypes[i])
                nowNum = i;
        }


        //  デバッグ
        if (m_debug) Debug.Log("現在のツール:" + playerToolType);

        for (int i = 0; i < m_toolObjects.Count; i++)
        {
            //  フレーム
            var toolFrame = m_toolObjects[i].GetComponent<ToolFrame>();

            ////  対応ツール
            //int thisToolID = playerToolNum - (centerNum - i);
            ////  オーバーしてたら修正
            //if (thisToolID < minID) thisToolID = (maxID + 1) - minID + thisToolID;
            //else if (thisToolID > maxID) thisToolID = thisToolID - (maxID + 1) + minID;
            ////  タイプに変換
            //ToolData.ToolType thisToolType = (ToolData.ToolType)thisToolID;

            //  対応ツール
            int thisToolID = nowNum - (centerNum - i);
            //  オーバーしてたら修正
            if (thisToolID < 0) 
                thisToolID = useToolTypes.Count + thisToolID;
            else if (thisToolID >= useToolTypes.Count) 
                thisToolID = thisToolID - useToolTypes.Count;
            //  タイプに変換
            ToolData.ToolType thisToolType = useToolTypes[thisToolID];


            //  大きさ変更
            m_toolObjects[i].transform.localScale = Vector3.one * m_graphScaleDeg;

            //  ツール数設定
            toolFrame.SetIsSelected(false);
            //  ツール画像設定
            toolFrame.SetImage(m_toolDataBase.toolDic[thisToolType].Icon);
            //  ツール作成可能数設定
            toolFrame.SetNum(GetToolUseNum(thisToolType));

            //  クールタイムがあるなら0.0～1.0に補間
            if (m_toolDataBase.toolDic[thisToolType].RecastTime > 0)
            {
                toolFrame.GetRecastImage().fillAmount =
                    m_plTool.RecastTime(thisToolType) / 
                    m_toolDataBase.toolDic[thisToolType].RecastTime;
            }
            else
            {
                toolFrame.GetRecastImage().fillAmount = 0.0f;
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
            ItemData.ItemType type = m_toolDataBase.toolDic[toolType].ItemMaterials[i].Type;
            int count = m_toolDataBase.toolDic[toolType].ItemMaterials[i].count;

            //  所持アイテム数から作成可能数を割り出す
            num = Mathf.Min(num, m_plItem.Items[type] / count);
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
            m_plTool = m_player.GetComponent<PlayerTool>();
            m_plItem = m_player.transform.Find("Item").gameObject.GetComponent<PlayerItem>();
        }
        //  プレイヤーが見つからなかったらデバッグ状態に
        if (m_player == null) m_debug = true;
    }
}
