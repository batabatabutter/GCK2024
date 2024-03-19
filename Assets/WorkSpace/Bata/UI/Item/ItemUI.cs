using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ItemUI : MonoBehaviour
{
    //  シーンマネージャー
    [Header("プレイシーンマネージャー")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    //  プレイヤー
    private GameObject m_player;

    //  アイテムUIのプレハブ
    [Header("アイテムUIのプレハブ")]
    [SerializeField] private List<Sprite> m_itemGraph;
    [SerializeField] private GameObject m_itemFrame;
    [SerializeField] private Vector2 m_hpOffset;

    //  HP格納
    private List<GameObject> m_itemObjects = new List<GameObject>();

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
        //  プレイシーンマネージャーが無かったら格納しない
        if (m_playSceneManager == null)
            Debug.Log("Error:Playerの格納に失敗 PlaySceneManagerが見つかりません:DungeonManager");
        else
        {
            //  プレイヤー格納
            m_player = m_playSceneManager.GetPlayer();
        }
        //  プレイヤーが見つからなかったらデバッグ状態に
        if (m_player == null) m_debug = true;

        //  UI生成
        //  生成位置
        Vector2 size = m_itemFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < (int)Item.Type.OVER; i++)
        {
            //  座標
            pos = new Vector3(0.0f, -(size.y + m_hpOffset.y) * i) + transform.position;
            //  UI生成
            GameObject frame = Instantiate(m_itemFrame, pos, Quaternion.identity, transform);
            //  画像設定
            frame.GetComponent<ItemFrame>().SetImage(m_itemGraph[i]);

            m_itemObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  アイテム数更新
        for(int i = 0; i < (int)Item.Type.OVER; i++) 
        {
            //  アイテム数設定
            m_itemObjects[i].GetComponent<ItemFrame>().SetNum(
                m_player.GetComponent<PlayerItem>().Items[(Item.Type)i]);
        }
    }
}
