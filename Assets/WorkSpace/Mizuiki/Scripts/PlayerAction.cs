using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
	[Header("�A�C�e���̐ݒu�͈�(���a)")]
	[SerializeField] private float m_itemSettingRange = 2.0f;

    [Header("�J�[�\���摜")]
    [SerializeField] private GameObject m_cursorImage = null;

	[Header("�ݒu�A�C�e��")]
	[SerializeField] private GameObject[] m_putItems;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// �v���C���[�̈ʒu
		Vector2 playerPos = transform.position;

		// �}�E�X�̈ʒu���擾
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// �v���C���[�̈ʒu����}�E�X�̈ʒu�ւ̃x�N�g��
		Vector2 playerToMouse = mousePos - playerPos;
		// �x�N�g�����K��
		playerToMouse.Normalize();
		// �v���C���[����̌@�����ւ�RayCast
		RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, m_itemSettingRange);

		//mousePos.x = RoundHalfUp(rayCast.point.x);
		//mousePos.y = RoundHalfUp(rayCast.point.y);

		// �l�̌ܓ�����
		mousePos.x = RoundHalfUp(mousePos.x);
		mousePos.y = RoundHalfUp(mousePos.y);

		// �v���C���[�ƃ}�E�X�J�[�\���̈ʒu���ݒu�͈͓�
		if (Vector2.Distance(playerPos, mousePos) < m_itemSettingRange)
		{
			// �l�̌ܓ�����
			mousePos.x = RoundHalfUp(mousePos.x);
			mousePos.y = RoundHalfUp(mousePos.y);

			m_cursorImage.transform.position = mousePos;
		}
		else
		{
			//// �v���C���[�̈ʒu����}�E�X�̈ʒu�ւ̃x�N�g��
			//Vector2 playerToMouse = mousePos - playerPos;
			//// �x�N�g�����K��
			//playerToMouse.Normalize();

			mousePos = playerPos + (playerToMouse * m_itemSettingRange);

			// �l�̌ܓ�����
			mousePos.x = RoundHalfUp(mousePos.x);
			mousePos.y = RoundHalfUp(mousePos.y);

			// �A�C�e���̐ݒu�ʒu
			m_cursorImage.transform.position = mousePos;
		}

	}


	public void Put()
    {
		// �A�C�e����u��
		GameObject tool = Instantiate(m_putItems[0]);
		// ���W�ݒ�
		tool.transform.position = m_cursorImage.transform.position;
    }


	// �l�̌ܓ�
	static float RoundHalfUp(float num)
	{
		// �����_�ȉ��̎擾
		float fraction = num - MathF.Floor(num);

		// �����_�ȉ���0.5����
		if (fraction < 0.5f)
		{
			// �؂�̂Ă�
			return MathF.Floor(num);
		}
		// �؂�グ��
		return MathF.Floor(num) + 1.0f;

	}

}
