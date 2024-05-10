using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessParent : MonoBehaviour
{
    //  Õ“Ë
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < collision.transform.childCount; i++)
        {
            //  On
            collision.GetComponent<ProcessChild>().Change(true);
        }
    }

    //  —£‚ê
    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < collision.transform.childCount; i++)
        {
            //  Off
            collision.GetComponent<ProcessChild>().Change(false);
        }
    }
}