using System.Collections.Generic;
using System.Linq;
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
    private PlayerTool m_plTool;
    private PlayerItem m_plItem;

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
                totalWidth += size.x * m_graphScaleDeg + m_offset.x;
            else
                totalWidth += size.x + m_offset.x;

            //  �ǉ�
            m_toolObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  �X���C�_�[��
        //  �c�[���̐�
        ToolData.ToolType playerToolType;
        int centerNum = m_toolObjects.Count / 2;

        //  ID
        List<ToolData.ToolType> useToolTypes = new List<ToolData.ToolType>(m_plTool.Tools.Keys);
        //  ���A�c�[���Ȃ�
        if (m_plTool.IsRareTool)
        {
            playerToolType = m_plTool.ToolTypeRare;
            useToolTypes.RemoveAll(x => x < ToolData.ToolType.RARE);
        }
        else
        {
            playerToolType = m_plTool.ToolType;
            useToolTypes.RemoveAll(x => x >= ToolData.ToolType.RARE);
        }

        //  ���݂̔ԍ��擾
        int nowNum = 0;
        for (int i = 0; i < useToolTypes.Count; i++) 
        {
            if (playerToolType == useToolTypes[i])
                nowNum = i;
        }


        //  �f�o�b�O
        if (m_debug) Debug.Log("���݂̃c�[��:" + playerToolType);

        for (int i = 0; i < m_toolObjects.Count; i++)
        {
            //  �t���[��
            var toolFrame = m_toolObjects[i].GetComponent<ToolFrame>();

            //  �Ή��c�[��
            int thisToolID = nowNum - (centerNum - i);
            //  �I�[�o�[���Ă���C��
            if (thisToolID < 0) 
                thisToolID = useToolTypes.Count + thisToolID;
            else if (thisToolID >= useToolTypes.Count) 
                thisToolID = thisToolID - useToolTypes.Count;
            //  �^�C�v�ɕϊ�
            ToolData.ToolType thisToolType = useToolTypes[thisToolID];


            //  �傫���ύX
            m_toolObjects[i].transform.localScale = Vector3.one * m_graphScaleDeg;

            //  �c�[�����ݒ�
            toolFrame.SetIsSelected(false);
            //  �c�[���摜�ݒ�
            toolFrame.SetImage(m_toolDataBase.toolDic[thisToolType].Icon);
            //  �c�[���쐬�\���ݒ�
            toolFrame.SetNum(GetToolUseNum(thisToolType));

            //  �N�[���^�C��������Ȃ�0.0�`1.0�ɕ��
            if (m_toolDataBase.toolDic[thisToolType].RecastTime > 0)
            {
                toolFrame.GetRecastImage().fillAmount =
                    m_plTool.RecastTime(thisToolType) / 
                    m_toolDataBase.toolDic[thisToolType].RecastTime;
            }
            else
            {
                toolFrame.GetRecastImage().fillAmount = 0.0f;
            }
        }

        //  �c�[���I����ԎQ��
        m_toolObjects[centerNum].GetComponent<ToolFrame>().SetIsSelected(true);
        m_toolObjects[centerNum].transform.localScale = Vector3.one;
    }

    //  �c�[���쐬�\���擾
    private int GetToolUseNum(ToolData.ToolType toolType)
    {
        //  �\��
        int num = int.MaxValue;

        //  �A�C�e�����擾
        for (int i = 0; i < m_toolDataBase.toolDic[toolType].ItemMaterials.Length; i++)
        {
            //  �A�C�e��
            ItemData.ItemType type = m_toolDataBase.toolDic[toolType].ItemMaterials[i].Type;
            int count = m_toolDataBase.toolDic[toolType].ItemMaterials[i].count;

            //  �����A�C�e��������쐬�\��������o��
            num = Mathf.Min(num, m_plItem.Items[type] / count);
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
            m_plTool = m_player.GetComponent<PlayerTool>();
            m_plItem = m_player.transform.Find("Item").gameObject.GetComponent<PlayerItem>();
        }
        //  �v���C���[��������Ȃ�������f�o�b�O��Ԃ�
        if (m_player == null) m_debug = true;
    }
}
