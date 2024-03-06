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

    [Header("--------------------------------------------")]

    [Header("プレイヤー")]
    [SerializeField] GameObject target;
    [Header("--------------------------------------------")]

    [Header("落石")]
    [SerializeField] GameObject rockfall;
    [Header("ハイライト")]
    [SerializeField] GameObject highlight;
    [Header("攻撃間隔")]
    [SerializeField] float attackCoolTime = 1.0f;
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


    float keepCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        keepCoolTime = attackCoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        attackCoolTime -= Time.deltaTime;
        if (attackCoolTime < 0.0f)
        {
            if(isfall)
                FallrockAttack();
            if(isroll)
                RockRollingAttack();

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
}
