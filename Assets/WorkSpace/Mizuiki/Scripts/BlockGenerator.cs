using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [Header("�u���b�N�̃f�[�^�x�[�X")]
    [SerializeField] private BlockDataBase m_blockDataBase = null;

    [Header("�}�b�v�I�u�W�F�N�g")]
    [SerializeField] private GameObject m_mapObject = null;
    [Header("�}�b�v�̖ډB��")]
    [SerializeField] private GameObject m_mapBlind = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// �u���b�N�𐶐�
    /// </summary>
    /// <param name="type">��������u���b�N�̎��</param>
    /// <param name="position">����������W</param>
    /// <param name="parent">�e</param>
    public void GenerateBlock(BlockData.ToolType type, Vector2 position, Transform parent = null)
    {
        // �u���b�N�̃f�[�^�擾
        BlockData data = m_blockDataBase.block[(int)type];

        // ���������u���b�N��ݒ肷��p
        GameObject block = null;

        if (parent)
        {
            // �e��ݒ肵�Đ���
            block = Instantiate(data.prefab, position, Quaternion.identity, parent);
        }
        else
        {
            // �w����W�ɐ���
            block = Instantiate(data.prefab, position, Quaternion.identity);
        }

        if (m_mapObject)
        {
            // �}�b�v�I�u�W�F�N�g�̐���
            GameObject mapObj = Instantiate(m_mapObject, block.transform);
            // �F�̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().color = data.color;
            mapObj.GetComponent<MapObject>().BlockColor = data.color;
            // �\�����̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.order;
            // �X�v���C�g�̐ݒ�
            mapObj.GetComponent<MapObject>().ParentSprite = gameObject.GetComponent<SpriteRenderer>();

        }
		if (m_mapBlind)
		{
			// �}�b�v�̖ډB������
			//Instantiate(m_mapBlind, block.transform);
		}

	}
}