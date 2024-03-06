using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttack : MonoBehaviour
{
    //�v���C���[�̉��ɏo��n�C���C�g�̒Ⴓ
    const float HEIGLIGHT_HEIGHT = 0.5f;

    enum Direction
    {
        UP,
        RIGHT,
        DWON,
        LEFT
    }

    [Header("�����R�E�Q�LON:OFF�i����f�o�b�O�p�j")]
    [SerializeField] bool isfall;
    [SerializeField] bool isroll;

    [Header("--------------------------------------------")]

    [Header("�v���C���[")]
    [SerializeField] GameObject target;
    [Header("--------------------------------------------")]

    [Header("����")]
    [SerializeField] GameObject rockfall;
    [Header("�n�C���C�g")]
    [SerializeField] GameObject highlight;
    [Header("�U���Ԋu")]
    [SerializeField] float attackCoolTime = 1.0f;
    [Header("���΂̐������鍂��")]
    public float heightRock = 3.0f;
    [Header("--------------------------------------------")]
    [Header("�]�����")]
    [SerializeField] GameObject rollRock;
    [Header("���̃n�C���C�g")]
    [SerializeField] GameObject highlightArrow;
    [Header("���̃n�C���C�g���o�����鋗��")]
    [Header("�ォ��")]
    [SerializeField] float up;
    [Header("�E����")]
    [SerializeField] float right;
    [Header("������")]
    [SerializeField] float down;
    [Header("������")]
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

        //4�ʂ肾����O�`�R
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
