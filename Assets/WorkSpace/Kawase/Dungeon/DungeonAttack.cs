using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("発生コウゲキON:OFF（これデバッグ用）")]
    [SerializeField] bool isfall;
    [SerializeField] bool isroll;
    [SerializeField] bool isbank;

    [Header("攻撃間隔")]
    [SerializeField] float attackCoolTime = 1.0f;


    [Header("--------------------------------------------")]

    [Header("シーンマネージャー")]
    [SerializeField] PlaySceneManager SceneManager;
    [Header("--------------------------------------------")]

    [Header("落石")]
    [SerializeField] GameObject rockfall;
    [Header("ハイライト")]
    [SerializeField] GameObject highlight;
    [Header("落石の生成する高さ")]
    public float heightRock = 3.0f;
    [Header("--------------------------------------------")]
    [Header("転がる岩")]
    [SerializeField] GameObject rollRock;
    [Header("矢印のハイライト")]
    [SerializeField] GameObject highlightArrow;
    [Header("矢印のハイライトが出現する距離")]
    [Header("上から")]
    [SerializeField] float up;
    [Header("右から")]
    [SerializeField] float right;
    [Header("下から")]
    [SerializeField] float down;
    [Header("左から")]
    [SerializeField] float left;
    [Header("--------------------------------------------")]
    [Header("土手")]
    [SerializeField] GameObject bank;
    [Header("土手ハイライト")]
    [SerializeField] GameObject bankHighlight;

    [Header("coreの攻撃間隔が２倍になる距離")]
    [SerializeField] float twiceAttackLength = 20;


    float keepCoolTime;

    GameObject target;
    GameObject core;

    // Start is called before the first frame update
    void Start()
    {
        target = SceneManager.GetPlayer();
        core = SceneManager.GetCore();
        keepCoolTime = attackCoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        int ratio = 1;

        //coreとプレイヤーが近いとコウゲキが2倍
        if(core != null) 
        {
            if (Vector2.Distance(core.transform.position, target.transform.position) < twiceAttackLength)
            {
                ratio = 2;
            }
        }



        //クールタイム減少
        attackCoolTime -= Time.deltaTime * ratio;

        if (attackCoolTime < 0.0f)
        {
            //コウゲキしたかどうか
            bool checkAttacked = false;
            //無限ループよけ
            int outNum = 100;
            //コウゲキしなかった場合再抽選
            while (!checkAttacked)
            {
                //マジックナンバーで行かして頂きまs
                int r = Random.Range(0, 3);

                if (isfall && r == 0)
                {
                    FallrockAttack();
                    checkAttacked = true;
                }

                if (isroll && r == 1)
                {
                    RockRollingAttack();
                    checkAttacked = true;
                }
                if (isbank && r == 2)
                {
                    BankAttack();
                    checkAttacked = true;
                }

                outNum--;
                //全てのコウゲキがoffだとここにはいるかな
                if (outNum < 0)
                    break;
            }
            //クールタイムリセット
            attackCoolTime = keepCoolTime;
        }
    }

    private void FallrockAttack()
    {
        Vector3 rockfallPos = new Vector3(target.transform.position.x, target.transform.position.y + heightRock, 0);
        Vector3 highlightPos = new Vector3(target.transform.position.x, target.transform.position.y - HEIGLIGHT_HEIGHT, 0);

        Instantiate(rockfall, rockfallPos, Quaternion.identity);
        Instantiate(highlight, highlightPos, Quaternion.identity);
    }

    private void RockRollingAttack()
    {
        Vector3 rollingPos;

        float rollingRotation;

        //4通りだから０～３
        int rand = Random.Range(0, 4);

        if(rand == (int)Direction.UP)
        {
            rollingPos = new Vector3(target.transform.position.x,target.transform.position.y + up, 0);

            rollingRotation = 180;

        }
        else if(rand == (int)Direction.RIGHT)
        {
            rollingPos = new Vector3(target.transform.position.x + right, target.transform.position.y, 0);
            rollingRotation = 90;

        }
        else if (rand == (int)Direction.DWON)
        {
            rollingPos = new Vector3(target.transform.position.x, target.transform.position.y - down, 0);
            rollingRotation = 0;

        }
        else
        {
            rollingPos = new Vector3(target.transform.position.x - left, target.transform.position.y, 0);
            rollingRotation = 270;

        }


        Instantiate(highlightArrow, rollingPos, Quaternion.Euler(0, 0, rollingRotation));
        Instantiate(rollRock, rollingPos, Quaternion.Euler(0, 0, rollingRotation));

    }
    private void BankAttack()
    {
        Vector3 pos = target.transform.position;

        pos.x -= 1;
        pos.y += 1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 1)
                    continue;
                    

                Instantiate(bank, new Vector3(pos.x + j,pos.y - i,0), Quaternion.identity);
                Instantiate(bankHighlight, new Vector3(pos.x + j, pos.y - i, 0), Quaternion.identity);
            }

        }
    }
}
