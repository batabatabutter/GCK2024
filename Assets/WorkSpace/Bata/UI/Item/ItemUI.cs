using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ItemUI : MonoBehaviour
{
    //  �V�[���}�l�[�W���[
    [Header("�v���C�V�[���}�l�[�W���[")]
    [SerializeField] private PlaySceneManager m_playSceneManager;

    //  �v���C���[
    private GameObject m_player;

    //  �A�C�e��UI�̃v���n�u
    [Header("�A�C�e��UI�̃v���n�u")]
    [SerializeField] private List<Sprite> m_itemGraph;
    [SerializeField] private GameObject m_itemFrame;
    [SerializeField] private Vector2 m_hpOffset;

    //  HP�i�[
    private List<GameObject> m_itemObjects = new List<GameObject>();

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;

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

        //  UI����
        //  �����ʒu
        Vector2 size = m_itemFrame.GetComponent<RectTransform>().sizeDelta;
        Vector3 pos;
        for (int i = 0; i < (int)Item.Type.OVER; i++)
        {
            //  ���W
            pos = new Vector3(0.0f, -(size.y + m_hpOffset.y) * i) + transform.position;
            //  UI����
            GameObject frame = Instantiate(m_itemFrame, pos, Quaternion.identity, transform);
            //  �摜�ݒ�
            frame.GetComponent<ItemFrame>().SetImage(m_itemGraph[i]);

            m_itemObjects.Add(frame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  �A�C�e�����X�V
        for(int i = 0; i < (int)Item.Type.OVER; i++) 
        {
            //  �A�C�e�����ݒ�
            m_itemObjects[i].GetComponent<ItemFrame>().SetNum(
                m_player.GetComponent<PlayerItem>().Items[(Item.Type)i]);
        }
    }
}
