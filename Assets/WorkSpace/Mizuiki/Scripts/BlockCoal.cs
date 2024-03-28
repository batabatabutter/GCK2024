using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoal : Block
{
	public override void DropItem(int count = 1)
	{
        int random = Random.Range(0, 1);

        if (random == 0)
        {
            base.DropItem(1);
        }
        else if (random == 1)
        {
            base.DropItem(2);
        }
	}

}
