using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ObjectLight : MonoBehaviour
{
	[Header("�������x��")]
	[SerializeField] private int m_lightLevel = 0;

	private void Awake()
	{
		FlashLight(m_lightLevel);
	}

	public void FlashLight(int lightLevel)
	{
		// ���邳���x���̐ݒ�
		m_lightLevel = lightLevel;

		// ���邳���x���������Ă邩
		if (lightLevel > 0)
		{
			// �R���C�_�[�ݒ�
			AddCricleColToDelete(lightLevel);
		}
	}


	private void AddCricleColToDelete(int lightLevel)
	{
		//// ���W�b�h�{�f�B���Ȃ���Βǉ�
		//if (!gameObject.GetComponent<Rigidbody2D>())
		//{
		//	Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		//	rb.isKinematic = true;

		//}

		// �~�̃R���C�_�[���Ȃ���Βǉ�
		if (!gameObject.GetComponent<CircleCollider2D>())
		{
			CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();

			//���邳���x���ő傫���w��
			circleCol.radius = lightLevel;
			circleCol.isTrigger = true;
		}
	}


	public int LightLevel
	{
		get { return m_lightLevel; }
		set {  m_lightLevel = value; }
	}

}
