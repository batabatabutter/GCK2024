using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //�Ƃ肠�����F���������
    SpriteRenderer spriteRenderer;
    //���邳�̍ő�l
    static private int MAX_BRIGHTNESS = 7;
    public int GetMAXBRIGHTBESS() { return MAX_BRIGHTNESS; }

    //LightList
    public List<GameObject> m_lightList = new List<GameObject>();
    public List<Vector3> m_lightPositionList = new List<Vector3>();

    //  �v���C���[�̍��W
    private Transform m_playerTr;
    //  ��������
    private const float DISTANCE_LIGHT = 17.0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //��������
        ChangeBlack();
        //�����͌���
        FlashLight();

    }

    // Update is called once per frame
    void Update()
    {
        //  �v���C���[�Ƃ̋��������ȏ㗣��Ă����珈�����Ȃ�
        if (m_playerTr == null)
        {
            //Debug.Log("Error:Block��Plyer���W�������ĂȂ��F" + this);
            return;
        }
        if (Vector2.Distance(transform.position, m_playerTr.position) > DISTANCE_LIGHT)
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
            return;
        }
        else
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = true;

            }
        }

        //�����̏��������Ȃ�
        if (GetComponent<Block>())
        {
            if (GetComponent<Block>().LightLevel > 0)
            {
                return;
            }
        }

        //���C�g���X�g�̊Ǘ�
        for (int i = 0; i < m_lightList.Count; i++)
        {
            //�����Ȃ��������
            if (m_lightList[i] == null)
            {
                RemoveLightList(i);
            }
            //���ꂽ�����(3�͓K���ɑ傫�߂ɂ���deleteLength�I��)
            else if (Mathf.Abs(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].gameObject.transform.position), gameObject.transform.position)) > m_lightList[i].GetComponent<Block>().LightLevel + 3)
            {
                RemoveLightList(i);
            }
        }

        //�F�̕ύX
        ChangeColor();
            

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //�������g���u���b�N��
        if (GetComponent<Block>())
        {
            //�c�[���i�����j���u���b�N�@�Ł@���g����������Ȃ��@���@�������x�����O���傫��
            if ((collision.gameObject.layer == 3 || collision.CompareTag("Block")) && GetComponent<Block>().LightLevel < 1 && collision.gameObject.GetComponent<Block>().LightLevel > 0)
            {
                //�V�K�����Ȃ�ǉ�����
                if (!CheckForObjectInList(collision.gameObject))
                {
                    AddLightList(collision.gameObject);
                }
            }
        }
        //���g���u���b�N����Ȃ���
        else if ((collision.gameObject.layer == 3 || collision.CompareTag("Block")) && collision.GetComponent<ChangeBrightness>())
        {
            //�V�K�����Ȃ�ǉ�����
            if (!CheckForObjectInList(collision.gameObject))
            {
                AddLightList(collision.gameObject);
            }
        }
    }

    private void ChangeColor()
    {
        //�����͂��̏��������Ȃ�
        if (GetComponent<Block>())
        {
            if (GetComponent<Block>().LightLevel > 0)
            {
                return;
            }
        }

        //HSV�̃J���[�̓f���o���p
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;


        //��O����
        if ((m_lightList.Count == 0 || !m_lightList.Any() || m_lightList[0] == null || gameObject == null))
        {
            ChangeBlack();
            return;
        }
        else
        {
            List<float> lightListV = new List<float>();

            for (int i = 0; i < m_lightList.Count; i++)
            {
                if (m_lightList[i] == null)
                    continue;

                float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].transform.position), this.transform.position));

                lightListV.Add(m_lightList[i].GetComponent<Block>().LightLevel - lightLength);


            }
            //�Ȃ�ł��킩��񂯂�2����Ȃ��ƃo�O��
            for (int i = 0; i < 2; i++)
            {
                //(v�̖��x���O�`�P�O�O�Ȃ̂�)�P�O�O����ɂ����ꃁ�������Z�o
                int rate = 100 / MAX_BRIGHTNESS;
                //�����O�D�O�`�P�D�O�̊ԂȂ̂��ȁH
                v = (rate * lightListV.Max()) * 0.01f;
                //hsv��rgb�ɕϊ�
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }


            if (GetComponent<Block>())
            {
                GetComponent<Block>().ReceiveLightLevel = (int)lightListV.Max();
            }
        }
    }

    private void ChangeBlack()
    {
        //HSV�̃J���[�̓f���o���p
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgb��hsv�ɕϊ�
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        v = 0.0001f;

        //hsv��rgb�ɕϊ�
        spriteRenderer.color = Color.HSVToRGB(h, s, v);


    }
    private void FlashLight()
    {
        //�u���b�N�X�N���v�g�݂Ȃ�
        if (GetComponent<Block>())
        {
            int lightLevel = GetComponent<Block>().LightLevel;
            //���邳���x���������Ă邩
            if (lightLevel > 0)
            {
                //��u�R���C�_�[�W�J
                StartCoroutine(AddCricleColToDelete());

                //HSV�̃J���[�̓f���o���p
                float h = 0.0f;
                float s = 0.0f;
                float v = 0.0f;
                //rgb��hsv�ɕϊ�
                Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

                //(v�̖��x���O�`�P�O�O�Ȃ̂�)�P�O�O����ɂ����ꃁ�������Z�o
                int rate = 100 / MAX_BRIGHTNESS;


                //�����O�D�O�`�P�D�O�̊ԂȂ̂��ȁH
                v = (rate * lightLevel) * 0.01f;

                //hsv��rgb�ɕϊ�
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }
        }
    }


    private IEnumerator AddCricleColToDelete()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        //�����Ȃ�������
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;

        }
        //�~�̃R���C�_�[
        CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();

        //���邳���x���ő傫���w��
        circleCol.radius = GetComponent<Block>().LightLevel;
        circleCol.isTrigger = true;

        yield return new WaitForSeconds(0.0f);

        //Destroy(circleCol);

    }


    bool CheckForObjectInList(GameObject obj)
    {
        // ���X�g���̊e�I�u�W�F�N�g���`�F�b�N
        foreach (GameObject item in m_lightList)
        {
            // �����I�u�W�F�N�g�����������ꍇ��true��Ԃ�
            if (item == obj)
            {
                return true;
            }
        }
        // �����I�u�W�F�N�g��������Ȃ������ꍇ��false��Ԃ�
        return false;
    }


    private void AddLightList(GameObject lightObj)
    {
        m_lightList.Add(lightObj);
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
}
