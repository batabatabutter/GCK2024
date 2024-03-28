using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class HPUI : MonoBehaviour
{
    //  HPUIのプレハブ
    [Header("HPUIのプレハブ")]
    [SerializeField] private GameObject m_hpGauge;
    [SerializeField] private GameObject m_hpGaugeFrame;
    [SerializeField] private Vector2 m_hpOffset;
    [SerializeField] private UnityEngine.Color m_hpColor;
    [SerializeField] private UnityEngine.Color m_armorColor;
    [SerializeField] private UnityEngine.Color m_emptyColor;

    //  シーンマネージャー
    private PlaySceneManager m_playSceneManager;
    //  プレイヤー
    private GameObject m_player;

    //  デバッグ用
    [Header("デバッグ用")]
    public bool m_debug = false;
    public int m_debugMaxHP;
    public int m_debugNowHP;

    //  HP格納
    private List<GameObject> m_hpGaugeObject = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //  プレイシーンマネージャー設定
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        //  攻撃
        int val = 0;
        if (m_debug) val = m_debugMaxHP;
        else val = m_player.GetComponent<Player>().MaxLife;

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
        int armorVal = 0;
        hpVal = m_player.GetComponent<Player>().HitPoint;
        maxHpVal = m_player.GetComponent<Player>().MaxLife;
        armorVal = m_player.GetComponent<Player>().Armor;
        
        //  UI変化
        //  生成位置
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < maxHpVal + armorVal; i++)
        {
            //  最大HPとアーマーよりUIが少なかったら生成
            if (i >= m_hpGaugeObject.Count)
            {
                //  座標
                pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
                //  UI生成
                GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
                frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
                m_hpGaugeObject.Add(frame);
            }

            //  HPがあれば色付き
            if (i < hpVal) m_hpGaugeObject[i].GetComponent<RawImage>().color = m_hpColor;
            else if(i >= maxHpVal && i < maxHpVal + armorVal) m_hpGaugeObject[i].GetComponent<RawImage>().color = m_armorColor;
            else m_hpGaugeObject[i].GetComponent<RawImage>().color = m_emptyColor;
        }

        //  アーマー

        //  デバッグ状態
        if (m_debug)
        {
            //  体力増減
            if (Input.GetKeyDown(KeyCode.Q)) m_player.GetComponent<Player>().AddDamage(1);
            if (Input.GetKeyDown(KeyCode.W)) m_player.GetComponent<Player>().AddDamage(-1);
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
