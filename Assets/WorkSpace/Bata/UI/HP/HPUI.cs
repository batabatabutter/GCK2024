using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    //  HPUI�̃v���n�u
    [Header("HPUI�̃v���n�u")]
    [SerializeField] private GameObject m_hpGauge;
    [SerializeField] private GameObject m_hpGaugeFrame;
    [SerializeField] private GameObject m_armorGauge;
    [Header("�l")]
    [SerializeField] private Vector2 m_hpOffset;
    [SerializeField] private Vector2 m_armorOutlineWidth;
    [SerializeField] private UnityEngine.Color m_hpColor;
    [SerializeField] private UnityEngine.Color m_emptyColor;

    //  �V�[���}�l�[�W���[
    private PlaySceneManager m_playSceneManager;
    //  �v���C���[
    private Player m_player;

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;
    public int m_debugMaxHP;
    public int m_debugNowHP;

    //  HP�i�[
    private List<GameObject> m_hpGaugeObjects = new List<GameObject>();
    //  �A�[�}�[�i�[
    private GameObject m_armorGaugeObject = null;

    // Start is called before the first frame update
    void Start()
    {
        //  �v���C�V�[���}�l�[�W���[�ݒ�
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        //  �U��
        int val = 0;
        if (m_debug) val = m_debugMaxHP;
        else val = m_player.MaxLife;

        //  UI����
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        //  �A�[�}�[�pUI����
        //  ���W
        pos = new Vector3((size.x + m_hpOffset.x) * ((val - 1) / 2.0f), 0.0f) + transform.position;
        m_armorGaugeObject = Instantiate(m_armorGauge, pos, Quaternion.identity, transform);
        m_armorGaugeObject.SetActive(false);
        //  �����ʒu
        for (int i = 0; i < val; i++)
        {
            //  ���W
            pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
            //  UI����
            GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
            frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
            m_hpGaugeObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  �v���C���[��������Ȃ�������f�o�b�O��Ԃ�
        if (m_player == null) m_debug = true;

        //  HP��Ԃ��Q�Ƃ��Q�[�W�ω�
        int hpVal = 0;
        int maxHpVal = 0;
        int armorVal = 0;
        hpVal = m_player.HitPoint;
        maxHpVal = m_player.MaxLife;
        armorVal = m_player.Armor;
        
        //  UI�ω�
        //  �����ʒu
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < maxHpVal; i++)
        {
            //  �ő�HP�ƃA�[�}�[���UI�����Ȃ������琶��
            if (i >= m_hpGaugeObjects.Count)
            {
                //  ���W
                pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
                //  UI����
                GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
                frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
                m_hpGaugeObjects.Add(frame);
            }

            ////  �A�[�}�[���͕\�����Ȃ��悤��
            //if (i >= maxHpVal + armorVal)
            //    m_hpGaugeObject[i].transform.parent.gameObject.SetActive(false);
            //else m_hpGaugeObject[i].transform.parent.gameObject.SetActive(true);

            //  HP������ΐF�t��
            if (i < hpVal) m_hpGaugeObjects[i].GetComponent<RawImage>().color = m_hpColor;
            //else if(i >= maxHpVal && i < maxHpVal + armorVal) m_hpGaugeObject[i].GetComponent<RawImage>().color = m_armorColor;
            else m_hpGaugeObjects[i].GetComponent<RawImage>().color = m_emptyColor;
        }

        //  �A�[�}�[�������
        if (armorVal > 0)
        {
            //  �\��
            m_armorGaugeObject.SetActive(true);

            //  �����ʒu
            pos = new Vector3((size.x + m_hpOffset.x) * ((maxHpVal - 1) / 2.0f), 0.0f) + transform.position;

            //  �傫��
            size = new Vector2(
                size.x * maxHpVal + m_hpOffset.x * (maxHpVal - 1) + m_armorOutlineWidth.x,
                size.y + m_armorOutlineWidth.y);

            //  ���
            m_armorGaugeObject.transform.position = pos;
            m_armorGaugeObject.GetComponent<RectTransform>().sizeDelta = size;
        }
        else m_armorGaugeObject.SetActive(false);

        //  �f�o�b�O���
        if (m_debug)
        {
            //  �̗͑���
            if (Input.GetKeyDown(KeyCode.Q)) m_player.AddDamage(1);
            if (Input.GetKeyDown(KeyCode.W)) m_player.AddDamage(-1);
        }

    }

    //  �v���C�V�[���ݒ�
    public void SetPlaySceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

        //  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
        if (m_playSceneManager == null)
            Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:ToolUI");
        else
        {
            //  �v���C���[�i�[
            m_player = m_playSceneManager.Player;
        }
        //  �v���C���[��������Ȃ�������f�o�b�O��Ԃ�
        if (m_player == null) m_debug = true;
    }
}
