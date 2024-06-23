using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    //  HPUIのプレハブ
    [Header("HPUIのプレハブ")]
    [SerializeField] private GameObject m_hpGauge;
    [SerializeField] private GameObject m_hpGaugeFrame;
    [SerializeField] private GameObject m_armorGauge;
    [Header("値")]
    [SerializeField] private Vector2 m_hpOffset;
    [SerializeField] private Vector2 m_armorOutlineWidth;
    [SerializeField] private UnityEngine.Color m_hpColor;
    [SerializeField] private UnityEngine.Color m_emptyColor;

    //  シーンマネージャー
    private PlaySceneManager m_playSceneManager;
    //  プレイヤー
    private Player m_player;

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;
    public int m_debugMaxHP;
    public int m_debugNowHP;

    //  HP格納
    private List<GameObject> m_hpGaugeObjects = new List<GameObject>();
    //  アーマー格納
    private GameObject m_armorGaugeObject = null;

    // Start is called before the first frame update
    void Start()
    {
        //  プレイシーンマネージャー設定
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        //  攻撃
        int val = 0;
        if (m_debug) val = m_debugMaxHP;
        else val = m_player.MaxLife;

        //  UI生成
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        //  アーマー用UI生成
        //  座標
        pos = new Vector3((size.x + m_hpOffset.x) * ((val - 1) / 2.0f), 0.0f) + transform.position;
        m_armorGaugeObject = Instantiate(m_armorGauge, pos, Quaternion.identity, transform);
        m_armorGaugeObject.SetActive(false);
        //  生成位置
        for (int i = 0; i < val; i++)
        {
            //  座標
            pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
            //  UI生成
            GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
            frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
            m_hpGaugeObjects.Add(frame);
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
        int armorVal = 0;
        hpVal = m_player.HitPoint;
        maxHpVal = m_player.MaxLife;
        armorVal = m_player.Armor;
        
        //  UI変化
        //  生成位置
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < maxHpVal; i++)
        {
            //  最大HPとアーマーよりUIが少なかったら生成
            if (i >= m_hpGaugeObjects.Count)
            {
                //  座標
                pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
                //  UI生成
                GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
                frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
                m_hpGaugeObjects.Add(frame);
            }

            ////  アーマー分は表示しないように
            //if (i >= maxHpVal + armorVal)
            //    m_hpGaugeObject[i].transform.parent.gameObject.SetActive(false);
            //else m_hpGaugeObject[i].transform.parent.gameObject.SetActive(true);

            //  HPがあれば色付き
            if (i < hpVal) m_hpGaugeObjects[i].GetComponent<RawImage>().color = m_hpColor;
            //else if(i >= maxHpVal && i < maxHpVal + armorVal) m_hpGaugeObject[i].GetComponent<RawImage>().color = m_armorColor;
            else m_hpGaugeObjects[i].GetComponent<RawImage>().color = m_emptyColor;
        }

        //  アーマーがあれば
        if (armorVal > 0)
        {
            //  表示
            m_armorGaugeObject.SetActive(true);

            //  生成位置
            pos = new Vector3((size.x + m_hpOffset.x) * ((maxHpVal - 1) / 2.0f), 0.0f) + transform.position;

            //  大きさ
            size = new Vector2(
                size.x * maxHpVal + m_hpOffset.x * (maxHpVal - 1) + m_armorOutlineWidth.x,
                size.y + m_armorOutlineWidth.y);

            //  代入
            m_armorGaugeObject.transform.position = pos;
            m_armorGaugeObject.GetComponent<RectTransform>().sizeDelta = size;
        }
        else m_armorGaugeObject.SetActive(false);

        //  デバッグ状態
        if (m_debug)
        {
            //  体力増減
            if (Input.GetKeyDown(KeyCode.Q)) m_player.AddDamage(1);
            if (Input.GetKeyDown(KeyCode.W)) m_player.AddDamage(-1);
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
            m_player = m_playSceneManager.Player;
        }
        //  プレイヤーが見つからなかったらデバッグ状態に
        if (m_player == null) m_debug = true;
    }
}
