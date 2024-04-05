using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// enemy‚ğe‚É‚µ‚½h‚èŒ^‚Ì“G
/// </summary>
public class EnemyDwell : Enemy
{
    [SerializeField] GameObject m_dwellBlock;
    // ƒvƒƒpƒeƒB
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

        //h‚èæ‚ª€‚ñ‚¾‚ç€‚Ê
        if (!m_dwellBlock)
        {
           base.Dead();
        }
    }

}
