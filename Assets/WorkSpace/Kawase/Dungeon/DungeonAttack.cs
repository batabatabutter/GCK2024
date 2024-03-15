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
    [SerializeField] bool isbank;

    [Header("�U���Ԋu")]
    [SerializeField] float attackCoolTime = 1.0f;


    [Header("--------------------------------------------")]

    [Header("�V�[���}�l�[�W���[")]
    [SerializeField] PlaySceneManager SceneManager;
    [Header("--------------------------------------------")]

    [Header("����")]
    [SerializeField] GameObject rockfall;
    [Header("�n�C���C�g")]
    [SerializeField] GameObject highlight;
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
    [Header("--------------------------------------------")]
    [Header("�y��")]
    [SerializeField] GameObject bank;
    [Header("�y��n�C���C�g")]
    [SerializeField] GameObject bankHighlight;

    [Header("core�̍U���Ԋu���Q�{�ɂȂ鋗��")]
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

        //core�ƃv���C���[���߂��ƃR�E�Q�L��2�{
        if(core != null) 
        {
            if (Vector2.Distance(core.transform.position, target.transform.position) < twiceAttackLength)
            {
                ratio = 2;
            }
        }



        //�N�[���^�C������
        attackCoolTime -= Time.deltaTime * ratio;

        if (attackCoolTime < 0.0f)
        {
            //�R�E�Q�L�������ǂ���
            bool checkAttacked = false;
            //�������[�v�悯
            int outNum = 100;
            //�R�E�Q�L���Ȃ������ꍇ�Ē��I
            while (!checkAttacked)
            {
                //�}�W�b�N�i���o�[�ōs�����Ē�����s
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
                //�S�ẴR�E�Q�L��off���Ƃ����ɂ͂��邩��
                if (outNum < 0)
                    break;
            }
            //�N�[���^�C�����Z�b�g
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
