using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttack : MonoBehaviour
{
    //プレイヤーの下に出るハイライトの低さ
    const float HEIGLIGHT_HEIGHT = 0.5f;


    [Header("プレイヤー")]
    [SerializeField] GameObject target;
    [Header("落石")]
    [SerializeField] GameObject rockfall;
    [Header("ハイライト")]
    [SerializeField] GameObject highlight;
    [Header("攻撃間隔")]
    [SerializeField] float attackCoolTime = 1.0f;
    [Header("落石の生成する高さ")]
    public float heightRock = 3.0f;

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
            Vector3 rockfallPos = new Vector3(target.transform.position.x,target.transform.position.y + heightRock, 0);
            Vector3 highlightPos = new Vector3(target.transform.position.x,target.transform.position.y - HEIGLIGHT_HEIGHT, 0);

            Instantiate(rockfall, rockfallPos, Quaternion.identity);
            Instantiate(highlight, highlightPos, Quaternion.identity);
            attackCoolTime = keepCoolTime;
        }
    }
}
