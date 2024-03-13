using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    //  シーンマネージャー
    [Header("プレイシーンマネージャー")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    //  プレイヤー
    private GameObject m_player;

    //  HPUIのプレハブ
    [Header("HPUIのプレハブ")]
    [SerializeField] private GameObject m_hpGauge;
    [SerializeField] private GameObject m_hpGaugeFrame;
    [SerializeField] private Vector2 m_hpOffset;

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;
    public int m_maxHP;
    public int m_nowHP;

    //  HP格納
    private List<GameObject> m_hpGaugeObject = new List<GameObject>();

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

        //  攻撃
        int val = 0;
        if (m_debug) val = m_maxHP;
        else val = m_player.GetHashCode();

        //  UI生成
        //  生成位置
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < val; i++)
        {
            //  座標
            pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
            //  UI生成
            GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
            frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
            m_hpGaugeObject.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  プレイヤーが見つからなかったらデバッグ状態に
        if (m_player == null) m_debug = true;

        //  HP状態を参照しゲージ変化
        int hpVal = 0;
        int maxHpVal = 0;
        if (m_debug)
        {
            hpVal = m_nowHP;
            maxHpVal = m_maxHP;
        }
        else
        {
            hpVal = m_player.GetHashCode();
            maxHpVal = m_player.GetHashCode();
        }
        //  UI変化
        for (int i = 0; i < maxHpVal; i++)
        {
            //  体力に応じてUI表示
            if (i < hpVal) m_hpGaugeObject[i].SetActive(true);
            else m_hpGaugeObject[i].SetActive(false);
        }
        
        //  デバッグ状態
        if (m_debug)
        {
            Debug.Log("HP:" + hpVal);

            //  体力増減
            if (Input.GetKeyDown(KeyCode.Q)) m_nowHP = Mathf.Max(m_nowHP - 1, 0);
            if (Input.GetKeyDown(KeyCode.W)) m_nowHP = Mathf.Min(m_nowHP + 1, m_maxHP);
        }

    }
}
