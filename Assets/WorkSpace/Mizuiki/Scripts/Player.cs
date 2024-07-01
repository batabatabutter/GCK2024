using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("���C�t")]
    [SerializeField] private int m_life = 5;

    [Header("�ő僉�C�t")]
    [SerializeField] private int m_maxLife = 10;

    [Header("�A�[�}�[�̐�")]
    [SerializeField] private int m_armor = 0;

    [Header("���G����")]
    [SerializeField] private float m_invincibleTime = 1.0f;
    private float m_invincible = 0.0f;

    //  �֐�
    private UnityEvent m_dmgEvents = new UnityEvent();


    // Update is called once per frame
    void Update()
    {
        if (m_invincible > 0.0f)
        {
			// ���G���Ԃ̌o��
			m_invincible -= Time.time;
        }        
    }

    // �_���[�W
    public void AddDamage(int damage)
    {
        // ���G���Ԓ�
        if (m_invincible > 0.0f)
            return;

        // �A�[�}�[������
        if (m_armor > 0)
        {
            // �A�[�}�[��1���
            m_armor--;
            if (m_armor <= 0) AudioManager.Instance.PlaySE(AudioDataID.BreakArmor);
            else AudioManager.Instance.PlaySE(AudioDataID.GetDamageArmor);
            return;
        }

        // �̗͌���
        m_life -= damage;
        if (damage > 0) m_dmgEvents.Invoke();
        if (m_life <= 0) AudioManager.Instance.PlaySE(AudioDataID.DeadPlayer);
        else AudioManager.Instance.PlaySE(AudioDataID.GetDamage);

        // ���G���Ԃ̐ݒ�
        m_invincible = m_invincibleTime;

    }

    // ��
    public void Healing(int val)
    {
        // �̗͉��Z
        m_life += val;

        // �ő僉�C�t�𒴂���
        if (m_life > m_maxLife)
        {
            m_life = m_maxLife;
        }
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
    public int Armor
    {
        get { return m_armor; }
        set { m_armor = value; }
    }

    public UnityEvent DmgEvents
    {
        get { return m_dmgEvents; }
    }

}
