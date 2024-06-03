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
            // IDから再生
            AudioManager.Instance.PlaySE(audioDataID);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //  IDから再生+位置指定
            AudioManager.Instance.PlaySE(audioDataID, pos);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //  クリップから再生
            AudioManager.Instance.PlaySE(audioClip);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            //  クリップから再生+位置指定
            AudioManager.Instance.PlaySE(audioClip, pos);
        }
    }
}
