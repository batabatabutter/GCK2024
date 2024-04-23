using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlind : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
        // 光源に当たった
        if (collision.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            Destroy(gameObject);
        }
		
	}

}
