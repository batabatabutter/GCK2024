using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�̗�")]
    [SerializeField] private float m_health = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �_���[�W���Z
    public void AddDamage(float damage)
    {
        m_health -= damage;
    }

}
