using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUI : MonoBehaviour
{
    //  �V�[���}�l�[�W���[
    [Header("�v���C�V�[���}�l�[�W���[")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    //  �v���C���[
    private GameObject m_player;

    //  �c�[��UI�̃v���n�u
    [Header("�c�[��UI�̃v���n�u")]
    [SerializeField] private List<Sprite> m_toolGraph;
    [SerializeField] private GameObject m_toolFrame;
    [SerializeField] private Vector2 m_offset;

    //  HP�i�[
    private List<GameObject> m_toolObjects = new List<GameObject>();

    [Header("�c�[���̃f�[�^�x�[�X")]
    [SerializeField] private ToolDataBase m_data;

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
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

        //  UI����
        //  �����ʒu
        Vector2 size = m_toolFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < (int)ToolData.ToolType.OVER; i++)
        {
            //  ���W
            pos = new Vector3((size.x + m_offset.x)* i, 0.0f) + transform.position;
            //  UI����
            GameObject frame = Instantiate(m_toolFrame, pos, Quaternion.identity, transform);
            //  �摜�ݒ�
            frame.GetComponent<ToolFrame>().SetImage(m_toolGraph[i]);

            m_toolObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  �c�[���X�V
        for (int i = 0; i < (int)ToolData.ToolType.OVER; i++)
        {
            //  �c�[�����ݒ�
            m_toolObjects[i].GetComponent<ToolFrame>().SetIsSelected(false);
            //  �c�[���쐬�\���ݒ�
            m_toolObjects[i].GetComponent<ToolFrame>().SetNum(GetToolUseNum((ToolData.ToolType)i));
        }

        //  �c�[���I����ԎQ��
        m_toolObjects[0].GetComponent<ToolFrame>().SetIsSelected(true); 
    }

    //  �c�[���쐬�\���擾
    public int GetToolUseNum(ToolData.ToolType toolType)
    {
        //  �\��
        int num = 0;

        //  �A�C�e�����擾
        for (int i = 0; i < m_data.tool[(int)toolType].itemMaterials.Count; i++)
        {

        }

        return num;
    }
}
