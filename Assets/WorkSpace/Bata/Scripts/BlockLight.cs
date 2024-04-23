using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : ObjectLight
{
    [SerializeField] private Block m_block;

    // Start is called before the first frame update
    private void Start()
    {
        if (m_block)
        {
            LightLevel = m_block.LightLevel;
            FlashLight(LightLevel);
        }
    }
}
