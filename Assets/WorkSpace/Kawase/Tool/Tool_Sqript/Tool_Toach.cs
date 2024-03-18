using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Tool_Toach : Tool
{
    [Header("置かれてからブロックするまでの時間")]
    [SerializeField] float time;
    public override void Initialize()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Block>().enabled = false;

    }
    public override void ToolUpdate()
    {

        if (time < 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<Block>().enabled = true;


        }
        else
        {
            time -= Time.deltaTime;
        }
    }


}
