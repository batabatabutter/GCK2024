using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
	//���邳�̍ő�l
	static private int MAX_BRIGHTNESS = 7;
    public int GetMAXBRIGHTBESS() { return MAX_BRIGHTNESS; }


	//LightList
	public List<ObjectLight> m_lightList = new();
    public List<Vector3> m_lightPositionList = new();

    //  �v���C���[�̍��W
    private Transform m_playerTr;
    //  ���������蔻��
    //private Collider[] m_colliders;
    //  �Փ˔���N������
    //private bool m_colldiersFlag = true;
    //  ��������
    private const float DISTANCE_LIGHT = 17.0f;
    [Header("�u���b�N")]
    [SerializeField] private Block m_block;

    // Start is called before the first frame update
    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //��������
        ChangeBlack();
        //  �u���b�N���擾
        //m_block = GetComponent<Block>();

        //  �Փ˔���擾
        //m_colliders = GetComponents<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //�����̏��������Ȃ�
        if (m_block)
        {
            if (m_block.LightLevel > 0)
            {
                return;
            }
        }

        //  �v���C���[�Ƃ̋��������ȏ㗣��Ă����珈�����Ȃ�
        if (m_playerTr == null)
        {
            Debug.Log("Error:Block��Plyer���W�������ĂȂ��F" + this);
            return;
        }
        if (Vector2.Distance(transform.position, m_playerTr.position) > DISTANCE_LIGHT) return;
        
        //if (Vector2.Distance(transform.position, m_playerTr.position) > DISTANCE_LIGHT)
        //{
        //    //  ���肪���Ă���Ȃ�
        //    if (m_colldiersFlag)
        //    {
        //        //  ���������
        //        m_colldiersFlag = false;
        //        foreach (Collider collider in m_colliders)
        //        {
        //            collider.enabled = false;
        //        }
        //    }
        //    return;
        //}
        //else
        //{
        //    //  ���肪���Ă��Ȃ��Ȃ�
        //    if (!m_colldiersFlag)
        //    {
        //        //  �������
        //        m_colldiersFlag = true;
        //        foreach (Collider collider in m_colliders)
        //        {
        //            collider.enabled = true;
        //        }
        //    }
        //}

		//���C�g���X�g�̊Ǘ�
		for (int i = 0; i < m_lightList.Count; i++)
        {
            //�����Ȃ��������
            if (m_lightList[i] == null)
            {
                RemoveLightList(i);
            }
            //���ꂽ�����(3�͓K���ɑ傫�߂ɂ���deleteLength�I��)
            else if (Mathf.Abs(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].gameObject.transform.position), gameObject.transform.position)) > m_lightList[i].LightLevel + 3)
            {
                RemoveLightList(i);
            }
        }

        //�F�̕ύX
        ChangeColor();


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �����������̂����C�g�ł͂Ȃ�
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;

        // ���C�g�̃��x���� 0 �ȉ�
        if (collision.gameObject.GetComponent<ObjectLight>().LightLevel <= 0)
            return;

		// �����̒ǉ�
		if (!CheckForObjectInList(collision.gameObject))
		{
			AddLightList(collision.gameObject);
		}

		////���g���u���b�N
		//if (m_block)
  //      {
  //          //�V�K�����Ȃ�ǉ�����
  //          if (!CheckForObjectInList(collision.gameObject))
  //          {
  //              AddLightList(collision.gameObject);
  //          }
  //      }
  //      //���g���u���b�N����Ȃ���(�����A�C�e���̏����H)
  //      else
  //      {
		//	//�V�K�����Ȃ�ǉ�����
		//	if (!CheckForObjectInList(collision.gameObject))
		//	{
		//		AddLightList(collision.gameObject);
		//	}
		//}
	}

    private void ChangeColor()
    {
        ////�����͂��̏��������Ȃ�
        //if (m_block)
        //{
        //    if (m_block.LightLevel > 0)
        //    {
        //        return;
        //    }
        //}

        ////HSV�̃J���[�̓f���o���p
        //float h = 0.0f;
        //float s = 0.0f;
        //float v = 0.0f;


        //��O����
        if ((m_lightList.Count == 0 || !m_lightList.Any()/* || m_lightList[0] == null || gameObject == null*/))
        {
            ChangeBlack();
            return;
        }
        else
        {
            List<float> lightListV = new();

            for (int i = 0; i < m_lightList.Count; i++)
            {
                if (m_lightList[i] == null)
                    continue;

                float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].transform.position), this.transform.position));

                lightListV.Add(m_lightList[i].LightLevel - lightLength);


            }

            if (m_block && !m_block.IsDestroyed() && lightListV.Count() != 0)
            {
                m_block.ReceiveLightLevel = Math.Max((int)lightListV.Max(), 0);
            }
        }
    }

    // ���g���Â�����
    private void ChangeBlack()
    {
        if (m_block)
        {
			m_block.ReceiveLightLevel = 0;
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
        m_lightPositionList.Add(lightObj.transform.position);
    }
    private void RemoveLightList(int num)
    {
        m_lightList.RemoveAt(num);
        m_lightPositionList.RemoveAt(num);
    }

    //  �v���C���[���W�n�ݒ�
    public void SetPlayerTransform(Transform tr) { m_playerTr = tr; }
    public Transform GetPlayerTransform() { return m_playerTr; }

    public Block Block
    {
        get { return m_block; }
        set { m_block = value; }
    }
}
