using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    //  �v���C���[
    [Header("�v���C���[���")]
    [SerializeField] Player m_player;

    //  �A�C�e��UI�̃v���n�u
    [Header("�A�C�e��UI�̃v���n�u")]
    [SerializeField] GameObject m_itemGraph;

    //  �f�o�b�O�p
    [Header("�f�o�b�O�p")]
    public bool m_debug = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //  �v���C���[��������Ȃ�������f�o�b�O��Ԃ�
        if (m_player == null) m_debug = true;
    }
}
