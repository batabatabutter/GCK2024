using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDwell : Enemy
{
    [SerializeField] GameObject m_dwellBlock;
    // プロパティ
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

        //宿り先が死んだら死ぬ
        if (!m_dwellBlock)
        {
           base.Dead();
        }
    }

}
