using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessParent : MonoBehaviour
{
    //  �Փˎ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < collision.transform.childCount; i++)
        {
            //  On
            collision.GetComponent<ProcessChild>().Change(true);
        }
    }

    //  ���ꎞ
    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < collision.transform.childCount; i++)
        {
            //  Off
            collision.GetComponent<ProcessChild>().Change(false);
        }
    }
}