using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUI : MonoBehaviour
{
    //  �c�[��UI�̃v���n�u
    [Header("�c�[��UI�̃v���n�u")]
    [SerializeField] private GameObject m_toolFrame;
    [SerializeField] private Vector2 m_offset;

    //  �c�[���f�[�^�x�[�X
    [Header("�c�[���̃f�[�^�x�[�X")]
    [SerializeField] private ToolDataBase m_data;

    //  HP�i�[
    private List<GameObject> m_toolObjects = new List<GameObject>();

    //  �V�[���}�l�[�W���[
    private PlaySceneManager m_playSceneManager;
    //  �v���C���[
    private GameObject m_player;

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
        //  �v���C�V�[���}�l�[�W���[�ݒ�
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

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
            frame.GetComponent<ToolFrame>().SetImage(m_data.tool[i].sprite);

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
            m_toolObjects[i].GetComponent<ToolFrame>().SetNum(GetToolUseNum(i));
        }

        //  �c�[���I����ԎQ��
        m_toolObjects[(int)m_player.GetComponent<PlayerAction>().ToolType].
            GetComponent<ToolFrame>().SetIsSelected(true); 
    }

    //  �c�[���쐬�\���擾
    public int GetToolUseNum(int toolType)
    {
        //  �\��
        int num = int.MaxValue;

        //  �A�C�e�����擾
        for (int i = 0; i < m_data.tool[toolType].itemMaterials.Count; i++)
        {
            //  �A�C�e��
            ItemData.Type type = m_data.tool[toolType].itemMaterials[i].type;
            int count = m_data.tool[toolType].itemMaterials[i].count;

            //  �����A�C�e��������쐬�\��������o��
            num = Mathf.Min(num,
                m_player.transform.Find("Item").gameObject.
                GetComponent<PlayerItem>().Items[type] / count);
        }

        return num;
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
