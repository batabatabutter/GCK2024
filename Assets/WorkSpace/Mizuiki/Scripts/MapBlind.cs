using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlind : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
        // �����ɓ�������
        if (collision.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            Destroy(gameObject);
        }
		
	}

}
