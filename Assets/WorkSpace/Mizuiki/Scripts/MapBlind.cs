using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlind : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
        // ツールに当たった
        if (collision.transform.CompareTag("Tool"))
        {
            Destroy(gameObject);
        }
		
	}

}
