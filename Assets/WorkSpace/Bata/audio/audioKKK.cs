using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioKKK : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] Vector2 pos;
    [SerializeField] AudioDataID audioDataID;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            // ID����Đ�
            AudioManager.Instance.PlaySE(audioDataID);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            //  ID����Đ�+�ʒu�w��
            AudioManager.Instance.PlaySE(audioDataID, pos);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            //  �N���b�v����Đ�
            AudioManager.Instance.PlaySE(audioClip);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            //  �N���b�v����Đ�+�ʒu�w��
            AudioManager.Instance.PlaySE(audioClip, pos);
        }
    }
}
