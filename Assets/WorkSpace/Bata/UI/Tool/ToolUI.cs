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
    [SerializeField] private ToolDataBase m_toolDataBase;

    //  UI�\���c�[����
    [Header("UI�ɕK�v�Ȑ�")]
    [SerializeField] private int m_graphToolNum;
    [SerializeField, Range(0.0f, 1.0f)] private float m_graphScaleDeg;

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

        //  ���S�̔ԍ�
        int centerNum = m_graphToolNum / 2;

        float totalWidth = 0.0f;

        //  �X���C�_�[��
        for (int i = 0; i < m_graphToolNum; i++)
        {
            //  ���W
            pos = new Vector3(totalWidth, 0.0f) + transform.position;
            //  UI����
            GameObject frame = Instantiate(m_toolFrame, pos, Quaternion.identity, transform);

            //  �傫���J�E���g
            if (i != centerNum)
                totalWidth += m_toolFrame.GetComponent<RectTransform>().sizeDelta.x * m_graphScaleDeg + m_offset.x;
            else
                totalWidth += m_toolFrame.GetComponent<RectTransform>().sizeDelta.x + m_offset.x;

            //  �ǉ�
            m_toolObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  �X���C�_�[��
        //  ���A�t���O
        bool isRare = m_player.GetComponent<PlayerTool>().IsRareTool;
        //  �c�[���̐�
        ToolData.ToolType playerToolType;
        int centerNum = m_toolObjects.Count / 2;

        //  ID
        int minID = 0;
        int maxID = 0;
        if (isRare)
        {
            minID = (int)ToolData.ToolType.RARE + 1;
            maxID = m_toolDataBase.toolData.Count - (int)ToolData.ToolType.RARE;
            //playerToolType = m_player.GetComponent<PlayerTool>().ToolTypeRare;
            playerToolType = m_player.GetComponent<PlayerTool>().ToolType;
        }
        else
        {
            minID = 0;
            foreach (var toolData in m_toolDataBase.toolData)
            {
                //  ������Ȃ��Ȃ�����X�L�b�v
                if (toolData.Key == ToolData.ToolType.NORMAL_NUM) break;

                maxID++;
            }
            playerToolType = m_player.GetComponent<PlayerTool>().ToolType;
        }

        //  �c�[����ԍ��ɕϊ�
        int playerToolNum = (int)playerToolType;

        //  �f�o�b�O
        if (m_debug) Debug.Log("���݂̃c�[��:" + playerToolType);

        for (int i = 0; i < m_toolObjects.Count; i++)
        {
            //  �Ή��c�[��
            int thisToolID = playerToolNum - (centerNum - i);
            //  �I�[�o�[���Ă���C��
            if (thisToolID < minID) thisToolID = maxID + thisToolID;
            else if (thisToolID >= maxID) thisToolID = thisToolID - maxID;
            //  �^�C�v�ɕϊ�
            ToolData.ToolType thisToolType = (ToolData.ToolType)thisToolID;

            //  �傫���ύX
            m_toolObjects[i].transform.localScale = Vector3.one * m_graphScaleDeg;

            //  �c�[�����ݒ�
            m_toolObjects[i].GetComponent<ToolFrame>().SetIsSelected(false);
            //  �c�[���摜�ݒ�
            //m_toolObjects[i].GetComponent<ToolFrame>().SetImage(m_toolDataBase.tool[thisToolID].sprite);
            m_toolObjects[i].GetComponent<ToolFrame>().SetImage(m_toolDataBase.toolDic[playerToolType].sprite);
            //  �c�[���쐬�\���ݒ�
            //m_toolObjects[i].GetComponent<ToolFrame>().SetNum(GetToolUseNum(thisToolID));
            m_toolObjects[i].GetComponent<ToolFrame>().SetNum(GetToolUseNum(playerToolType));

            //  �N�[���^�C��������Ȃ�0.0�`1.0�ɕ��
            //if (m_toolDataBase.tool[thisToolID].recastTime > 0)
            if (m_toolDataBase.toolDic[playerToolType].recastTime > 0)
            {
                //m_toolObjects[i].GetComponent<ToolFrame>()
                //.GetRecastImage().fillAmount =
                //m_player.GetComponent<PlayerAction>()
                //.GetToolRecast(thisToolType) /
                //m_toolDataBase.tool[thisToolID].recastTime;
                m_toolObjects[i].GetComponent<ToolFrame>()
                .GetRecastImage().fillAmount =
                m_player.GetComponent<PlayerAction>()
                .GetToolRecast(playerToolType) /
                m_toolDataBase.toolDic[playerToolType].recastTime;
            }
            else
            {
                m_toolObjects[i].GetComponent<ToolFrame>()
                    .GetRecastImage().fillAmount = 0.0f;
            }
        }

        //  �c�[���I����ԎQ��
        m_toolObjects[centerNum].GetComponent<ToolFrame>().SetIsSelected(true);
        m_toolObjects[centerNum].transform.localScale = Vector3.one;
    }

    //  �c�[���쐬�\���擾
    public int GetToolUseNum(int toolType)
    {
        //  �\��
        int num = int.MaxValue;

        //  �A�C�e�����擾
        for (int i = 0; i < m_toolDataBase.tool[toolType].itemMaterials.Count; i++)
        {
            //  �A�C�e��
            ItemData.Type type = m_toolDataBase.tool[toolType].itemMaterials[i].type;
            int count = m_toolDataBase.tool[toolType].itemMaterials[i].count;

            //  �����A�C�e��������쐬�\��������o��
            num = Mathf.Min(num,
                m_player.transform.Find("Item").gameObject.
                GetComponent<PlayerItem>().Items[type] / count);
        }

        return num;
    }
    //  �c�[���쐬�\���擾
    public int GetToolUseNum(ToolData.ToolType toolType)
    {
        //  �\��
        int num = int.MaxValue;

        //  �A�C�e�����擾
        for (int i = 0; i < m_toolDataBase.toolDic[toolType].itemMaterials.Count; i++)
        {
            //  �A�C�e��
            ItemData.Type type = m_toolDataBase.toolDic[toolType].itemMaterials[i].type;
            int count = m_toolDataBase.toolDic[toolType].itemMaterials[i].count;

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
