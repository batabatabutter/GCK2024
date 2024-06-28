using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : ObjectLight
{
    // Start is called before the first frame update
    private void Start()
    {
        //  �擾
        if (transform.parent.TryGetComponent<ObjectAffectLight>(out ObjectAffectLight affectLight))
        {
            LightLevel = affectLight.LightLevel;
            FlashLight(LightLevel);
        }
    }
}