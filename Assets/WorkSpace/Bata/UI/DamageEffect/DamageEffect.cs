using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [Header("���o�p�摜")]
    [SerializeField] private Image m_img = null;

    [Header("���_���[�W�F")]
    [SerializeField] private Color m_dmgColor = Color.white;
    [Header("�Z�_���[�W�F")]
    [SerializeField] private Color m_armorDmgColor = Color.white;

    [Header("�t�F�[�h�A�E�g����")]
    [SerializeField] private float m_fadeOutTime = 0.0f;

    //  �V�[���}�l�[�W���[
    private PlaySceneManager m_playSceneManager;
    //  �v���C���[
    private Player m_player;

    //  ���ԃJ�E���g�p
    private float m_timeCount = 0.0f;

    /// <summary>
    /// �J�n��
    /// </summary>
    private void Start()
    {
        //  �v���C�V�[���}�l�[�W���[�ݒ�
        SetPlaySceneManager(GetComponentInParent<PlaySceneUICanvas>().GetPlaySceneManager());

        m_player.DmgEvents.AddListener(GenerateEffect);
    }

    // Update is called once per frame
    private void Update()
    {
        //  ���Ԍ��Z
        if(m_timeCount > 0) m_timeCount = Mathf.Max(m_timeCount - Time.deltaTime, 0.0f);

        //  �G�t�F�N�g�F
        Color col = m_img.color;
        col.a = m_timeCount / m_fadeOutTime;
        m_img.color = col;

        if(Input.GetKey(KeyCode.P))
        {
            GenerateEffect();        
        }
    }

    /// <summary>
    /// �_���[�W��H��������̃G�t�F�N�g�Đ�
    /// </summary>
    private void GenerateEffect()
    {
        //if (isArmor) m_img.color = m_armorDmgColor;
        //else
        m_img.color = m_dmgColor;
        m_timeCount = m_fadeOutTime;
    }

    //  �v���C�V�[���ݒ�
    public void SetPlaySceneManager(PlaySceneManager playSceneManager)
    {
        m_playSceneManager = playSceneManager;

        //  �v���C�V�[���}�l�[�W���[������������i�[���Ȃ�
        if (m_playSceneManager == null)
            UnityEngine.Debug.Log("Error:Player�̊i�[�Ɏ��s PlaySceneManager��������܂���:ToolUI");
        else
        {
            //  �v���C���[�i�[
            m_player = m_playSceneManager.Player;
        }
    }
}
