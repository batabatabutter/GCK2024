using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("体力")]
    [SerializeField] private float m_health = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ダメージ加算
    public void AddDamage(float damage)
    {
        m_health -= damage;
    }

}
