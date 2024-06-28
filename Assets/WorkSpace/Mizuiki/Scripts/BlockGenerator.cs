using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [Header("�`�����N�Ǘ��N���X")]
    [SerializeField] private ChunkManager m_chunkManager = null;

    [Header("�u���b�N�̃f�[�^�x�[�X")]
    [SerializeField] private BlockDataBase m_blockDataBase = null;

    [Header("�}�b�v�I�u�W�F�N�g")]
    [SerializeField] private GameObject m_mapObject = null;
    [Header("�}�b�v�̖ډB��")]
    [SerializeField] private GameObject m_mapBlind = null;

    [Header("�n��")]
    [SerializeField] private GameObject m_ground = null;

    [Header("�e(�u���b�N�̖ډB��)")]
    [SerializeField] private GameObject m_shadow = null;
    // �e�̐e
    private GameObject m_shadowParent = null;

	//�u���b�N�̐e
	private GameObject m_blockParent = null;




    private void Awake()
	{
		// �u���b�N�f�[�^�x�[�X���Ȃ�
		if (m_blockDataBase == null)
		{
            Debug.Log(gameObject.name + "�Ƀu���b�N�f�[�^�x�[�X��ݒ肵�Ă�");
		}
	}

    /// <summary>
    /// �u���b�N�𐶐�(type��OVER��ݒ肷��ƒn�ʂ݂̂����������)
    /// </summary>
    /// <param name="type">��������u���b�N�̎��</param>
    /// <param name="position">����������W</param>
    /// <param name="parent">�e</param>
    /// <param name="isBlockBrightness">�u���b�N���邳�����邩�ǂ���</param>
    /// <param name="isGroundBrightness">�n�ʖ��邳�����邩�ǂ���</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position/*, Transform parent = null*/)
    {
        // �u���b�N�̐e����
        if (m_blockParent == null)
            m_blockParent = new GameObject("Block");
        // �e�̐e����
        if(m_shadowParent == null)
            m_shadowParent = new GameObject("Shadow");
        m_shadowParent.SetActive(false);


        // �e���擾
        Transform blockParent = m_blockParent.transform;
        Transform shadowParent = m_shadowParent.transform;

        // �`�����N�}�l�[�W���[������
        if (m_chunkManager)
        {
            // ���݃`�����N��e�ɂ���
            ChunkManager.Chunk chunk = m_chunkManager.GetChunk(position);
            // �u���b�N�`�����N�̐ݒ�
            blockParent = chunk.blockChunk.transform;
            // �u���b�N�`�����N�̐e�ݒ�
            blockParent.parent = m_blockParent.transform;
            // �e�`�����N�̐ݒ�
            shadowParent= chunk.shadowChunk.transform;
            // �e�`�����N�̐e�ݒ�
            shadowParent.parent = m_shadowParent.transform;
		}

		// �n�ʂ𐶐�
		GameObject ground = Instantiate(m_ground, position, Quaternion.identity, blockParent.transform);
        // �}�b�v�̖ډB������
        Instantiate(m_mapBlind, ground.transform);

        // �e(�u���b�N�̖ډB��)�𐶐�
        Instantiate(m_shadow, position, Quaternion.identity, shadowParent.transform);

        // �u���b�N�̃f�[�^�擾
        BlockData data = MyFunction.GetBlockData(m_blockDataBase, type);

        // �f�[�^���Ȃ�
        if (data == null)
            return ground;

        // �v���n�u���ݒ肳��Ă��Ȃ�
        if (!data.Prefab)
        {
            Debug.Log(data.name + "�Ƀu���b�N�̃v���n�u��ݒ肵�Ă�");

            return null;
        }

		// ���������u���b�N��ݒ肷��p
		GameObject obj;
        // �n�ʂ���������Ă���
        if (ground)
        {
            // �n�ʂ�e�ɐݒ肵�Đ���
            obj = CreateObject(ground.transform, data.Prefab, position);
        }
        // �n�ʂ͂Ȃ�
        else
        {
            // �e��ݒ肵�Đ���
            obj = CreateObject(blockParent.transform, data.Prefab, position);
        }

        // �f�[�^�̐ݒ�
        if (!obj.TryGetComponent(out Block block))
            return obj;

		// �摜�̐ݒ�
		if (data.Sprite)
		{
            block.SetSprite(data.Sprite);
		}

        // ���O�̐ݒ�
        block.name = data.Type.ToString() + "_BLOCK";

        // �f�[�^
        block.BlockData = data;
        // �ϋv
        block.Endurance = data.Endurance;
        // �j��s��
        block.DontBroken = data.DontBroken;
        // �߈ˉ\
        block.CanPossess = data.CanPossess;
        //// �������x��
        //block.LightLevel = data.LightLevel;

        // �F�̐ݒ�
        block.SetColor(data.Color);
        //if (obj.TryGetComponent(out SpriteRenderer blockSprite))
        //{
        //    blockSprite.color = data.Color;
        //}

        // �}�b�v����
        if (m_mapObject)
        {
            // �}�b�v�I�u�W�F�N�g�̐���
            GameObject mapObj = Instantiate(m_mapObject, obj.transform);
            MapObject map = mapObj.GetComponent<MapObject>();
            // �F�̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().color = data.Color;
            map.BlockColor = data.Color;
            // �\�����̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.Order;
            //// �}�b�v�I�u�W�F�N�g��ݒ�
            //block.MapObject = map;
        }

        return obj;
	}

    // �I�u�W�F�N�g�𐶐�����
    private GameObject CreateObject(Transform parent, GameObject gameObject, Vector2 position)
    {
        // ��������I�u�W�F�N�g
        GameObject obj;
        // �e���w�肳��Ă���
        if (parent)
        {
            obj = Instantiate(gameObject, position, Quaternion.identity, parent.transform);
        }
        // �e���w�肳��Ă��Ȃ�
        else
        {
            obj = Instantiate(gameObject, position, Quaternion.identity);
        }

		// ���������I�u�W�F�N�g��Ԃ�
		return obj;
    }

    //  �v���C���[���W�n�ݒ�
    public void SetPlayerTransform(Transform player)
    {
        m_chunkManager.Player = player;
    }
}