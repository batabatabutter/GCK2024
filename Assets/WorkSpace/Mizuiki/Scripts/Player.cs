using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("���C�t")]
    [SerializeField] private int m_life = 5;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �_���[�W
    public void AddDamage(int damage)
    {
        m_life -= damage;
    }




    public int HitPoint
    {
        get { return m_life; }
    }
}