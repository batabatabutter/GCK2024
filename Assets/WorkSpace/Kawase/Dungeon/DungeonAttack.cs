using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DungeonAttack;
using static WaveManager;

public class DungeonAttack : MonoBehaviour
{
    //プレイヤーの下に出るハイライトの低さ
    const float HEIGLIGHT_HEIGHT = 0.5f;



    enum Direction
    {
        UP,
        RIGHT,
        DWON,
        LEFT
    }

    public enum AttackPattern
    {
        FallRock,
        RollRock,
        Bank,

        OverID

    }

    [Header("ダンジョンデータベース")]
    [SerializeField] DungeonDataBase m_dungeonDataBase;

    [Header("--------------------------------------------")]

    [Header("coreの攻撃してくる距離")]
    [SerializeField] float m_attackLength = 20;

    [Header("--------------------------------------------")]

    [Header("シーンマネージャー")]
    [SerializeField] PlaySceneManager m_sceneManager;
    [Header("--------------------------------------------")]

    [Header("落石")]
    [SerializeField] GameObject m_fallRock;
    [Header("ハイライト")]
    [SerializeField] GameObject m_fallRockHighlight;
    [Header("落石の生成する高さ")]
    public float heightRock = 3.0f;
    [Header("コウゲキ時間")]
    [SerializeField] float m_fallTime = 4.0f;

    [Header("--------------------------------------------")]
    [Header("転がる岩")]
    [SerializeField] GameObject m_rollRock;
    [Header("矢印のハイライト")]
    [SerializeField] GameObject m_rollRockhighlight;
    [Header("矢印のハイライトが出現する距離")]
    [Header("上から")]
    [SerializeField] float m_up;
    [Header("右から")]
    [SerializeField] float m_right;
    [Header("下から")]
    [SerializeField] float m_down;
    [Header("左から")]
    [SerializeField] float m_left;
    [Header("コウゲキ時間")]
    [SerializeField] float m_rollTime = 7.0f;

    [Header("--------------------------------------------")]
    [Header("土手")]
    [SerializeField] GameObject m_bank;
    [Header("土手ハイライト")]
    [SerializeField] GameObject m_bankHighlight;
    [Header("コウゲキ時間")]
    [SerializeField] float m_bankTime = 4.0f;

    //"攻撃間隔"
    float m_attackCoolTime;

    //クルータイム記憶
    float m_keepCoolTime;

    GameObject m_target;
    GameObject m_core;

    //攻撃の選択用
    bool m_isFall = false;
    bool m_isRoll = false;
    bool m_isBank = false;

    //ウェーブ
    int m_wave;

    //ウェーブマネージャー
    WaveManager m_waveManager;


    // Start is called before the first frame update
    void Start()
    {
        if (m_waveManager == null)
        {
            m_waveManager = GetComponent<WaveManager>();
        }
        //ウェーブ数の取得
        m_wave = m_waveManager.WaveNum;
        //タゲとコア
        m_target = m_sceneManager.GetPlayer();
        m_core = m_sceneManager.GetCore();
        //攻撃間隔
        m_keepCoolTime =
            m_dungeonDataBase.dungeonDatas[GetComponent<DungeonGenerator>().GetStageNum()].DungeonWaves[m_wave].geterateEnemyInterval;

        //タイマーセット
        m_attackCoolTime = m_keepCoolTime;

        //アタックパターンの取得
        List<AttackPattern> attackPatterns = m_dungeonDataBase.
            dungeonDatas[GetComponent<DungeonGenerator>().GetStageNum()].
            DungeonWaves[m_wave].dungeonATKPattern;

        //bool値のせってい
        SetAtkIs(attackPatterns);

    }

    // Update is called once per frame
    void Update()
    {
        //ウェーブ変化で内容更新
        if(m_wave != GetComponent<WaveManager>().WaveNum)
        {
            Start();
        }


        int ratio = 1;

        //coreとプレイヤーが近いとコウゲキが2倍
        if(m_core != null) 
        {
            if (Vector2.Distance(m_core.transform.position, m_target.transform.position) < m_attackLength)
            {
                ratio = 2;

            }
        }

        //アタック状態でダンジョン攻撃
        if (m_waveManager.waveState == WaveState.Attack)
        {        
            //クールタイム減少
            m_attackCoolTime -= Time.deltaTime * ratio;
        }

        if (m_attackCoolTime < 0.0f)
        {
            //コウゲキしたかどうか
            bool checkAttacked = false;
            //無限ループよけ
            int outNum = 10;

            //コウゲキに合わせた時間加算
            float attackAddTime = 0.0f;

            //コウゲキしなかった場合再抽選
            while (!checkAttacked)
            {

                //マジックナンバーで行かして頂きまs
                int r = Random.Range(0, 3);

                if (m_isFall && r == 0)
                {
                    FallrockAttack();
                    checkAttacked = true;

                    attackAddTime = m_fallTime;
                }

                if (m_isRoll && r == 1)
                {
                    RockRollingAttack();
                    checkAttacked = true;

                    attackAddTime = m_rollTime;

                }
                if (m_isBank && r == 2)
                {
                    BankAttack();
                    checkAttacked = true;
                    attackAddTime = m_bankTime;

                }

                outNum--;
                //全てのコウゲキがoffだとここにはいるかな
                if (outNum < 0)
                    break;
            }
            //クールタイムリセット
            m_attackCoolTime = m_keepCoolTime + attackAddTime;
        }
    }

    private void FallrockAttack()
    {
        Vector3 rockfallPos = new (m_target.transform.position.x, m_target.transform.position.y + heightRock, 0);
        Vector3 highlightPos = new (m_target.transform.position.x, m_target.transform.position.y - HEIGLIGHT_HEIGHT, 0);

        Instantiate(m_fallRock, rockfallPos, Quaternion.identity);
        Instantiate(m_fallRockHighlight, highlightPos, Quaternion.identity);
    }

    private void RockRollingAttack()
    {
        Vector3 rollingPos;

        float rollingRotation;

        //4通りだから０～３
        int rand = Random.Range(0, 4);

        if(rand == (int)Direction.UP)
        {
            rollingPos = new Vector3(m_target.transform.position.x,m_target.transform.position.y + m_up, 0);

            rollingRotation = 180;

        }
        else if(rand == (int)Direction.RIGHT)
        {
            rollingPos = new Vector3(m_target.transform.position.x + m_right, m_target.transform.position.y, 0);
            rollingRotation = 90;

        }
        else if (rand == (int)Direction.DWON)
        {
            rollingPos = new Vector3(m_target.transform.position.x, m_target.transform.position.y - m_down, 0);
            rollingRotation = 0;

        }
        else
        {
            rollingPos = new Vector3(m_target.transform.position.x - m_left, m_target.transform.position.y, 0);
            rollingRotation = 270;

        }


        Instantiate(m_rollRockhighlight, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
        Instantiate(m_rollRock, rollingPos, Quaternion.Euler(0, 0, rollingRotation));

    }
    private void BankAttack()
    {
        Vector3 pos = m_target.transform.position;

        pos.x -= 1;
        pos.y += 1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 1)
                    continue;
                    

                Instantiate(m_bank, new Vector3(pos.x + j,pos.y - i,0), Quaternion.identity);
                Instantiate(m_bankHighlight, new Vector3(pos.x + j, pos.y - i, 0), Quaternion.identity);
            }

        }
    }

    private void SetAtkIs(List<AttackPattern> attackPatterns)
    {
        m_isFall = false;
        m_isRoll = false;
        m_isBank = false;
        //攻撃の設定
        for (int i = 0; i < attackPatterns.Count; i++)
        {
            switch (attackPatterns[i])
            {
                case AttackPattern.FallRock:
                    m_isFall = true;
                    break;
                case AttackPattern.RollRock:
                    m_isRoll = true;
                    break;
                case AttackPattern.Bank:
                    m_isBank = true;
                    break;
                default:
                    break;
            }
        }

    }
}
