using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDamageDisplay : MonoBehaviour
{
    [Header("�\������")]
    [SerializeField] private float m_displayTime;
    private float m_time = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ԃ̉��Z
        m_time += Time.deltaTime;
        
        // ��莞�Ԍo�߂��Ă���Ύ��g������
        if (m_time >= m_displayTime)
        {
            Destroy(gameObject);
        }

    }
}
