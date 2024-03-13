using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDamageDisplay : MonoBehaviour
{
    [Header("•\Ž¦ŽžŠÔ")]
    [SerializeField] private float m_displayTime;
    private float m_time = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ŽžŠÔ‚Ì‰ÁŽZ
        m_time += Time.deltaTime;
        
        // ˆê’èŽžŠÔŒo‰ß‚µ‚Ä‚¢‚ê‚ÎŽ©g‚ðÁ‚·
        if (m_time >= m_displayTime)
        {
            Destroy(gameObject);
        }

    }
}
