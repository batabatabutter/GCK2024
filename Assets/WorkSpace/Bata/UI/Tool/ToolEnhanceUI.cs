using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static PlayerTool;
using static UnityEngine.GridBrushBase;

public class ToolEnhanceUI : MonoBehaviour
{
    [Header("�v���n�u")]
    [SerializeField] GameObject m_toolFrameObj;
    [SerializeField] Vector2 m_toolOffset = Vector2.zero;

    //  �c�[���f�[�^�x�[�X
    [Header("�c�[���̃f�[�^�x�[�X")]
    [SerializeField] private ToolDataBase m_toolDataBase;

    //  ��������UI�i�[
    private List<ToolEnhanceFrame> m_toolFrames = new List<ToolEnhanceFrame>();

    //  �V�[���}�l�[�W���[
    private PlaySceneManager m_playSceneManager;
    //  �v���C���[
    private GameObject m_player;
    private PlayerTool m_plTool;

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
        //  �v���C�V�[���}�l�[�W���[�ݒ�
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        // �c�[���擾
        foreach (Tool tool in m_plTool.ToolScripts.Values)
        {
            //  �@��z�����擾
            if (tool.GetType() == typeof(ToolMining))
            {
                var frame = Instantiate(m_toolFrameObj, transform).GetComponent<ToolEnhanceFrame>();
                frame.transform.localScale = Vector3.zero;
                frame.ToolMining = (ToolMining)tool;
                frame.ToolIcon.sprite = m_toolDataBase.toolDic[((ToolMining)tool).ToolType].Icon;
                m_toolFrames.Add(frame);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        m_toolFrames.Sort((a, b) => (int)a.Bar.transform.localScale.x - (int)b.Bar.transform.localScale.x);

        int count = 0;

        foreach (var frame in m_toolFrames)
        {
            if (frame.Bar.transform.localScale.x > 0.0f)
            {
                frame.transform.localPosition = new Vector2(0.0f, -m_toolOffset.y * count);
                count++;
            }
        }
    }

    //  �v���C�V�[���ݒ�
    public void SetPlaySceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

        //  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
        if (m_playSceneManager == null)
            Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:" + this);
        else
        {
            //  �v���C���[�i�[
            m_player = m_playSceneManager.GetPlayer();
            m_plTool = m_player.GetComponent<PlayerTool>();
        }
        //  �v���C���[��������Ȃ�������f�o�b�O��Ԃ�
        if (m_player == null) m_debug = true;
    }
}
