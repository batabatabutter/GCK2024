using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    //  �V�[���}�l�[�W���[
    [Header("�v���C�V�[���}�l�[�W���[")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    //  �v���C���[
    private GameObject m_player;

    //  HPUI�̃v���n�u
    [Header("HPUI�̃v���n�u")]
    [SerializeField] private GameObject m_hpGauge;
    [SerializeField] private GameObject m_hpGaugeFrame;
    [SerializeField] private Vector2 m_hpOffset;

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;
    public int m_maxHP;
    public int m_nowHP;

    //  HP�i�[
    private List<GameObject> m_hpGaugeObject = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
        if (m_playSceneManager == null)
            Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:DungeonManager");
        else
        {
            //  �v���C���[�i�[
            m_player = m_playSceneManager.GetPlayer();
        }
        //  �v���C���[��������Ȃ�������f�o�b�O��Ԃ�
        if (m_player == null) m_debug = true;

        //  �U��
        int val = 0;
        if (m_debug) val = m_maxHP;
        else val = m_player.GetHashCode();

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
        if (m_debug)
        {
            hpVal = m_nowHP;
            maxHpVal = m_maxHP;
        }
        else
        {
            hpVal = m_player.GetHashCode();
            maxHpVal = m_player.GetHashCode();
        }
        //  UI�ω�
        for (int i = 0; i < maxHpVal; i++)
        {
            //  �̗͂ɉ�����UI�\��
            if (i < hpVal) m_hpGaugeObject[i].SetActive(true);
            else m_hpGaugeObject[i].SetActive(false);
        }
        
        //  �f�o�b�O���
        if (m_debug)
        {
            Debug.Log("HP:" + hpVal);

            //  �̗͑���
            if (Input.GetKeyDown(KeyCode.Q)) m_nowHP = Mathf.Max(m_nowHP - 1, 0);
            if (Input.GetKeyDown(KeyCode.W)) m_nowHP = Mathf.Min(m_nowHP + 1, m_maxHP);
        }

    }
}
