using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioKKK : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] Vector2 pos;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            // IDから再生
            AudioManager.Instance.PlaySE(AudioClipID.Dig);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            //  IDから再生+位置指定
            AudioManager.Instance.PlaySE(AudioClipID.Put, pos);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            //  クリップから再生
            AudioManager.Instance.PlaySE(audioClip);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            //  クリップから再生+位置指定
            AudioManager.Instance.PlaySE(audioClip, pos);
        }
    }
}
