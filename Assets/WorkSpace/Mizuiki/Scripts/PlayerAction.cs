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

	[Header("���C���[�}�X�N")]
	[SerializeField] private LayerMask m_layerMask;


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
		// �������擾���Ă���
		float length = playerToMouse.magnitude;
		// �x�N�g�����K��
		playerToMouse.Normalize();
		// �v���C���[����̌@�����ւ�RayCast
		RaycastHit2D rayCast = Physics2D.Raycast(playerPos, playerToMouse, length, m_layerMask);

		Debug.Log(rayCast.point);
		// �u���b�N�ɓ�������
		if (rayCast)
		{
			mousePos = rayCast.point + (rayCast.normal * new Vector2(0.1f, 0.1f));
		}

		//mousePos = rayCast.point;
		//Debug.Log(rayCast.point);

		// �v���C���[�ƃ}�E�X�J�[�\���̈ʒu���ݒu�͈͓�
		if (Vector2.Distance(playerPos, mousePos) < m_itemSettingRange)
		{
			// �l�̌ܓ�����
			mousePos = RoundHalfUp(mousePos);

			// �c�[���̐ݒu�ʒu���}�E�X�J�[�\���̈ʒu�ɂ���
			m_cursorImage.transform.position = mousePos;
		}
		else
		{
			// �͂��ő�͈͂ɐݒ�
			mousePos = playerPos + (playerToMouse * m_itemSettingRange);

			// �l�̌ܓ�����
			mousePos = RoundHalfUp(mousePos);

			// �A�C�e���̐ݒu�ʒu
			m_cursorImage.transform.position = mousePos;
		}

	}

	// �c�[���ݒu
	public void Put()
    {
		// �I������Ă���A�C�e�����쐬�ł��邩
		//if (!CheckCreate(��肽���A�C�e���̎��))

		// �A�C�e����u��
		GameObject tool = Instantiate(m_putItems[0]);
		// ���W�ݒ�
		tool.transform.position = m_cursorImage.transform.position;
    }


	Vector2 RoundHalfUp(Vector2 value)
	{
		value.x = RoundHalfUp(value.x);
		value.y = RoundHalfUp(value.y);

		return value;
	}

	// �l�̌ܓ�
	static float RoundHalfUp(float value)
	{
		// �����_�ȉ��̎擾
		float fraction = value - MathF.Floor(value);

		// �����_�ȉ���0.5����
		if (fraction < 0.5f)
		{
			// �؂�̂Ă�
			return MathF.Floor(value);
		}
		// �؂�グ��
		return MathF.Floor(value) + 1.0f;

	}

}
