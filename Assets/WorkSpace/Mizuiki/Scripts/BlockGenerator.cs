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

    [Header("�n��")]
    [SerializeField] private GameObject m_ground = null;


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
    /// <param name="isBrightness">���邳�����邩�ǂ���</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position, Transform parent = null, bool isBrightness = false)
    {
		// �n�ʂ𐶐�
		GameObject ground = CreateObject(parent, m_ground, position);
 
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
            obj = CreateObject(parent, data.Prefab, position);
        }

		// �摜�̐ݒ�
		if (data.Sprite)
		{
			if (obj.TryGetComponent(out SpriteRenderer sprite))
			{
				sprite.sprite = data.Sprite;
			}
		}

		//���邳�̒ǉ�
		if (isBrightness)
        {
            obj.AddComponent<ChangeBrightness>();
        }

        // �f�[�^�̐ݒ�
        if (!obj.TryGetComponent(out Block block))
            return obj;

        // �f�[�^
        block.BlockData = data;
        // �ϋv
        block.Endurance = data.Endurance;
        // �j��s��
        block.DontBroken = data.DontBroken;
        // �������x��
        block.LightLevel = data.LightLevel;

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
            // �X�v���C�g�̐ݒ�
            map.ParentSprite = obj.gameObject.GetComponent<SpriteRenderer>();
            // �e�̐ݒ�
            map.Parent = block;
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
}