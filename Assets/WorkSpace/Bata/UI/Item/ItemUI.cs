using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    //  アイテムUIのプレハブ
    [Header("アイテムUIのプレハブ")]
    [SerializeField] private GameObject m_itemFrame;
    [SerializeField] private Vector2 m_offset;

    //  アイテムデータベース
    [Header("アイテムのデータベース")]
    [SerializeField] private ItemDataBase m_data;

    //  HP格納
    private List<GameObject> m_itemObjects = new List<GameObject>();

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
        Vector2 size = m_itemFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < m_data.item.Count; i++)
        {
            //  座標
            pos = new Vector3(0.0f, -(size.y + m_offset.y) * i) + transform.position;
            //  UI生成
            GameObject frame = Instantiate(m_itemFrame, pos, Quaternion.identity, transform);
            //  画像設定
            frame.GetComponent<ItemFrame>().SetImage(m_data.item[i].Sprite);

            m_itemObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  アイテム数更新
        int graphNum = 0;
        for(int i = 0; i < m_data.item.Count; i++) 
        {
            //  アイテム数設定
            var num = m_player.transform.Find("Item").gameObject.GetComponent<PlayerItem>().Items[m_data.item[i].Type];
            if (num == 0)
            {
                m_itemObjects[i].SetActive(false);
            }
            else
            {
                m_itemObjects[i].SetActive(true);
                Vector2 size = m_itemFrame.GetComponent<RectTransform>().sizeDelta;
                Vector3 pos = new Vector3(0.0f, -(size.y + m_offset.y) * graphNum) + transform.position;
                m_itemObjects[i].GetComponent<ItemFrame>().SetNum(num);
                m_itemObjects[i].transform.position = pos;
                graphNum++;
            }
        }
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
