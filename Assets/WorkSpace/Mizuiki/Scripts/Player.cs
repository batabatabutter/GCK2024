using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("���C�t")]
    [SerializeField] private int m_life = 5;

    [Header("�ő僉�C�t")]
    [SerializeField] private int m_maxLife = 10;

    [Header("�A�[�}�[�̐�")]
    [SerializeField] private int m_armor = 0;


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
        // �A�[�}�[������
        if (m_armor > 0)
        {
            // �A�[�}�[��1���
            m_armor--;
            return;
        }

        m_life -= damage;
    }



    // ���C�t
    public int HitPoint
    {
        get { return m_life; }
    }
    // �ő僉�C�t
    public int MaxLife
    {
        get { return m_maxLife; }
    }
    // �A�[�}�[�̖���
    public int ARMOR
    {
        get { return m_armor; }
        set { m_armor = value; }
    }

}
