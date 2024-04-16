using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAreaHealing : BlockTotem
{
    /*
�E�ݒu�^
�E���邳���x��10
�E���a3�}�X�̊Ԃɂ���Ƒ̗͂����񂾂�񕜂��� 2�b��1��
�E�ϋv�l500
�E1�b������50�_���[�W���g�ŐH�炤
     */

    [Header("�񕜗�")]
    [SerializeField] private int m_healingValue = 2;
    [Header("�񕜊Ԋu")]
    [SerializeField] private float m_healingInterval = 2.0f;
    // �񕜂̃^�C�}�[
    private float m_healingTimer = 0.0f;


    // Update is called once per frame
    void Update()
    {
        // ���Ԍo��
        if (m_healingTimer > 0.0f)
        {
			m_healingTimer -= Time.deltaTime;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
        // �v���C���[����Ȃ�
        if (!collision.CompareTag("Player"))
            return;

        // �C���^�[�o����
        if (m_healingTimer > 0.0f)
            return;

        // �v���C���[�̎擾
        if (collision.TryGetComponent(out Player player))
        {
            // �v���C���[�̗͉̑�
            player.Healing(m_healingValue);
            // �C���^�[�o��
            m_healingTimer = m_healingInterval;
            Debug.Log("��");
        }

	}

}
