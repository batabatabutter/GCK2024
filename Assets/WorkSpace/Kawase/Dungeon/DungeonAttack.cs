using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttack : MonoBehaviour
{
    //�v���C���[�̉��ɏo��n�C���C�g�̒Ⴓ
    const float HEIGLIGHT_HEIGHT = 0.5f;


    [Header("�v���C���[")]
    [SerializeField] GameObject target;
    [Header("����")]
    [SerializeField] GameObject rockfall;
    [Header("�n�C���C�g")]
    [SerializeField] GameObject highlight;
    [Header("�U���Ԋu")]
    [SerializeField] float attackCoolTime = 1.0f;
    [Header("���΂̐������鍂��")]
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
