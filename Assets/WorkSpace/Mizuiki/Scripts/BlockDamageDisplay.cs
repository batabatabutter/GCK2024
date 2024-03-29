using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDamageDisplay : MonoBehaviour
{
    [Header("表示時間")]
    [SerializeField] private float m_displayTime;
    private float m_time = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 時間の加算
        m_time += Time.deltaTime;
        
        // 一定時間経過していれば自身を消す
        if (m_time >= m_displayTime)
        {
            Destroy(gameObject);
        }

    }
}
