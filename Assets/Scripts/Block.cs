using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("�u���b�N�̑ϋv")]
    [SerializeField] private float m_blockEndurance;

    [Header("�j��s��")]
    [SerializeField] private bool m_dontBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �̌@�_���[�W
    public void AddMiningDamage(float power)
    {
        // �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
        if (m_dontBroken)
            return;

        // �̌@�_���[�W���Z
        m_blockEndurance -= power;

        // �ϋv��0�ɂȂ���
        if (m_blockEndurance <= 0.0f)
        {
            BrokenBlock();
        }
    }

    // �u���b�N����ꂽ
    private void BrokenBlock()
    {
        // �j��s�\�u���b�N�̏ꍇ�͏������Ȃ�
        if (m_dontBroken)
            return;

        // �A�C�e���h���b�v


        // ���g���폜
        Destroy(gameObject);

    }

    
    public bool DontBroken
    {
        get { return m_dontBroken; }
        set { m_dontBroken = value; }
    }

}
