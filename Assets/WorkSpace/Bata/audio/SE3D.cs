using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SE3D : MonoBehaviour
{
    //  ‰¹
    [SerializeField] private AudioSource m_AudioSource;

    private void Start()
    {
        Call();
    }

    //  ‰¹‚ªÄ¶‚³‚ê‚Ä‚¢‚é‚©Šm”F
    private IEnumerator Checking(AudioSource audio, UnityAction callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audio.isPlaying)
            {
                callback();
                break;
            }
        }
    }

    void Call()
    {
        StartCoroutine(Checking(m_AudioSource, () => {
            Destroy(gameObject);
        }));
    }

}
