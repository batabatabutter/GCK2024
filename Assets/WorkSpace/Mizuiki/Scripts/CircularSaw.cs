using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
	[System.Serializable]
	public enum MiningType
	{
		NORMAL,		// �o�����X�^
		RANGE,		// �͈͌^
		POWER,		// �p���[�^
		SPEED,		// �X�s�[�h�^
		CRITICAL,	// �N���e�B�J���^
		DROP,		// �h���b�v�^

		OVER,
	}

	[System.Serializable]
	public struct CircularSawSprite
	{

	}

	[Header("�ۂ̂��̉摜")]
	[SerializeField] private List<Sprite> m_sprites = new();

	[Header("�X�v���C�g�̐ݒ��")]
	[SerializeField] private SpriteRenderer m_spriteRenderer = null;



	private void Start()
	{
		// �X�v���C�g�����_�[���Ȃ���ΐݒ�
		if (m_spriteRenderer == null)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}

	}

	// �̂��̎�ސݒ�
	public void SetType(MiningType type)
	{

	}

}
