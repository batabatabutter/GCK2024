using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    float x;
    float y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Mathf.Sin( Time.time ) * 5;
        y = Mathf.Cos( Time.time ) * 1;


        transform.position = new Vector3(x, 0.0f,0.0f);
    }
}
