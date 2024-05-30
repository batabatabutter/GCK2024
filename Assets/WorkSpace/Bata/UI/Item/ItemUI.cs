using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    //  �A�C�e��UI�̃v���n�u
    [Header("�A�C�e��UI�̃v���n�u")]
    [SerializeField] private GameObject m_itemFrame;
    [SerializeField] private Vector2 m_offset;

    //  �A�C�e���f�[�^�x�[�X
    [Header("�A�C�e���̃f�[�^�x�[�X")]
    [SerializeField] private ItemDataBase m_data;

    //  HP�i�[
    private List<GameObject> m_itemObjects = new List<GameObject>();

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
        Vector2 size = m_itemFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < m_data.item.Count; i++)
        {
            //  ���W
            pos = new Vector3(0.0f, -(size.y + m_offset.y) * i) + transform.position;
            //  UI����
            GameObject frame = Instantiate(m_itemFrame, pos, Quaternion.identity, transform);
            //  �摜�ݒ�
            frame.GetComponent<ItemFrame>().SetImage(m_data.item[i].Sprite);

            m_itemObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  �A�C�e�����X�V
        int graphNum = 0;
        for(int i = 0; i < m_data.item.Count; i++) 
        {
            //  �A�C�e�����ݒ�
            var num = m_player.transform.Find("Item").gameObject.GetComponent<PlayerItem>().Items[m_data.item[i].Type];
            if (num == 0)
            {
                m_itemObjects[i].SetActive(false);
            }
            else
            {
                m_itemObjects[i].SetActive(true);
                Vector2 size = m_itemFrame.GetComponent<RectTransform>().sizeDelta;
                Vector3 pos = new Vector3(0.0f, -(size.y + m_offset.y) * graphNum) + transform.position;
                m_itemObjects[i].GetComponent<ItemFrame>().SetNum(num);
                m_itemObjects[i].transform.position = pos;
                graphNum++;
            }
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
