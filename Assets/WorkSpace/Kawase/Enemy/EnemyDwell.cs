using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// enemy��e�ɂ����h��^�̓G
/// </summary>
public class EnemyDwell : Enemy
{
    [SerializeField] GameObject m_dwellBlock;
    // �v���p�e�B
    public GameObject DwellBlock
    {
        get
        {
            return m_dwellBlock;
        }
        set
        {
            m_dwellBlock = value;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //�h��悪���񂾂玀��
        if (!m_dwellBlock)
        {
           base.Dead();
        }
    }

}
