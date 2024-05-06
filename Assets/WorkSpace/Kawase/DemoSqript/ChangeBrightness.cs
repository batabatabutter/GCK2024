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
    //  ��������
    private static readonly float DISTANCE_LIGHT = 17.0f;

    //LightList
    public List<ObjectLight> m_lightList = new();
    public HashSet<ObjectLight> m_lights = new HashSet<ObjectLight>();
    //  ���݂̎󂯎�胉�C�g���x��
    private int m_nowLightLevel = 0;

    //  �v���C���[�̍��W
    private Transform m_playerTr;

    //  ����
    private ObjectLight m_objlight;
    //  �����Փ˔���
    private Collider[] m_lightColliders;
    //  �����Փ˔���t���O
    private bool m_lightColidersFlag = true;

	[Header("�u���b�N")]
	[SerializeField] private /*Block*/ObjectAffectLight m_affectLight;

	// Start is called before the first frame update
	void Start()
	{
		//��������
		ChangeBlack();

        //  �����擾
        if (gameObject.TryGetComponent(out ObjectLight objLight))
        {
            m_objlight = objLight;
            m_nowLightLevel = m_objlight.LightLevel;
            m_lightColliders = GetComponents<Collider>();
        }

        // ���C�g�̉e�����󂯂�I�u�W�F�N�g�̎擾
        if (gameObject.TryGetComponent(out ObjectAffectLight affectLight))
		{
			m_affectLight = affectLight;
		}
	}

	// Update is called once per frame
	void Update()
	{
        //  ���̋�����MAX�Ȃ珈�����Ȃ�
        if (m_objlight)
        {
            if (m_objlight.LightLevel >= MAX_BRIGHTNESS)
            {
                m_affectLight.ReceiveLightLevel = MAX_BRIGHTNESS;
                return;
            }
        }

        //  �������x��
        int lv = 0;
        if (m_objlight) lv = m_objlight.LightLevel;

        //  �v���C���[�Ƃ̋��������ȏ㗣��Ă����珈�����Ȃ�
        if (m_playerTr == null)
        {
            if (Time.frameCount % 60 == 0) // 60FPS�Œ�Ȃ�A
            {
                Debug.Log("Error:Block��Plyer���W�������ĂȂ��F" + this);
            }
        }
        else if (Vector2.Distance(transform.position, m_playerTr.position) > DISTANCE_LIGHT + lv)
        {
            //  ����������ꍇ�̒ǉ�����
            if (m_objlight && m_lightColidersFlag)
            {
                //  �Փ˔��������
                foreach (Collider col in m_lightColliders) { col.gameObject.SetActive(false); }
                m_lightColidersFlag = false;
            }
            return;
        }
        else
        {
            //  ����������ꍇ�̏���
            if (m_objlight && !m_lightColidersFlag)
            {
                //  �Փ˔��������
                foreach (Collider col in m_lightColliders) { col.gameObject.SetActive(true); }
                m_lightColidersFlag = true;
            }
        }

        ChangeColor();
    }

    void OnTriggerEnter2D(Collider2D collision)
	{
        //  �����ő�ȏ�Ȃ珈�����Ȃ�
        if (m_objlight)
            if (m_objlight.LightLevel >= MAX_BRIGHTNESS) return;

        // �����������̂����C�g�ł͂Ȃ�
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
			return;

        //  ���C�g�R���|
        var li = collision.gameObject.GetComponent<ObjectLight>();

        // ���C�g�̃��x���� 0 �ȉ�
        if (li.LightLevel <= 0)
			return;

        //  ���C�g�̃��x���������̌������x���ȉ�
        if (m_objlight)
            if (li.LightLevel <= m_objlight.LightLevel) return;

        // �����̒ǉ�
  //      if (!CheckForObjectInList(collision.gameObject))
		//{
  //          m_lightList.Add(li);
  //      }
        if (!m_lights.Contains(li))
        {
            m_lights.Add(li);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //  �����ő�ȏ�Ȃ珈�����Ȃ�
        if (m_objlight)
            if (m_objlight.LightLevel >= MAX_BRIGHTNESS) return;

        // �����������̂����C�g�ł͂Ȃ�
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;

        // ���C�g�̃��x���� 0 �ȉ�
        if (collision.gameObject.GetComponent<ObjectLight>().LightLevel <= 0)
            return;

        // HashSet�ɕϊ����ď���������B
        //var mainHashSet = new HashSet<ObjectLight>(m_lightList);
        //foreach (var light in m_lightList)
        //{
        //    if (light == null)
        //        mainHashSet.Remove(light);
        //}
        //m_lightList = mainHashSet.ToList();
        foreach (var light in m_lights)
        {
            if (light == null || light.IsDestroyed())
                m_lights.Remove(light);
        }

        ////���C�g���X�g�̊Ǘ�
        //for (int i = 0; i < m_lightList.Count; i++)
        //{
        //    //�����Ȃ��������
        //    if (m_lightList[i] == null)
        //    {
        //        RemoveLightList(i);
        //    }
        //    //���ꂽ�����(3�͓K���ɑ傫�߂ɂ���deleteLength�I��)
        //    else if (Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].gameObject.transform.position), gameObject.transform.position) > m_lightList[i].LightLevel + 3)
        //    {
        //        RemoveLightList(i);
        //    }
        //}
    }

    private void ChangeColor()
    {
        // �������x���̌v�Z
        int receiveLightLv = m_nowLightLevel;

        foreach(var light in m_lights)
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

        //foreach (ObjectLight light in m_lightList)
        //{
        //    // ���邳�ő�Ȃ珈���I��
        //    if (receiveLightLv >= MAX_BRIGHTNESS)
        //        break;

        //    // null�Ȃ�X�L�b�v
        //    if (light == null)
        //        continue;

        //    // �󂯂����x���������ȉ��Ȃ�X�L�b�v
        //    if (receiveLightLv >= light.LightLevel)
        //        continue;

        //    // ���������v�Z
        //    float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(light.transform.position), transform.position));

        //    receiveLightLv = Mathf.Max(receiveLightLv, light.LightLevel - (int)lightLength);
        //}

        // �������x���ݒ�
        m_nowLightLevel = receiveLightLv;

        // �u���b�N�����݂���Ȃ�
        if (m_affectLight && !m_affectLight.IsDestroyed())
        {
            m_affectLight.ReceiveLightLevel = m_nowLightLevel;
        }
    }

    //   private void ChangeColor()
    //{
    //	//��O����
    //	if ((m_lightList.Count == 0 || !m_lightList.Any()/* || m_lightList[0] == null || gameObject == null*/))
    //	{
    //		ChangeBlack();
    //		return;
    //	}
    //	else
    //	{
    //           //  ���������x��
    //           int receiveLightLv = 0;
    //           if (m_objlight) receiveLightLv = m_objlight.LightLevel;

    //           for (int i = 0; i < m_lightList.Count; i++)
    //		{
    //               //  ���邳�ő�Ȃ珈���I��
    //               if (receiveLightLv >= MAX_BRIGHTNESS) break;

    //               //  null�Ȃ�X�L�b�v
    //               if (m_lightList[i] == null) continue;

    //               //  �󂯂����x���������ȉ��Ȃ�X�L�b�v
    //               if (receiveLightLv >= m_lightList[i].LightLevel) continue;

    //               //  ���������v�Z
    //               float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].transform.position), this.transform.position));

    //               receiveLightLv = math.max(receiveLightLv, m_lightList[i].LightLevel - (int)lightLength);
    //           }

    //           //  �������x���ݒ�
    //           m_nowLightLevel = receiveLightLv;

    //           //  �u���b�N�����݂���Ȃ�
    //           if (m_affectLight && !m_affectLight.IsDestroyed())
    //           {
    //               m_affectLight.ReceiveLightLevel = m_nowLightLevel;
    //           }
    //	}
    //}

    // ���g���Â�����
    private void ChangeBlack()
	{
		if (m_affectLight)
		{
			m_affectLight.ReceiveLightLevel = 0;
		}
	}

    // �����̃I�u�W�F�N�g�����ɂ���(Light�v���n�u)
    bool CheckForObjectInList(GameObject obj)
	{
		// ���X�g���̊e�I�u�W�F�N�g���`�F�b�N
		foreach (ObjectLight item in m_lightList)
		{
			if (item == null)
				continue;

			// �����I�u�W�F�N�g�����������ꍇ��true��Ԃ�
			if (item.gameObject == obj)
			{
				return true;
			}
		}
		// �����I�u�W�F�N�g��������Ȃ������ꍇ��false��Ԃ�
		return false;
	}


	private void AddLightList(GameObject lightObj)
	{
		m_lightList.Add(lightObj.GetComponent<ObjectLight>());
	}
	private void RemoveLightList(int num)
	{
		m_lightList.RemoveAt(num);
	}

	//  �v���C���[���W�n�ݒ�
	public void SetPlayerTransform(Transform tr) { m_playerTr = tr; }
	public Transform GetPlayerTransform() { return m_playerTr; }

	public /*Block*/ObjectAffectLight AffectLight
	{
		get { return m_affectLight; }
		set { m_affectLight = value; }
	}
}
