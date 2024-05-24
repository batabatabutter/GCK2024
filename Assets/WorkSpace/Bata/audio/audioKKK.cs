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
            AudioManager.Instance.PlaySE(audioClip);
        }
        if(Input.GetKeyUp(KeyCode.P))
        {
            AudioManager.Instance.PlaySE(audioClip, pos);
        }
    }
}
