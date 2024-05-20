using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ChangeBrightness : MonoBehaviour
{
    //���邳�̍ő�l
    private static readonly int MAX_BRIGHTNESS = 7;
    public int GetMAXBRIGHTBESS() { return MAX_BRIGHTNESS; }

    //LightList
    public HashSet<ObjectLight> m_lights = new HashSet<ObjectLight>();

    //  �v���C���[�̍��W
    private Transform m_playerTr;

    [Header("�u���b�N")]
    [SerializeField] private ObjectAffectLight m_affectLight;

    // Start is called before the first frame update
    void Start()
    {
        //��������
        ChangeBlack();

        //  �t���O�I�t�Ȃ珈�����Ȃ�
        if (m_affectLight)
            if (m_affectLight.BrightnessFlag == false)
            {
                ChangeWhite();
                Destroy(gameObject);
            }
    }

    // Update is called once per frame
    void Update()
    {
        //  ���̋�����MAX�Ȃ珈�����Ȃ�
        if (m_affectLight.LightLevel >= MAX_BRIGHTNESS)
        {
            m_affectLight.ReceiveLightLevel = MAX_BRIGHTNESS;
            return;
        }

        List<ObjectLight> removeList = new List<ObjectLight>();
        foreach (var light in m_lights)
        {
            if (light == null || light.IsDestroyed())
                removeList.Add(light);
        }
        foreach (var light in removeList)
        {
            m_lights.Remove(light);
        }
        removeList.Clear();

        ChangeColor();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //  �����ő�ȏ�Ȃ珈�����Ȃ�
        if (m_affectLight.LightLevel >= MAX_BRIGHTNESS) return;

        //  ���C�g�R���|
        var li = collision.gameObject.GetComponent<ObjectLight>();

        //  ���C�g�̃��x���� 0 �ȉ��������̌������x���ȉ�
        if (li.LightLevel <= 0 || li.LightLevel <= m_affectLight.LightLevel) return;

        // �����̒ǉ�
        if (!m_lights.Contains(li))
        {
            m_lights.Add(li);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //  �����ő�ȏ�Ȃ珈�����Ȃ�
        if (m_affectLight.LightLevel >= MAX_BRIGHTNESS) return;

        // �����������̂����C�g�ł͂Ȃ�
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;

        //  ���C�g�R���|
        var li = collision.gameObject.GetComponent<ObjectLight>();
        m_lights.Remove(li);
    }

    private void ChangeColor()
    {
        // �������x���̌v�Z
        int receiveLightLv = 0;
        if (m_affectLight) receiveLightLv = m_affectLight.LightLevel;

        foreach (var light in m_lights)
        {
            // ���邳�ő�Ȃ珈���I��
            if (receiveLightLv >= MAX_BRIGHTNESS)
                break;

            // null�Ȃ�X�L�b�v
            if (light == null)
                continue;

            // �󂯂����x���������ȉ��Ȃ�X�L�b�v
            if (receiveLightLv >= light.LightLevel)
                continue;

            // ���������v�Z
            float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(light.transform.position), transform.position));

            receiveLightLv = Mathf.Max(receiveLightLv, light.LightLevel - (int)lightLength);
        }

        // �u���b�N�����݂���Ȃ�
        if (m_affectLight)
            if (!m_affectLight.IsDestroyed())
            {
                m_affectLight.ReceiveLightLevel = receiveLightLv;
            }
    }

    // ���g���Â�����
    private void ChangeBlack()
    {
        if (m_affectLight)
        {
            m_affectLight.ReceiveLightLevel = 0;
        }
    }

    private void ChangeWhite()
    {
        if (m_affectLight)
        {
            m_affectLight.ReceiveLightLevel = MAX_BRIGHTNESS;
        }
    }

    //// �����̃I�u�W�F�N�g�����ɂ���(Light�v���n�u)
    //bool CheckForObjectInList(GameObject obj)
    //{
    //    // ���X�g���̊e�I�u�W�F�N�g���`�F�b�N
    //    foreach (ObjectLight item in m_lightList)
    //    {
    //        if (item == null)
    //            continue;

    //        // �����I�u�W�F�N�g�����������ꍇ��true��Ԃ�
    //        if (item.gameObject == obj)
    //        {
    //            return true;
    //        }
    //    }
    //    // �����I�u�W�F�N�g��������Ȃ������ꍇ��false��Ԃ�
    //    return false;
    //}

    //  �v���C���[���W�n�ݒ�
    public void SetPlayerTransform(Transform tr) { m_playerTr = tr; }
    public Transform GetPlayerTransform() { return m_playerTr; }

    public /*Block*/ObjectAffectLight AffectLight
    {
        get { return m_affectLight; }
        set { m_affectLight = value; }
    }
}
