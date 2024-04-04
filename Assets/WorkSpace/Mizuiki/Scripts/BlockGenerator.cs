using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        // �u���b�N�̃f�[�^�x�[�X���Ȃ���΃A�Z�b�g�Q��
        if (m_blockDataBase == null)
        {
            m_blockDataBase = AssetDatabase.LoadAssetAtPath<BlockDataBase>("Assets/DataBase/Block/BlockDataBase.asset");
        }
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
    /// <param name="isBrightness">���邳�����邩�ǂ���</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position, Transform parent = null, bool isBrightness = false)
    {
        // �u���b�N�̃f�[�^�擾
        BlockData data = MyFunction.GetBlockData(m_blockDataBase, type);

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

        // �u���b�N�̉摜�ݒ�
        if (block.TryGetComponent(out SpriteRenderer sprite))
        {
            if (data.sprite)
            {
				sprite.sprite = data.sprite;
			}
		}

        //���邳�̒ǉ�
        if(isBrightness)
        {
            block.AddComponent<ChangeBrightness>();
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
            mapObj.GetComponent<MapObject>().ParentSprite = block.gameObject.GetComponent<SpriteRenderer>();

        }
		if (m_mapBlind)
		{
			// �}�b�v�̖ډB������
			//Instantiate(m_mapBlind, block.transform);
		}


        return block;
	}
}