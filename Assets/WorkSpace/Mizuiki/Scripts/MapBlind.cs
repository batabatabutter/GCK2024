using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlind : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        // �c�[���ɓ�������
        if (collision.transform.CompareTag("Tool"))
        {
            Destroy(gameObject);
        }
		
	}

}
