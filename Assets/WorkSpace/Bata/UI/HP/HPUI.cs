using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class HPUI : MonoBehaviour
{
    //  HPUI�̃v���n�u
    [Header("HPUI�̃v���n�u")]
    [SerializeField] private GameObject m_hpGauge;
    [SerializeField] private GameObject m_hpGaugeFrame;
    [SerializeField] private Vector2 m_hpOffset;
    [SerializeField] private UnityEngine.Color m_hpColor;
    [SerializeField] private UnityEngine.Color m_armorColor;
    [SerializeField] private UnityEngine.Color m_emptyColor;

    //  �V�[���}�l�[�W���[
    private PlaySceneManager m_playSceneManager;
    //  �v���C���[
    private GameObject m_player;

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;
    public int m_debugMaxHP;
    public int m_debugNowHP;

    //  HP�i�[
    private List<GameObject> m_hpGaugeObject = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //  �v���C�V�[���}�l�[�W���[�ݒ�
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        //  �U��
        int val = 0;
        if (m_debug) val = m_debugMaxHP;
        else val = m_player.GetComponent<Player>().MaxLife;

        //  UI����
        //  �����ʒu
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < val; i++)
        {
            //  ���W
            pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
            //  UI����
            GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
            frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
            m_hpGaugeObject.Add(frame);
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
        hpVal = m_player.GetComponent<Player>().HitPoint;
        maxHpVal = m_player.GetComponent<Player>().MaxLife;
        armorVal = m_player.GetComponent<Player>().Armor;
        
        //  UI�ω�
        //  �����ʒu
        Vector2 size = m_hpGaugeFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < maxHpVal + armorVal; i++)
        {
            //  �ő�HP�ƃA�[�}�[���UI�����Ȃ������琶��
            if (i >= m_hpGaugeObject.Count)
            {
                //  ���W
                pos = new Vector3((size.x + m_hpOffset.x) * i, 0.0f) + transform.position;
                //  UI����
                GameObject frame = Instantiate(m_hpGaugeFrame, pos, Quaternion.identity, transform);
                frame = Instantiate(m_hpGauge, pos, Quaternion.identity, frame.transform);
                m_hpGaugeObject.Add(frame);
            }

            //  HP������ΐF�t��
            if (i < hpVal) m_hpGaugeObject[i].GetComponent<RawImage>().color = m_hpColor;
            else if(i >= maxHpVal && i < maxHpVal + armorVal) m_hpGaugeObject[i].GetComponent<RawImage>().color = m_armorColor;
            else m_hpGaugeObject[i].GetComponent<RawImage>().color = m_emptyColor;
        }

        //  �A�[�}�[

        //  �f�o�b�O���
        if (m_debug)
        {
            //  �̗͑���
            if (Input.GetKeyDown(KeyCode.Q)) m_player.GetComponent<Player>().AddDamage(1);
            if (Input.GetKeyDown(KeyCode.W)) m_player.GetComponent<Player>().AddDamage(-1);
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
            m_player = m_playSceneManager.GetPlayer();
        }
        //  �v���C���[��������Ȃ�������f�o�b�O��Ԃ�
        if (m_player == null) m_debug = true;
    }
}
