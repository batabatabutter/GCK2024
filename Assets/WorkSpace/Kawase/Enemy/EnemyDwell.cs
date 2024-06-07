using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// enemy��e�ɂ����h��^�̓G
/// </summary>
public class EnemyDwell : Enemy
{
    GameObject m_dwellBlock;

    //  �����J�ڎ���
    public static readonly float FADE_TAKE_TIME = 1.0f;
    //  ��������
    private float m_fadeTime = 0.0f;

    // �v���p�e�B
    public GameObject DwellBlock
    {
        get
        {
            return m_dwellBlock;
        }
        set
        {
            m_dwellBlock = value;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //  �����x�ݒ�
        //m_colorAlpha = 0.0f;

        if (m_dwellBlock)
            if (m_dwellBlock.TryGetComponent(out ObjectAffectLight light))
            {
                BrightnessFlag = light.BrightnessFlag;
            }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //  �ŏ��̃X�[
        if (m_fadeTime < FADE_TAKE_TIME)
        {
            m_fadeTime += Time.deltaTime;
            //m_colorAlpha = m_fadeTime / FADE_TAKE_TIME;
            if (m_fadeTime >= FADE_TAKE_TIME)
            {
                //m_colorAlpha = 1.0f;
            }
        }

        //�h��悪���񂾂玀��
        if (!m_dwellBlock)
        {
            base.Dead();
            return;
        }

        //  ���邳�擾
        if (BrightnessFlag)
            ReceiveLightLevel = m_dwellBlock.GetComponent<ObjectAffectLight>().ReceiveLightLevel;

        if (base.Player != null)
        {
            RotationToPlayer();
        }    
    }

    protected void RotationToPlayer()
    {
        Transform target = base.Player.transform;
        // �^�[�Q�b�g�̕����x�N�g�����v�Z
        Vector3 direction = target.position - transform.position;

        // �����x�N�g���𐳋K��
        direction.Normalize();

        // �����x�N�g������p�x���v�Z
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �p�x���l�̌ܓ����āA0���A90���A180���A270���̂����ꂩ�Ɋۂ߂�
        angle = Mathf.Round(angle / 90) * 90 - 90;

        // �I�u�W�F�N�g����]
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
