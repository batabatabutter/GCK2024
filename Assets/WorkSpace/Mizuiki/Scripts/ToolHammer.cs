using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolHummer : Tool
{
    [Header("�n���}�[�͈̔�")]
    [Header("�n���}�[�̔��a")]
    [SerializeField] private float m_radius = 1.0f;
    [Header("�n���}�[�̊p�x")]
    [SerializeField] private float m_degree = 90.0f;

    [Header("�n���}�[�̍U����")]
    [SerializeField] private float m_power = 100.0f;


	public override void UseTool(GameObject obj)
	{
		base.UseTool(obj);

		// ����(�����l�͏�ɂ��Ă���)
		Vector2 direction = MyFunction.GetFourDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition) - obj.transform.position);

		// �n���}�[�͈̔͂̃u���b�N���擾
		Collider2D[] blocks = Physics2D.OverlapCircleAll(obj.transform.position, m_radius, LayerMask.GetMask("Block"));

        // ��r�p��cos
        float cos = Mathf.Cos(m_degree / 2.0f);

        foreach (Collider2D block in blocks)
        {
            // �v���C���[����u���b�N�ւ̃x�N�g��
            Vector2 playerToBlock = block.transform.position - obj.transform.position;

            // �v���C���[�ƃu���b�N�̓���
            float dot = Vector2.Dot(playerToBlock, direction);

            // �n���}�[�͈̔͊O
            if (dot < cos)
                continue;

            // �u���b�N�X�N���v�g�̎擾
            if (block.TryGetComponent(out Block b))
            {
                // �_���[�W��^����
                b.AddMiningDamage(m_power);
            }

        }

	}

}
