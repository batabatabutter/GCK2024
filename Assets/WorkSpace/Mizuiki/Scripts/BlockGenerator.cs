using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("�����s�y���p�v���n�u")]
    [SerializeField] private GameObject m_pross = null;
    [SerializeField] private bool m_prossFlag = false;

    [Header("�������������邩�ۂ�")]
    [SerializeField] private bool m_isBrightness = false;
    [Header("��������������I�u�W�F�N�g")]
    [SerializeField] private GameObject m_lightObject = null;

    //  �v���C���[�̍��W
    private Transform m_playerTr;

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
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position, Transform parent = null)
    {
        //  �����y��
        Transform pross = null;
        if (m_prossFlag)
        {
            pross = Instantiate(m_pross, position, Quaternion.identity).transform;
            pross.parent = parent;
            parent = pross.transform;
        }

        // �n�ʂ𐶐�
        GameObject ground = Instantiate(m_ground, position, Quaternion.identity, parent);
        ground.GetComponent<ObjectAffectLight>().BrightnessFlag = m_isBrightness;

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
            obj.GetComponent<Block>().Ground = ground.GetComponent<Ground>();
        }
        // �n�ʂ͂Ȃ�
        else
        {
            // �e��ݒ肵�Đ���
            obj = CreateObject(parent, data.Prefab, position);
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
        // �������x��
        block.LightLevel = data.LightLevel;
        // �F
        block.Color = data.Color;

        //  ��������
        // �����̐ݒ�
        if (m_isBrightness)
        {
            // ���������p�̃I�u�W�F�N�g����
            GameObject light = Instantiate(m_lightObject, block.transform);

            // �����R���C�_�[����
            light.GetComponent<ObjectLight>().FlashLight(block.LightLevel);
        }

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
            // �}�b�v�I�u�W�F�N�g��ݒ�
            block.MapObject = map;
        }

        if (m_prossFlag && pross)
        {
            var p = pross.GetComponent<ProcessChild>();
            p.Scripts = new List<MonoBehaviour>(pross.GetComponentsInChildren<MonoBehaviour>().Skip(1));
            p.Collider2Ds = new List<Collider2D>(pross.GetComponentsInChildren<Collider2D>().Skip(1));
            p.Change(false);
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
    public void SetPlayerTransform(Transform tr) { m_playerTr = tr; }
}