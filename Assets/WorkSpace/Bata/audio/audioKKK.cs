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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ID����Đ�
            AudioManager.Instance.PlaySE(audioDataID);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //  ID����Đ�+�ʒu�w��
            AudioManager.Instance.PlaySE(audioDataID, pos);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //  �N���b�v����Đ�
            AudioManager.Instance.PlaySE(audioClip);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            //  �N���b�v����Đ�+�ʒu�w��
            AudioManager.Instance.PlaySE(audioClip, pos);
        }
    }
}
