using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
	[System.Serializable]
	public struct CircularSawSprite
	{
		public MiningData.MiningType type;
		public Sprite sprite;
	}

	[Header("�ۂ̂��̈ړ����x")]
	[SerializeField] private float m_circularSawSpeed = 1.0f;
	[Header("�ۂ̂��̉�]���x")]
	[SerializeField] private float m_circularSawRotate = 100.0f;

	[Header("�ۂ̂��̉摜")]
	[SerializeField] private List<CircularSawSprite> m_sprites = new();

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

	// ��
	public void Rotate(float speed)
	{
		transform.localEulerAngles += m_circularSawRotate * speed * Time.deltaTime * Vector3.back;
	}

	// �ۂ̂��̈ʒu�擾
	public Vector3 SetPosition(Vector3 miningPoint)
	{
		// �ۂ̂�����̌@�ʒu�ւ̃x�N�g��
		Vector3 circularSawToMining = miningPoint - transform.position;
		// �ۂ̂��ƍ̌@�ʒu�̋���
		float distance = circularSawToMining.magnitude;

		// ������ 1f �̈ړ��ʈȓ��Ȃ炻�̂܂܍̌@�n�_��Ԃ�
		if (distance <= m_circularSawSpeed * Time.deltaTime)
			return miningPoint;

		// �̌@�ʒu�ւ̃x�N�g�����K��
		circularSawToMining.Normalize();

		// �ۂ̂��̈ʒu��Ԃ�
		return transform.position + (circularSawToMining * Time.deltaTime * m_circularSawSpeed);
	}

	// �̂��̎�ސݒ�
	public void SetType(MiningData.MiningType type)
	{

	}

}
