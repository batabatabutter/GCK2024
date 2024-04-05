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


	private void Awake()
	{
		// �u���b�N�f�[�^�x�[�X���Ȃ���Ύ擾����
		if (m_blockDataBase == null)
		{
			m_blockDataBase = AssetDatabase.LoadAssetAtPath<BlockDataBase>("Assets/DataBase/Block/BlockDataBase.asset");
		}
	}

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
    /// <param name="isBrightness">���邳�����邩�ǂ���</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position, Transform parent = null, bool isBrightness = false)
    {
		// �u���b�N�̃f�[�^�擾
		BlockData data = MyFunction.GetBlockData(m_blockDataBase, type);

        // �f�[�^���Ȃ�
        if (data == null)
            return null;

		// ���������u���b�N��ݒ肷��p
		GameObject obj;

        // �v���n�u���ݒ肳��Ă��Ȃ��ꍇ�̓A�Z�b�g�Q�ƂŃu���b�N�������Ă���
        if (!data.prefab)
        {
            data.prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Block.prefab");
        }

        if (parent)
        {
            // �e��ݒ肵�Đ���
            obj = Instantiate(data.prefab, position, Quaternion.identity, parent);
        }
        else
        {
            // �w����W�ɐ���
            obj = Instantiate(data.prefab, position, Quaternion.identity);
        }

        // �f�[�^�̐ݒ�
        if (obj.TryGetComponent(out Block block))
        {
            // �f�[�^
            block.BlockData = data;
            // �ϋv
            block.Endurance = data.endurance;
            // �j��s��
            block.DontBroken = data.dontBroken;
            // �������x��
            block.LightLevel = data.lightLevel;
        }

		// �摜�̐ݒ�
		if (data.sprite)
		{
			if (obj.TryGetComponent(out SpriteRenderer sprite))
			{
				sprite.sprite = data.sprite;
			}
		}

		//���邳�̒ǉ�
		if (isBrightness)
        {
            obj.AddComponent<ChangeBrightness>();
        }


        if (m_mapObject)
        {
            // �}�b�v�I�u�W�F�N�g�̐���
            GameObject mapObj = Instantiate(m_mapObject, obj.transform);
            // �F�̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().color = data.color;
            mapObj.GetComponent<MapObject>().BlockColor = data.color;
            // �\�����̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.order;
            // �X�v���C�g�̐ݒ�
            mapObj.GetComponent<MapObject>().ParentSprite = obj.gameObject.GetComponent<SpriteRenderer>();

        }
		if (m_mapBlind)
		{
			// �}�b�v�̖ډB������
			//Instantiate(m_mapBlind, block.transform);
		}


        return obj;
	}
}